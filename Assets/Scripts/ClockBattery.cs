using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClockBattery : MonoBehaviour
{
    [HideInInspector] public GameObject clockWork;
    [Header("���͸�")]
    public float fMaxClockBattery;
    public float fCurClockBattery;
    public float fLowClockBatteryPoint;
    public bool bDoing;
    public bool bBatteryFull;


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



    // #. �¿� �⺻ ȸ�� �Լ� (ó������ ������ ȸ��, ������ õõ�� ȸ��)
    protected void TurningClockWork()
    {
        Debug.Log("���͸� �۵� ��!!!!!!!!!");

        float batteryRatio = fCurClockBattery / fMaxClockBattery;

        // ȸ�� �ӵ��� �ּ� 10f���� �ִ� 100f���� ��ȭ
        float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // �ּ� ȸ�� �ӵ� 10f, �ִ� �ӵ� 200f

        clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }


    // #. �¿��� �Ϲ� ȸ��
    protected void TruningClockWork_Simple(float fRoateSpeed)
    {
        clockWork.transform.Rotate(Vector3.forward * fRoateSpeed * Time.deltaTime);
    }

    // #. �¿��� ���
    protected void TruningClockWork_Shake(float fDuration, float dShakeStength = 20f)
    {
        clockWork.transform.DOPunchRotation(new Vector3(0, 0, 5), fDuration, 20, 1);
    }





}
