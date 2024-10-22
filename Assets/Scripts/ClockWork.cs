using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockWork : MonoBehaviour
{
    public ClockPower clockPower;

    public void OnClockWork()
    {
        clockPower.OnClockPower();
    }

    public void OffClockWork()
    {
        clockPower.OffClockPower();
    }
}
