using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ClockWorkType
{
    Floor,
    Wall
};

public class ClockWork : InteractableObject
{
    public ClockBattery clockBattery;
    [SerializeField] private ClockWorkType clockWorkType;

    private void Start()
    {
        type = InteractableType.ClockWork;
        canInteract = true;
    }

    public void ChargingBattery()
    {
        if (clockBattery.fMaxClockBattery > clockBattery.fCurClockBattery && !clockBattery.bDoing)
        {
            Debug.Log("태엽 돌리는 중");
            clockBattery.fCurClockBattery += 1;
            //transform.Rotate(Vector3.forward * 80f * Time.deltaTime);
            clockBattery.clockWork = this.gameObject;
            canInteract = false;
        }
        if (clockBattery.fMaxClockBattery <= clockBattery.fCurClockBattery) clockBattery.bBatteryFull = true;
    }

    public void EndCharging_To_BatteryStart()
    {
        if (!canInteract)
        {
            Debug.Log("태엽 -> 배터리 가동");
            clockBattery.TrunOnObj();
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

    public void ClockWorkRotate()
    {
        if(clockWorkType == ClockWorkType.Wall)
            gameObject.transform.DORotate(new Vector3(0, 0, 180), 0.3f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);
        else if(clockWorkType == ClockWorkType.Floor)
            gameObject.transform.DORotate(new Vector3(0, 0, 180), 0.8f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear);
    }

}
