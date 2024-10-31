using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClockWorkType
{ 
    Floor,
    Wall
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
            Debug.Log("태엽 돌리는 중");
            clockBattery.fCurClockBattery += 2f * Time.deltaTime;
            transform.Rotate(Vector3.forward * 80f * Time.deltaTime);
            clockBattery.clockWork = this.gameObject;
            bIsActive = true;
        }
        if (clockBattery.fMaxClockBattery <= clockBattery.fCurClockBattery) clockBattery.bBatteryFull = true;
    }

    public void EndCharging_To_BatteryStart()
    {
        if(bIsActive)
        {
            Debug.Log("태엽 -> 배터리 가동");
            clockBattery.TrunOnObj();
            bIsActive = false;
        }
    }


    public bool BoolBatteryFullCharging()
    {
        return clockBattery.bBatteryFull;
    }

    public ClockWorkType GetClockWorkType()
    {
        return clockWorkType;
    }


}
