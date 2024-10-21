using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClockObject : ClockWork
{
    public abstract void OnObject(); // 추상 메서드 선언
    public abstract void OffObject(); // 추상 메서드 선언
}
