using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // 현재 진행해야 하는 방향 true = 앞 방향 / false = 뒷 방향

    public float initialSpeed;  // 처음 속도

    private Rigidbody rb;
    private Coroutine nowCoroutine;


    public override void TrunOnObj()
    {
        base.TrunOnObj();

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
        float currentSpeed = initialSpeed; // 시작 속도 유지

        while (fCurClockBattery > 0)
        {
            // 배터리 잔량이 설정된 임계값 이하로 떨어지면 감속 모드로 전환
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                // 감속 중
                currentSpeed = Mathf.Lerp(0, initialSpeed, fCurClockBattery / fLowClockBatteryPoint); // 감속 처리
            }

            float fMoveDirection = bMoveDirection ? -1f : 1f;

            // Transform을 사용해 위치 업데이트
            transform.position += transform.forward * currentSpeed * fMoveDirection * Time.deltaTime;

            fCurClockBattery -= Time.deltaTime;
            TurningClockWork();

            yield return null;
        }

        TrunOffObj();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 대상이 RoadCar 스크립트를 가지고 있는지 확인
        if (collision.gameObject.GetComponent<ParkingCar>() != null) 
        {
            TrunOffObj();
        }
    }
}
