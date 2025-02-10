using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOwnerType
{
    Nothing,
<<<<<<< Updated upstream
    TrafficLightClockWork,      // 신호등 
    SoundPiece,                 // 사운드 블록
    ToyTruckClockWork,          // 장난감 트럭에 장착할 태엽 
    StampMachine,               // 도장 찍는 기계
    GameMachine                 // 게임 기계
}

public enum PartsAreaType
{
    Wall,
    Floor
=======
    TrafficLightClockWork,      // ��ȣ�� 
    SoundPiece,                 // ���� ����
    ToyTruckClockWork,          // �峭�� Ʈ���� ������ �¿� 
    StampMachine
>>>>>>> Stashed changes
}

public class PartsArea : MonoBehaviour
{
    public GameObject Parts;                         // 파츠
    public Transform PartsTransform;                 // 파츠가 들어갈 위치   
    public Transform PartsInteractTransform;         // 파츠를 끼울 수 있는 위치
    public bool BCanInteract;                        // 파츠를 넣을 수 있는 상태인지
    public int iIndex;

    public PartOwnerType PartOwnertype;              // 파츠 타입 구분
    public PartsAreaType PartsAreaType;              // 파츠 위치 구분
    public GameObject[] partsOwnerObjects;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;
    }

    // #. 파츠를 장착했을 때 실행시키는 함수
    public virtual void InsertParts(GameObject partsObj)
    {
        OffCanInteract();
        Parts = partsObj;


        foreach (var owner in partsOwnerObjects)
        {
            partsOwner = owner.GetComponent<IPartsOwner>();
            if (partsOwner != null) partsOwner.InsertOwnerFunc(partsObj, iIndex);
        }
    }

    // #. 파츠를 제거했을 때 실행시키는 함수
    public virtual void RemoveParts()
    {
        OffCanInteract();
        Parts = null;


        foreach (var owner in partsOwnerObjects)
        {
            partsOwner = owner.GetComponent<IPartsOwner>();
            if (partsOwner != null) partsOwner.RemoveOwnerFunc(iIndex);
        }
    }


    private void OffCanInteract()
    {
        if (PartOwnertype == PartOwnerType.TrafficLightClockWork)
        {
            BCanInteract = false;
        }
    }







}
