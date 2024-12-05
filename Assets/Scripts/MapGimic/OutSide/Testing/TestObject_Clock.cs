using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestObject_Clock : ClockBattery
{
    Coroutine nowCoroutine;

    public override void TrunOnObj()
    {
        base.TrunOnObj();

        nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
        RotateObject((int)fCurClockBattery);
    }

    public override void TrunOffObj()
    {
        base.TrunOffObj();
    }


    private IEnumerator MoveForwardWithAcceleration()
    {
      

        while (fCurClockBattery > 0)
        {
            fCurClockBattery -= Time.deltaTime;
            yield return null;
        }

        TrunOffObj();
    }

}
