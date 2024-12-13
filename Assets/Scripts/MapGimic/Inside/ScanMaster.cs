using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ColorType
{
    None,
    Yellow,
    Red
}

public class ScanMaster : MonoBehaviour
{
    public ColorType[] ColorCorrects;       // ScanMaster�� ���� �÷�
    public Scanner[] scanners;              // �� ���������� Scanner 
    public ScanMaster_ClockWork clockWork;  // ScanMaster ���� ClockWork




    // �׽�Ʈ�� ����� �̹���
    public GameObject[] testFaces;


    // #. ScanMaster ���� ��ĵ ����
    public void ScanStart()
    {
        StartCoroutine(ScanStart_());
    }
    private IEnumerator ScanStart_()
    {


        if (BoolCheckObjOnScanner())  // ��ĵ ����
        {
            Scan_Success();
            yield break;
        }
        else if (!BoolCheckObjOnScanner())
        {
            Scan_Fail();
        }
            
       
        yield return new WaitForSeconds(1f);


        Scan_Reset();
    }







    // #. ��ĳ�� ���� ������Ʈ���� ������ ����� ��ġ�ϴ��� �˻��ϴ� �Լ�
    private bool BoolCheckObjOnScanner()
    {
        for (int i = 0; i < ColorCorrects.Length; i++)
        {
            bool matchFound = false;

            if (scanners[i] != null)
            {
                foreach (var colorObj in scanners[i].GetColorObjList())
                {
                    if (colorObj != null && colorObj.colorType == ColorCorrects[i])
                    {
                        matchFound = true;
                        break;
                    }
                }
            }


            if (!matchFound) return false;
        }

        return true;
    }


    // #. ��ĵ ����!
    private void Scan_Success()
    {
        testFaces[0].SetActive(true);
        testFaces[1].SetActive(false);
        testFaces[2].SetActive(false);


        for(int i = 0; i < scanners.Length; i++)
        {
            scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        }  
    }
    // #. ��ĵ ���� �Ф�
    private void Scan_Fail()
    {
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(false);
        testFaces[2].SetActive(true);


        for (int i = 0; i < scanners.Length; i++)
        {
            scanners[i].ThrowOtherColorObj(ColorCorrects[i]);
        }
    }
    // #. ��ĳ�� �ʱ� ���·� ������
    private void Scan_Reset()
    {
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(true);
        testFaces[2].SetActive(false);

        clockWork.canInteract = true;
    }





}
