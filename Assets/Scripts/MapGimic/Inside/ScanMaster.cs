using DG.Tweening;
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
    [Header("스탠 기능 관련")]
    public ColorType[] ColorCorrects;       // ScanMaster의 정답 컬러
    public Scanner[] scanners;              // 각 스테이지의 Scanner 
    public ScanMaster_ClockWork clockWork;  // ScanMaster 전용 ClockWork

    [Header("MasterObj 관련")]
    public GameObject masterObject;         // 스캔에 성공하면 획득할 수 있는 오브젝트
    public Transform transformMasterObj;    // 마스터 오브젝트 생성 위치
        

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
        else
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
        GameObject spawnedObject = Instantiate(masterObject, transformMasterObj.position, Quaternion.identity);

        // Collider와 Rigidbody 참조
        Collider objCollider = spawnedObject.GetComponent<Collider>();
        Rigidbody objRigidbody = spawnedObject.GetComponent<Rigidbody>();

        // 초기 위치를 바닥으로 설정 (Y축으로 -2만큼 아래로)
        Vector3 startPosition = transformMasterObj.position;
        startPosition.y -= 2f;
        spawnedObject.transform.position = startPosition;

        // Collider 및 Rigidbody 비활성화
        if (objCollider != null) objCollider.enabled = false;
        if (objRigidbody != null) objRigidbody.isKinematic = true;

        // DOTween으로 부드럽게 올라오는 애니메이션
        spawnedObject.transform.DOMoveY(transformMasterObj.position.y, 1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                // 애니메이션 완료 후 Collider 및 Rigidbody 활성화
                if (objCollider != null) objCollider.enabled = true;
                if (objRigidbody != null) objRigidbody.isKinematic = false;
            });

        // 추가 기능들
        testFaces[0].SetActive(true);
        testFaces[1].SetActive(false);
        testFaces[2].SetActive(false);

        for (int i = 0; i < scanners.Length; i++)
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




        clockWork.canInteract = true;






        // #. 테스트용 함수
        testFaces[0].SetActive(false);
        testFaces[1].SetActive(true);
        testFaces[2].SetActive(false);

        
    }





}
