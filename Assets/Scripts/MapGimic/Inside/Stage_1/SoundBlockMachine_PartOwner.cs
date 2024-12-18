using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBlockMachine_PartOwner : MonoBehaviour, IPartOwner
{
    public Transform partsTransform;
    public Transform partsInteractTransform;
    public int iIndex;
    public SoundBlockMachine SoundBlockMachine_ { get; set; }


    // #. 인터페이스 구현 부분
    public GameObject Parts { get; set; }
    public Transform PartsTransform { get; set; }
    public Transform PartsInteractTransform { get; set; }

    private void Awake()
    {
        PartsTransform = partsTransform;
        PartsInteractTransform = partsInteractTransform;
    }

    public void InsertParts(GameObject partsObj)
    {
        Parts = partsObj;

        SoundBlockMachine_.AddSoundPiece(partsObj, iIndex);
    }

    public void RemoveParts() 
    {
        Parts = null;

        SoundBlockMachine_.RemoveSoundPiece(iIndex);
    }




}
