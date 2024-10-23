using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockWork : MonoBehaviour
{
    public ClockBattery clockBattery;
    private bool bIsActive;

    public void ChargingBattery()
    {
        if(clockBattery.fMaxClockBattery > clockBattery.fCurClockBattery && !clockBattery.bDoing)
        {
            Debug.Log("�¿� ������ ��");
            clockBattery.fCurClockBattery += 2f * Time.deltaTime;
            transform.Rotate(Vector3.up * 80f * Time.deltaTime);
            clockBattery.clockWork = this.gameObject;
            bIsActive = true;
        }  
    }

    public void EndCharging_To_BatteryStart()
    {
        if(bIsActive)
        {
            Debug.Log("�¿� -> ���͸� ����");
            clockBattery.TrunOnObj();
            bIsActive = false;
        }
    }

}
