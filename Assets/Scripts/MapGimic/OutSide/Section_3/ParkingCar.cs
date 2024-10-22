using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockPower
{
    public bool bMoveDirection; // 현재 진행해야 하는 방향 true = 앞 방향 / false = 뒷 방향
    public bool bIsMoving; // 현재 움직이고 있는지 아닌지
    public bool bClockWorking; // 플레이어가 태엽을 잡았는지 아닌지

    public float initialSpeed = 2f;  // 처음 속도
    public float accelerationRate = 1f;  // 가속 비율
    public float maxSpeed = 10f;  // 최고 속력
    public float moveDuration = 5f;  // 가속에 걸리는 시간

    private Rigidbody rb;
    private Coroutine nowCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void StartMoving()
    {
        if (!bIsMoving) // 이미 이동 중이 아니면 실행
        {
            bIsMoving = true;
            nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
        }
    }

    private IEnumerator MoveForwardWithAcceleration()
    {
        float elapsedTime = 0f;
        float currentSpeed = initialSpeed;

        // 일정 시간 동안 가속
        while (elapsedTime < moveDuration)
        {
            currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, elapsedTime / moveDuration);

            float fMoveDirection = bMoveDirection ? -1f : 1f;

            rb.velocity = transform.forward * currentSpeed * fMoveDirection;

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 가속이 완료된 후, 최고 속도로 계속 이동
        rb.velocity = transform.forward * maxSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 대상이 RoadCar 스크립트를 가지고 있는지 확인
        if (collision.gameObject.GetComponent<ParkingCar>() != null && bIsMoving)
        {
            // 충돌한 대상이 RoadCar일 경우 이동 중단 및 상태 변경
            Debug.Log("이동 중지");

            rb.velocity = Vector3.zero;
            bIsMoving = false;
            bMoveDirection = !bMoveDirection; // 현재 방향을 반대로 바꿈

            if(nowCoroutine != null) StopCoroutine(nowCoroutine);
        }
    }




    public override void OnClockPower() 
    {
        if(!bIsMoving) bClockWorking = true;
    }



    public override void OffClockPower() 
    {
        if(bClockWorking)
        {
            bClockWorking = false;
            StartMoving();
        }
    }


}
