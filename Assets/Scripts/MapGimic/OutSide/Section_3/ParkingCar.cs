using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // ���� �����ؾ� �ϴ� ���� true = �� ���� / false = �� ����

    public float initialSpeed;  // ó�� �ӵ�


    public GameObject CarObj; // ���������� �������� ����� ������Ʈ
    public Rigidbody rb; // CarObj�� ���� �׸񿡼� ã�� Rigidbody
    public bool bIsWall;


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
        // CarObj�� ���� ������Ʈ���� Rigidbody ã��
        if (CarObj != null && !bIsWall)
        {
            rb = CarObj.GetComponentInChildren<Rigidbody>();
        }

    }


    private IEnumerator MoveForwardWithAcceleration()
    {
        float currentSpeed = initialSpeed;

        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                currentSpeed = Mathf.Lerp(0, initialSpeed, fCurClockBattery / fLowClockBatteryPoint);
            }

            float moveDirection = bMoveDirection ? -1f : 1f;

            // CarObj�� ��ġ ������Ʈ
            CarObj.transform.position += CarObj.transform.forward * currentSpeed * moveDirection * Time.deltaTime;

            // ���͸� �Ҹ�
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }

        TrunOffObj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ParkingCar>() != null)
        {
            Debug.Log("�ڵ����� ����!!!");
            TrunOffObj();
        }
        else
        {
            Debug.Log("�ش� ��ü�� ParkingCar ��ũ��Ʈ�� ������ ���� �ʽ��ϴ�.");
        }
    }
}
