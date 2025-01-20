using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToyTruck : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    public GameObject Trunk;            

    private Vector3 originRotate;
    private Vector3 currentRotate;
    private bool isShaking = false;         // �����Ÿ� ���� Ȯ��

    public GameObject[] Baggages;           // �������� �峭����
    public float throwForce = 10f; // ��ü�� ���� ��
    public Vector3 throwDirection; // ��ü�� ���� ����



    private void Awake()
    {
        originRotate = Trunk.transform.localRotation.eulerAngles;
        currentRotate = originRotate; // ���� �����̼��� �ʱⰪ���� ����
    }



    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ActiveTrunk());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null)
        {
            StopCoroutine(nowCoroutine);
            nowCoroutine = null;
        }

        ResetTrunkRotation(); // Ʈ��ũ�� �ʱ� ���·� ����
    }


    // #. Ʈ��ũ�� ���� ȸ�� ������ ���ƿ�
    private void ResetTrunkRotation()
    {
        if (Trunk != null)
        {
            // DOTween���� ���� ���·� �ε巴�� ����
            Trunk.transform.DOLocalRotate(originRotate, 0.5f).SetEase(Ease.OutQuad);
            currentRotate = originRotate; // ���� ȸ�� ���µ� ���� ���·� ����
        }
    }


    IEnumerator ActiveTrunk()
    {
        float zRotation = currentRotate.z; // ���� Z�� �����̼� �� (���� �����̼��� ��������)

        while (fCurClockBattery > 0)
        {
            

            if (Trunk != null)
            {
                zRotation += 2f; // ȸ�� �� ����
                currentRotate.z = zRotation; // ���� �����̼� ������Ʈ
                Trunk.transform.DOLocalRotate(currentRotate, 0.5f)
                    .SetEase(Ease.Linear);

                if (zRotation >= 6f && !isShaking)
                {
                    isShaking = true;
                    yield return StartCoroutine(ShakeTrunk());
                    isShaking = false;


                    ThrowBaggages();
                    float targetRotation = 40f;
                    currentRotate.z = targetRotation; // ���ο� ��ǥ ȸ�� ���� ����
                    Trunk.transform.DOLocalRotate(currentRotate, 1f)
                    .SetEase(Ease.OutBack);
               

                    yield return new WaitForSecondsRealtime(2.0f);
                    
                    StopCoroutine(nowCoroutine);
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(1.0f); // 1�� ���

            fCurClockBattery -= 1;
            // ���͸��� �� �Ǹ� �ʱ� ���·� ����
            if (fCurClockBattery <= 0)
            {
                fCurClockBattery = 0;
                ResetTrunkRotation();
                TurnOffObj();
                yield break;
            }

            
        }

        TurnOffObj(); // ���͸��� �� �Ǹ� ����
    }


    IEnumerator ShakeTrunk()
    {
        if (Trunk != null)
        {
            float duration = 1.2f; // ��鸲 �ð�
            float strength = 0.2f; // ��鸲 ����
            int vibrato = 10; // ���� Ƚ�� (��)

            // Ʈ��ũ�� ���� ȸ�� ���¸� �������� ����
            currentRotate = Trunk.transform.localRotation.eulerAngles;

            float elapsedTime = 0f;
            float shakeDuration = duration; // ��鸲 ���� �ð�
            float shakeStrength = strength; // ��鸲 ����

            // ��鸲 ȿ�� �ε巴�� ����
            while (elapsedTime < shakeDuration)
            {
                // �����ĸ� �̿��� �ڿ������� ��鸲
                float sineValue = Mathf.Sin(elapsedTime * vibrato * Mathf.PI * 2f);
                float randomZ = sineValue * shakeStrength; // �����Ŀ� ������ ����

                // ���� ȸ�������� Z�� ȸ���� ������Ŵ
                currentRotate.z = currentRotate.z + randomZ;

                Trunk.transform.localRotation = Quaternion.Euler(currentRotate);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ��鸲�� ���� �� ���� ȸ�� ������ ����
            Trunk.transform.localRotation = Quaternion.Euler(originRotate);
        }
    }



    private void ThrowBaggages()
    {
        foreach (GameObject baggage in Baggages)
        {
            Rigidbody rb = baggage.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float randomX = Random.Range(-3f, 3f);
                float randomZ = Random.Range(-3f, 3f);

                float randomThrowForce = Random.Range(-2f, 1f);

                Vector3 modifiedThrowDirection = throwDirection + new Vector3(randomX, 0f, randomZ);

                rb.AddForce(modifiedThrowDirection * (throwForce + randomThrowForce), ForceMode.Impulse);
            }
        }
    }

    public void InsertOwnerFunc(GameObject parts, int iIndex = 0)
    {
        //throw new System.NotImplementedException();
    }

    public void RemoveOwnerFunc(int iIndex = 0)
    {
        //throw new System.NotImplementedException();
    }
} 
