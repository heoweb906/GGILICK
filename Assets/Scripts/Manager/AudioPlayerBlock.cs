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

    // ������� ���� ������ ����� ��, Ǯ�� ��ȯ
    private IEnumerator WaitForAudioEnd()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        SoundAssistManager.Instance.ReturnAudioPlayerBlock(gameObject);
    }

}
