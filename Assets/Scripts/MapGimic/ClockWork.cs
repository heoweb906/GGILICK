using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockWork : MonoBehaviour
{
    public ClockObject clockobj;

    public void OnClockWork()
    {
        clockobj.OnObject();
    }

    public void OffClockWork()
    {
        clockobj.OffObject();
    }
}
