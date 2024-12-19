using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOwnerType
{
    Nothing,
    TrafficLightClockWork,
    SoundPiece
}

public class PartsArea : MonoBehaviour
{
    public GameObject Parts;                         // 파츠
    public Transform PartsTransform;                 // 파츠가 들어갈 위치   
    public Transform PartsInteractTransform;         // 파츠를 끼울 수 있는 위치
    public bool BCanInteract;                        // 파츠를 넣을 수 있는 상태인지
    public int iIndex;

    public PartOwnerType PartOwnertype;              // 파츠 타입 구분
    public GameObject partsOwnerObject;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;

        if (partsOwnerObject != null) partsOwner = partsOwnerObject.GetComponent<IPartsOwner>();
        else Debug.Log("여기 비었다 채워라");

    }

    // #. 파츠를 장착했을 때 실행시키는 함수
    public virtual void InsertParts(GameObject partsObj) 
    {
        OffCanInteract();
        Parts = partsObj;

        partsOwner.InsertOwnerFunc(partsObj, iIndex);
    }

    // #. 파츠를 제거했을 때 실행시키는 함수
    public virtual void RemoveParts() 
    {
        OffCanInteract();
        Parts = null;

        partsOwner.RemoveOwnerFunc(iIndex);
    }


    private void OffCanInteract()
    {
        if (PartOwnertype == PartOwnerType.TrafficLightClockWork)
        {
            BCanInteract = false;
        }
    }







}
