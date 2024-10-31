using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBattery : MonoBehaviour
{
    [HideInInspector] public GameObject clockWork;
    [Header("���͸�")]
    public float fMaxClockBattery;
    public float fCurClockBattery;
    public float fLowClockBatteryPoint;
    public bool bDoing;
    public bool bBatteryFull;

    private void Start()
    {
        
    }

    public virtual void TrunOnObj()
    {
        Debug.Log("���͸� ����!!!");
        bDoing = true;
        clockWork.GetComponent<ClockWork>().canInteract = false; // ���� (��� �ʿ���� )
    }


    public virtual void TrunOffObj()
    {
        Debug.Log("���͸� ����@@@");
        bDoing = false;
        clockWork.GetComponent<ClockWork>().canInteract = true;
        bBatteryFull = false;
    }


    protected void TurningClockWork()
    {
        Debug.Log("���͸� �۵� ��!!!!!!!!!");

        float batteryRatio = fCurClockBattery / fMaxClockBattery;

        // ȸ�� �ӵ��� �ּ� 10f���� �ִ� 200f���� ��ȭ
        float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // �ּ� ȸ�� �ӵ� 10f, �ִ� �ӵ� 200f

        clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
