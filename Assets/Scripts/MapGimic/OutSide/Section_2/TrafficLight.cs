using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrafficLight : ClockObject
{
    [Header("���� ��ȣ�� �۵� ����")]
    [SerializeField] private bool bTrafficLightOnOff; // ���� ��ȣ���� ����
    [SerializeField] private bool bTrafficCharging; // ��ȣ���� ���� ���������� �ƴ���
    [SerializeField] private float fElectricPower; // ������ ���� ������
    public float fMaxElectricPower; // �ִ�� ������ �� �ִ� ������

    [Header("��ȣ�� �Һ� ����")]
    public GameObject[] TrafficThreeColors;
    private Coroutine coroutineTrafficColor;


    private void Awake()
    {
        bTrafficLightOnOff = false;
        fElectricPower = 0;
        ChangeTrafficColor(2);
    }


    private void Update()
    {
        if(bTrafficCharging) ChargeTrafficLight();

        
        if(!bTrafficCharging)
        {
            if (1 <= fElectricPower)
            {
                fElectricPower -= 2f * Time.deltaTime;
                if(coroutineTrafficColor == null)
                {
                    coroutineTrafficColor = StartCoroutine(ChangeToYellowAndRed());
                }
            }
            else
            {
                ChangeTrafficColor(2);
            }
        }
      
    }

    public override void OnObject() { bTrafficCharging = true; }
    public override void OffObject() { bTrafficCharging = false; }



    // #. ��ȣ�� ���� ���� �Լ�
    public void ChargeTrafficLight()
    {
        StopAllCoroutines();
        coroutineTrafficColor = null;

        bTrafficCharging = true;
        ChangeTrafficColor(2);
        if (fElectricPower <= fMaxElectricPower) fElectricPower += 5f * Time.deltaTime;
    }


    // #. ��ȣ�� �� ��ü �Լ�
    private void ChangeTrafficColor(int index) // 0 = ������, 1 = �����, 2 = �ʷϺ�
    {
        // ���� ó��
        if (index < 0 || index >= TrafficThreeColors.Length) return;
        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);

        if (index == 2)
        {
            bTrafficLightOnOff = true;
            coroutineTrafficColor = null;
        }
        else bTrafficLightOnOff = false;

        TrafficThreeColors[index].SetActive(true);
    }
    private IEnumerator ChangeToYellowAndRed()
    {
        yield return new WaitForSeconds(0.7f);
        ChangeTrafficColor(1);
        yield return new WaitForSeconds(1.1f);
        ChangeTrafficColor(0);
    }

}
