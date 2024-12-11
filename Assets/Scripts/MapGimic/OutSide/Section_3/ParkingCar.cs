using System.Collections; 
using System.Collections.Generic; 
using UnityEditor; 
using UnityEngine; 

public class ParkingCar : ClockBattery
{
    public bool bMoveDirection; // ���� �����ؾ� �ϴ� ���� true = �� ���� / false = �� ����
    private bool bIsMove;

    public float initialSpeed;  // ó�� �ӵ�

    public GameObject CarObj; // ���������� �������� ����� ������Ʈ
    public Rigidbody rb; // CarObj�� ���� �׸񿡼� ã�� Rigidbody
    public bool bIsWall;


    private Coroutine nowCoroutine;


    public override void TrunOnObj()
    {
        base.TrunOnObj();

        bIsMove = true;

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(MoveForwardWithAcceleration());
    }
    public override void TrunOffObj()
    {
        base.TrunOffObj();

        Debug.Log("����");

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        if(rb != null) rb.velocity = Vector3.zero;

        if(bIsMove)
        {
            bMoveDirection = !bMoveDirection;
            bIsMove = false;
        }
       
    }



    private void Awake()
    {
        // CarObj�� ���� ������Ʈ���� Rigidbody ã��
        if (CarObj != null && !bIsWall)
        {
            rb = CarObj.GetComponentInChildren<Rigidbody>();
        }

        bIsMove = false;
    }


    private IEnumerator MoveForwardWithAcceleration()
    {
        float currentSpeed = initialSpeed;

        Color rayColor = Color.red;

        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery < fLowClockBatteryPoint)
            {
                currentSpeed = Mathf.Lerp(0, initialSpeed, fCurClockBattery / fLowClockBatteryPoint);
            }
            float moveDirection = bMoveDirection ? -1f : 1f;

            CarObj.transform.position += CarObj.transform.forward * currentSpeed * moveDirection * Time.deltaTime;
            fCurClockBattery -= Time.deltaTime;

            yield return null;
        }


        TrunOffObj();
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ParkingCar>() != null && bIsMove)
        {
            Debug.Log("�浹!!!");
            TrunOffObj();
        }
    }





}
