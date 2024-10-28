using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum ClockWorkType
{ 
    Floor,
    Wall_East,
    Wall_West,
    Wall_South,
    Wall_North
};



public class ClockWork : MonoBehaviour
{
    public ClockBattery clockBattery;
    [SerializeField] private ClockWorkType clockWorkType;
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
        if (clockBattery.fMaxClockBattery <= clockBattery.fCurClockBattery) clockBattery.bBatteryFull = true;
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


    public bool BoolBatteryFullCharging()
    {
        return clockBattery.bBatteryFull;
    }


}
