using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampMacine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    [Header("스탬프 정보")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // 찍을 스탬프들
    public Transform transforom_stamp;  // 스탬프 찍을 위치

    private Stack<GameObject> stampStack = new Stack<GameObject>(); // 생성된 스탬프 관리용 스택


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(HitStamp());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        Debug.Log(iStampNum);

    }


    private IEnumerator HitStamp()
    {
        // 배터리가 3보다 작다면
        if (fCurClockBattery < 3)
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
                yield return new WaitForSecondsRealtime(1.0f); // 1초 대기
            }
        }
        else // 배터리가 3 이상이라면
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
                yield return new WaitForSecondsRealtime(1.0f); // 1초 대기
            }

            if (iStampNum > 0)
            {
                // Y축 오프셋 계산: 스택에 있는 요소 수에 따라 설정
                float yOffset = stampStack.Count * 0.01f;

                // 스탬프 프리팹 생성
                GameObject newStamp = Instantiate(
                    Stamps[iStampNum - 1],
                    transforom_stamp.position + new Vector3(0, yOffset, 0),
                    Quaternion.Euler(90f, 0f, 0f) // X축으로 90도 회전
                );

                // 스탬프를 스택에 추가
                stampStack.Push(newStamp);

                // 부모 설정
                newStamp.transform.SetParent(transforom_stamp);
            }
        }

        TurnOffObj();
        yield break;
    }














    // #. IPartOwner 인터페이스
    #region

    public void InsertOwnerFunc(GameObject stampParts, int index)
    {
        StampParts stampParts_ = stampParts.GetComponent<StampParts>();
        iStampNum = stampParts_.iStampeNum;
    }

    public void RemoveOwnerFunc(int index)
    {
        iStampNum = 0;
    }



    #endregion
}
