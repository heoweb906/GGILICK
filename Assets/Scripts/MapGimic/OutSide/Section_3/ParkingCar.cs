using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ParkingCar : ClockPower
{
    public bool bMoveDirection; // ���� �����ؾ� �ϴ� ���� true = �� ���� / false = �� ����
    public bool bIsMoving; // ���� �����̰� �ִ��� �ƴ���
    public bool bClockWorking; // �÷��̾ �¿��� ��Ҵ��� �ƴ���

    public float initialSpeed = 2f;  // ó�� �ӵ�
    public float accelerationRate = 1f;  // ���� ����
    public float maxSpeed = 10f;  // �ְ� �ӷ�
    public float moveDuration = 5f;  // ���ӿ� �ɸ��� �ð�

    private Rigidbody rb;
    private Coroutine nowCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void StartMoving()
    {
        if (!bIsMoving) // �̹� �̵� ���� �ƴϸ� ����
        {
            bIsMoving = true;
            nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
        }
    }

    private IEnumerator MoveForwardWithAcceleration()
    {
        float elapsedTime = 0f;
        float currentSpeed = initialSpeed;

        // ���� �ð� ���� ����
        while (elapsedTime < moveDuration)
        {
            currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, elapsedTime / moveDuration);

            float fMoveDirection = bMoveDirection ? -1f : 1f;

            rb.velocity = transform.forward * currentSpeed * fMoveDirection;

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ������ �Ϸ�� ��, �ְ� �ӵ��� ��� �̵�
        rb.velocity = transform.forward * maxSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ����� RoadCar ��ũ��Ʈ�� ������ �ִ��� Ȯ��
        if (collision.gameObject.GetComponent<ParkingCar>() != null && bIsMoving)
        {
            // �浹�� ����� RoadCar�� ��� �̵� �ߴ� �� ���� ����
            Debug.Log("�̵� ����");

            rb.velocity = Vector3.zero;
            bIsMoving = false;
            bMoveDirection = !bMoveDirection; // ���� ������ �ݴ�� �ٲ�

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
