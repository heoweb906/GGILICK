using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SoundAssistManager : MonoBehaviour
{
    public static SoundAssistManager Instance;

    // ��ųʸ��� ���� (���ϸ��� Ű�� ���)
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();


    // Ǯ�� ����
    public int iPoolSize;
    public GameObject audioPlaterBlockPrefab; 
    public Queue<GameObject> audioPlayerBlockPool = new Queue<GameObject>();
    public List<AudioPlayerBlock> audioPlayerBlockList = new List<AudioPlayerBlock>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadSounds("Sounds");
        InitializeAudioPlayerBlockPool(iPoolSize);
    }

    // ���� ã��
    #region 

    private void LoadSounds(string folderPath)
    {
        string fullPath = Path.Combine(Application.dataPath, "Resources", folderPath);
        string[] files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);  // MP3 ���ϸ� �ε�

        foreach (var file in files)
        {
            string relativePath = "Sounds" + file.Substring(Application.dataPath.Length).Replace("\\", "/").Replace(Path.GetExtension(file), "");

            StartCoroutine(LoadMP3AudioClip(file));
        }
    }

    private IEnumerator LoadMP3AudioClip(string filePath)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            if (clip != null)
            {
                // ���ϸ��� Ű�� ����Ͽ� ��ųʸ��� �߰�
                string clipName = Path.GetFileNameWithoutExtension(filePath);
                soundDictionary[clipName] = clip;
            }
        }
        else
        {
            Debug.LogWarning($"Failed to load sound from {filePath}: {www.error}");
        }
    }
    #endregion



    // Ǯ�� ����
    #region

    // ������Ʈ Ǯ �ʱ�ȭ: ������ ����ŭ ������Ʈ�� �̸� ����
    public void InitializeAudioPlayerBlockPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newAudioPlayerBlock = Instantiate(audioPlaterBlockPrefab);
            AudioPlayerBlock audioBlock  = newAudioPlayerBlock.GetComponent<AudioPlayerBlock>();
            newAudioPlayerBlock.transform.SetParent(transform, this);
            newAudioPlayerBlock.SetActive(false);  // ó������ ��Ȱ��ȭ
            audioPlayerBlockPool.Enqueue(newAudioPlayerBlock);

            audioPlayerBlockList.Add(audioBlock);

            Debug.Log("������");
        }
    }


    // ������Ʈ Ǯ���� ���� �� �������� (ȿ����)
    public GameObject GetEffectAudioBlock(string audioClipName = null, Transform objTransform = null)
    {
        if (audioClipName != null)
        {
            if (audioPlayerBlockPool.Count > 0)
            {
                GameObject audioPlayerBlock = audioPlayerBlockPool.Dequeue();
                audioPlayerBlock.transform.SetParent(objTransform);
                audioPlayerBlock.SetActive(true);  // ������ ������Ʈ Ȱ��ȭ

                AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
                if (soundDictionary.ContainsKey(audioClipName))
                {
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // ����� Ŭ�� �Ҵ�
                    audioBlock.PlayAudioClip();  // ����� Ŭ�� ���
                }
                else
                {
                    Debug.LogWarning($"Sound with name {audioClipName} not found in the dictionary.");
                }

                return audioPlayerBlock;
            }
            else
            {
                Debug.LogWarning("AudioPlayerBlock Pool is empty! Creating new AudioPlayerBlock.");
                GameObject newAudioPlayerBlock = Instantiate(audioPlaterBlockPrefab);
                newAudioPlayerBlock.transform.SetParent(objTransform);

                AudioPlayerBlock audioBlock = newAudioPlayerBlock.GetComponent<AudioPlayerBlock>();
                if (soundDictionary.ContainsKey(audioClipName))
                {
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // ����� Ŭ�� �Ҵ�
                    audioBlock.PlayAudioClip();  // ����� Ŭ�� ���
                }
                else
                {
                    Debug.LogWarning($"Sound with name {audioClipName} not found in the dictionary.");
                }

                audioPlayerBlockList.Add(audioBlock);

                return newAudioPlayerBlock;
            }
        }

        return null;
    }


    // ������Ʈ Ǯ�� ������Ʈ ��ȯ
    public void ReturnAudioPlayerBlock(GameObject audioPlayerBlock)
    {
        audioPlayerBlock.transform.SetParent(transform, this);
        audioPlayerBlock.SetActive(false);  // ������Ʈ�� ��Ȱ��ȭ

        AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
        audioBlock.audioSource.clip = null;

        audioPlayerBlockPool.Enqueue(audioPlayerBlock);  // Ǯ�� �ǵ�������
    }



    #endregion







}
