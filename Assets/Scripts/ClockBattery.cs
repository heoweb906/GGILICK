using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBattery : MonoBehaviour
{
    [HideInInspector] public GameObject clockWork;
    [Header("배터리")]
    public float fMaxClockBattery;
    public float fCurClockBattery;
    public float fLowClockBatteryPoint;
    public bool bDoing;
    public bool bBatteryFull;

    private void Start()
    {
        
    }

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


    protected void TurningClockWork()
    {
        Debug.Log("배터리 작동 중!!!!!!!!!");

        float batteryRatio = fCurClockBattery / fMaxClockBattery;

        // 회전 속도는 최소 10f에서 최대 200f까지 변화
        float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // 최소 회전 속도 10f, 최대 속도 200f

        clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
