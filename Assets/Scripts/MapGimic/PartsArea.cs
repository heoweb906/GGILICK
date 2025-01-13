using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartOwnerType
{
    Nothing,
    TrafficLightClockWork,      // ��ȣ�� 
    SoundPiece,                 // ���� ���
    ToyTruckClockWork           // �峭�� Ʈ���� ������ �¿� 
}

public class PartsArea : MonoBehaviour
{
    public GameObject Parts;                         // ����
    public Transform PartsTransform;                 // ������ �� ��ġ   
    public Transform PartsInteractTransform;         // ������ ���� �� �ִ� ��ġ
    public bool BCanInteract;                        // ������ ���� �� �ִ� ��������
    public int iIndex;

    public PartOwnerType PartOwnertype;              // ���� Ÿ�� ����
    public GameObject[] partsOwnerObjects;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;
    }

    // #. ������ �������� �� �����Ű�� �Լ�
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

    // #. ������ �������� �� �����Ű�� �Լ�
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
