using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // 현재 진행해야 하는 방향 true = 앞 방향 / false = 뒷 방향

    public float initialSpeed;  // 처음 속도


    public GameObject CarObj; // 최종적으로 움직임이 적용될 오브젝트
    public Rigidbody rb; // CarObj의 하위 항목에서 찾은 Rigidbody
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
        // CarObj의 하위 오브젝트에서 Rigidbody 찾기
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

            // CarObj의 위치 업데이트
            CarObj.transform.position += CarObj.transform.forward * currentSpeed * moveDirection * Time.deltaTime;

            // 배터리 소모
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }

        TrunOffObj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ParkingCar>() != null)
        {
            Debug.Log("자동차에 닿음!!!");
            TrunOffObj();
        }
        else
        {
            Debug.Log("해당 물체는 ParkingCar 스크립트를 가지고 있지 않습니다.");
        }
    }
}
