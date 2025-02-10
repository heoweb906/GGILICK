using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOwnerType
{
    Nothing,
    TrafficLightClockWork,      // ��ȣ�� 
    SoundPiece,                 // ���� ���
    ToyTruckClockWork,          // �峭�� Ʈ���� ������ �¿� 
    StampMachine,               // ���� ��� ���
    GameMachine                 // ���� ���
}

public enum PartsAreaType
{
    Wall,
    Floor
}

public class PartsArea : MonoBehaviour
{
    public GameObject Parts;                         // ?�츠
    public Transform PartsTransform;                 // ?�츠가 ?�어�??�치   
    public Transform PartsInteractTransform;         // ?�츠�??�울 ???�는 ?�치
    public bool BCanInteract;                        // ?�츠�??�을 ???�는 ?�태?��?
    public int iIndex;

    public PartOwnerType PartOwnertype;              // ?�츠 ?�??구분
    public PartsAreaType PartsAreaType;              // ?�츠 ?�치 구분
    public GameObject[] partsOwnerObjects;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;
    }

    // #. ?�츠�??�착?�을 ???�행?�키???�수
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

    // #. ?�츠�??�거?�을 ???�행?�키???�수
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
