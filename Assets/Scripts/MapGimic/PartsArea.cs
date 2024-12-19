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
    public GameObject Parts;                         // ����
    public Transform PartsTransform;                 // ������ �� ��ġ   
    public Transform PartsInteractTransform;         // ������ ���� �� �ִ� ��ġ
    public bool BCanInteract;                        // ������ ���� �� �ִ� ��������
    public int iIndex;

    public PartOwnerType PartOwnertype;              // ���� Ÿ�� ����
    public GameObject partsOwnerObject;
    private IPartsOwner partsOwner;

    private void Awake()
    {
        BCanInteract = true;

        if (partsOwnerObject != null) partsOwner = partsOwnerObject.GetComponent<IPartsOwner>();
        else Debug.Log("���� ����� ä����");

    }

    // #. ������ �������� �� �����Ű�� �Լ�
    public virtual void InsertParts(GameObject partsObj) 
    {
        OffCanInteract();
        Parts = partsObj;

        partsOwner.InsertOwnerFunc(partsObj, iIndex);
    }

    // #. ������ �������� �� �����Ű�� �Լ�
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
