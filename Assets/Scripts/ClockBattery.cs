using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClockBattery : MonoBehaviour
{
    [HideInInspector] public GameObject clockWork;
    [Header("배터리")]
    public float fMaxClockBattery;
    public float fCurClockBattery;
    public float fLowClockBatteryPoint;
    public bool bDoing;
    public bool bBatteryFull;


    public virtual void TrunOnObj()
    {
        Debug.Log("배터리 실행!!!");
        bDoing = true;
        clockWork.GetComponent<ClockWork>().canInteract = false; // 보험 (사실 필요없음 )
    }


    public virtual void TrunOffObj()
    {
        Debug.Log("배터리 중지@@@");
        bDoing = false;
        clockWork.GetComponent<ClockWork>().canInteract = true;
        bBatteryFull = false;
    }



    // #. 태엽 기본 회전 함수 (처음에는 빠르게 회전, 서서히 천천히 회전)
    protected void TurningClockWork()
    {
        Debug.Log("배터리 작동 중!!!!!!!!!");

        float batteryRatio = fCurClockBattery / fMaxClockBattery;

        // 회전 속도는 최소 10f에서 최대 100f까지 변화
        float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // 최소 회전 속도 10f, 최대 속도 200f

        clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }


    // #. 태엽의 일반 회전
    protected void TruningClockWork_Simple(float fRoateSpeed)
    {
        clockWork.transform.Rotate(Vector3.forward * fRoateSpeed * Time.deltaTime);
    }

    // #. 태엽을 흔듬
    protected void TruningClockWork_Shake(float fDuration, float dShakeStength = 20f)
    {
        clockWork.transform.DOPunchRotation(new Vector3(0, 0, 5), fDuration, 20, 1);
    }





}
