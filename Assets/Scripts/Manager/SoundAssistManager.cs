using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SoundAssistManager : MonoBehaviour
{
    public static SoundAssistManager Instance { get; private set; }

    // ��ųʸ��� ���� (���ϸ��� Ű�� ���)
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    public AudioMixer audioMixer_Master;


    [Header("AudioBlock_BGM")]
    public AudioSource audioSource_BGM;

    [Header("AudioBlock_SFX")]
    public int iPoolSize;
    public GameObject audioPlaterBlockPrefab; 
    public Queue<GameObject> audioPlayerBlockPool = new Queue<GameObject>();
    public List<AudioPlayerBlock> audioPlayerBlockList = new List<AudioPlayerBlock>();



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSounds("Sounds");
            InitializeAudioPlayerBlockPool(iPoolSize);

            Invoke("AudioMixerSet",0.1f);

            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            UnmuteMasterVolume();

            Destroy(gameObject); // �̹� �����ϴ� �ν��Ͻ��� ������ ���� ������Ʈ �ı�
        }

        
    }

    // ���� ã��
    #region 

    private void LoadSounds(string folderPath)
    {
        //string fullPath = Path.Combine(Application.dataPath, "Resources", folderPath);
        //string[] files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);  // MP3 ���ϸ� �ε�

        //foreach (var file in files)
        //{
        //    string relativePath = "Sounds" + file.Substring(Application.dataPath.Length).Replace("\\", "/").Replace(Path.GetExtension(file), "");

        //    StartCoroutine(LoadMP3AudioClip(file));
        //}

        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderPath);
        foreach (AudioClip clip in clips)
        {
            soundDictionary[clip.name] = clip;
        }
    }

    private IEnumerator LoadMP3AudioClip(string filePath)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    string clipName = Path.GetFileNameWithoutExtension(filePath);
                    soundDictionary[clipName] = clip;
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load sound from {filePath}: {www.error}");
            }
        }
    }

    // #. ���� ��ųʸ��� ��� �ִ� AudioClip Key / Value ���
    public void DebugSoundDictionary()
    {
        foreach (var entry in soundDictionary)
        {
            string clipName = entry.Key;
            AudioClip clip = entry.Value;
            Debug.Log($"Key: {clipName}, Clip: {clip.name}, Length: {clip.length} seconds");
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
        }
    }


    // ������Ʈ Ǯ���� ���� �� �������� (ȿ����)
    public GameObject GetSFXAudioBlock(string audioClipName = null, Transform objTransform = null)
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





    // ���� ���� ����
    #region



    // #. ���� �����Ϳ��� ���� �� �ҷ�����
    public void AudioMixerSet()
    {
        float masterVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetMasterVolume());
        audioMixer_Master.SetFloat("Master", masterVolume);

        float bgmVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetBGMVolume());
        audioMixer_Master.SetFloat("BGM", bgmVolume);

        float sfxVolume = Mathf.Lerp(-80f, 0f, SaveData_Manager.Instance.GetSFXVolume());
        audioMixer_Master.SetFloat("SFX", sfxVolume);

        //bool isMasterMuted = masterVolume <= -30f;
        //audioMixer_Master.SetFloat("MasterMute", isMasterMuted ? 1f : 0f);

        //bool isBGMMuted = bgmVolume <= -50f;
        //.SetFloat("BGMMute", isBGMMuted ? 1f : 0f);

        //bool isSFXMuted = sfxVolume <= -50f;
        //audioMixer_Master.SetFloat("SFXMute", isSFXMuted ? 1f : 0f);


        audioSource_BGM.Play();
    }


    public void MuteMasterVolume()
    {
        float muteVolume = -80f;

        // DOTween�� ����Ͽ� ������ 2�� ���� ������ -80f�� ����
        DOTween.To(() => {
            audioMixer_Master.GetFloat("Master", out float tempMaster);
            return tempMaster;
        },
            x => {
                audioMixer_Master.SetFloat("Master", x);
                // audioMixer_Master.SetFloat("MasterMute", x <= -30f ? 1f : 0f);  // ���� ���� ���� ��Ʈ ó��
            },
            muteVolume, 2f).SetUpdate(true);
    }
    public void UnmuteMasterVolume()
    {
        // ����� ���� ���� �� �������� (0�� 1 ������ ��)
        float originalVolume = SaveData_Manager.Instance.GetMasterVolume();
        float adjustedVolume = Mathf.Lerp(-80f, 0f, originalVolume);

        DOTween.To(() => {
            audioMixer_Master.GetFloat("Master", out float tempMaster);
            return tempMaster;
        },
            x => {
                audioMixer_Master.SetFloat("Master", x);
                //audioMixer_Master.SetFloat("MasterMute", x <= -30f ? 1f : 0f);  // ���� ���� ���� ��Ʈ ó��
            },
            adjustedVolume, 2f).SetUpdate(true);
    }



    // #. �ٸ� �ڵ�鿡�� ȣ���ϰ� ����
    public void BGMChange(string sceneName = null)
    {
        switch (sceneName)
        {
            case "Chapter1_1_City":
            case "Chapter1_2_Subway":
            case "Chapter1_3_City":
            case "Chapter_1_11_Inside":
                if (audioSource_BGM.clip != soundDictionary["TestBGM"])
                {
                    audioSource_BGM.Stop();
                    audioSource_BGM.clip = soundDictionary["TestBGM"];
                    audioSource_BGM.Play(); 
                    Debug.Log("soundDictionary[TestBGM] ���");
                }
                break;

            case "MainMenu":
                if (audioSource_BGM.clip != soundDictionary["TestBGM_2"])
                {
                    audioSource_BGM.Stop();
                    audioSource_BGM.clip = soundDictionary["TestBGM_2"];
                    audioSource_BGM.Play(); 
                    Debug.Log("soundDictionary[TestBGM] ���");
                }
                break;

            default:
                audioSource_BGM.Stop();
                break;
        }

       

    }







    #endregion


}






