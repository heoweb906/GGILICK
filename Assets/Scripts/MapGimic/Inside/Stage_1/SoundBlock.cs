using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBlock : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            PlaySound();
        }
    }

    private void PlaySound()
    {
        SoundAssistManager.Instance.GetEffectAudioBlock("FX_piano_02", transform);
    }
}
