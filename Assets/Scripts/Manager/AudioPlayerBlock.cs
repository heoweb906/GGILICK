using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerBlock : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudioClip()
    {
        audioSource.Play();
        StartCoroutine(WaitForAudioEnd());
    }

    // 오디오가 끝날 때까지 대기한 후, 풀로 반환
    private IEnumerator WaitForAudioEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SoundAssistManager.Instance.ReturnAudioPlayerBlock(gameObject);
    }

}
