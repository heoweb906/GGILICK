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
    public ColorType[] ColorCorrects;       // ScanMaster의 정답 컬러
    public Scanner[] scanners;              // 각 스테이지의 Scanner 
    public ScanMaster_ClockWork clockWork;  // ScanMaster 전용 ClockWork




    // 테스트로 사용할 이미지
    public GameObject[] testFaces;


    // #. ScanMaster 메인 스캔 동작
    public void ScanStart()
    {
        StartCoroutine(ScanStart_());
    }
    private IEnumerator ScanStart_()
    {


        if (BoolCheckObjOnScanner())  // 스캔 성공
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







    // #. 스캐너 위의 오브젝트들의 색상이 정답과 일치하는지 검사하는 함수
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


    // #. 스캔 성공!
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
    // #. 스캔 실패 ㅠㅠ
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
    // #. 스캐너 초기 상태로 돌리기
    private void Scan_Reset()
    {
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(true);
        testFaces[2].SetActive(false);

        clockWork.canInteract = true;
    }





}
