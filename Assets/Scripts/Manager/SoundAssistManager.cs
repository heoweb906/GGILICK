using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SoundAssistManager : MonoBehaviour
{
    public static SoundAssistManager Instance;

    // 딕셔너리로 변경 (파일명을 키로 사용)
    public Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();


    // 풀링 관련
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

    // 사운드 찾기
    #region 

    private void LoadSounds(string folderPath)
    {
        string fullPath = Path.Combine(Application.dataPath, "Resources", folderPath);
        string[] files = Directory.GetFiles(fullPath, "*.mp3", SearchOption.AllDirectories);  // MP3 파일만 로드

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
                // 파일명을 키로 사용하여 딕셔너리에 추가
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



    // 풀링 관련
    #region

    // 오브젝트 풀 초기화: 지정된 수만큼 오브젝트를 미리 생성
    public void InitializeAudioPlayerBlockPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newAudioPlayerBlock = Instantiate(audioPlaterBlockPrefab);
            AudioPlayerBlock audioBlock  = newAudioPlayerBlock.GetComponent<AudioPlayerBlock>();
            newAudioPlayerBlock.transform.SetParent(transform, this);
            newAudioPlayerBlock.SetActive(false);  // 처음에는 비활성화
            audioPlayerBlockPool.Enqueue(newAudioPlayerBlock);

            audioPlayerBlockList.Add(audioBlock);

            Debug.Log("생성됨");
        }
    }


    // 오브젝트 풀에서 사운드 블럭 가져오기 (효과음)
    public GameObject GetEffectAudioBlock(string audioClipName = null, Transform objTransform = null)
    {
        if (audioClipName != null)
        {
            if (audioPlayerBlockPool.Count > 0)
            {
                GameObject audioPlayerBlock = audioPlayerBlockPool.Dequeue();
                audioPlayerBlock.transform.SetParent(objTransform);
                audioPlayerBlock.SetActive(true);  // 가져온 오브젝트 활성화

                AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
                if (soundDictionary.ContainsKey(audioClipName))
                {
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // 오디오 클립 할당
                    audioBlock.PlayAudioClip();  // 오디오 클립 재생
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
                    audioBlock.audioSource.clip = soundDictionary[audioClipName];  // 오디오 클립 할당
                    audioBlock.PlayAudioClip();  // 오디오 클립 재생
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


    // 오브젝트 풀로 오브젝트 반환
    public void ReturnAudioPlayerBlock(GameObject audioPlayerBlock)
    {
        audioPlayerBlock.transform.SetParent(transform, this);
        audioPlayerBlock.SetActive(false);  // 오브젝트를 비활성화

        AudioPlayerBlock audioBlock = audioPlayerBlock.GetComponent<AudioPlayerBlock>();
        audioBlock.audioSource.clip = null;

        audioPlayerBlockPool.Enqueue(audioPlayerBlock);  // 풀로 되돌려보냄
    }



    #endregion







}
