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
        bDoing = true;
        clockWork.GetComponent<ClockWork>().canInteract = false; // ���� (��� �ʿ���� )
    }


    public virtual void TrunOffObj()
    {
        bDoing = false;
        clockWork.GetComponent<ClockWork>().canInteract = true;
        bBatteryFull = false;
    }



    protected void RotateObject(int time)
    {
        float rotationAmount = time * -180f;

        clockWork.transform.DORotate(new Vector3(0, 0, rotationAmount), time * 2f, RotateMode.LocalAxisAdd)
                 .SetEase(Ease.OutQuad);
    }



    // #. �¿� �⺻ ȸ�� �Լ� (ó������ ������ ȸ��, ������ õõ�� ȸ��)
    //protected void TurningClockWork()
    //{
    //    Debug.Log("���͸� �۵� ��!!!!!!!!!");

    //    float batteryRatio = fCurClockBattery / fMaxClockBattery;

    //    float rotationSpeed = Mathf.Lerp(-100f, -20f, 1f - batteryRatio); // �ּ� ȸ�� �ӵ� 10f, �ִ� �ӵ� 200f

    //    clockWork.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    //}

    // #. �¿��� �Ϲ� ȸ��
    protected void TruningClockWork_Simple(float fRoateSpeed)
    {
        clockWork.transform.Rotate(Vector3.forward * fRoateSpeed * Time.deltaTime);
    }









    // #. �¿��� ��� (���۵� ����)
    protected void TruningClockWork_Shake(float fDuration, float dShakeStength = 20f)
    {
        clockWork.transform.DOPunchRotation(new Vector3(0, 0, 5), fDuration, 20, 1);
    }



}
