using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // ���� �����ؾ� �ϴ� ���� true = �� ���� / false = �� ����

    public float initialSpeed;  // ó�� �ӵ�

    private Rigidbody rb;
    private Coroutine nowCoroutine;


    public override void TrunOnObj()
    {
        base.TrunOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
    }
    public override void TrunOffObj()
    {
        base.TrunOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        rb.velocity = Vector3.zero;
        bMoveDirection = !bMoveDirection;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private IEnumerator MoveForwardWithAcceleration()
    {
        float currentSpeed = initialSpeed; // ���� �ӵ� ����

        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                currentSpeed = Mathf.Lerp(0, initialSpeed, fCurClockBattery / fLowClockBatteryPoint);
            }

            float fMoveDirection = bMoveDirection ? -1f : 1f;

            // Rigidbody�� ����� ��ġ ������Ʈ
            rb.MovePosition(transform.position + transform.forward * currentSpeed * fMoveDirection * Time.deltaTime);
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }

        TrunOffObj();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ParkingCar>() != null)
        {
            TrunOffObj();
        }
    }
}
