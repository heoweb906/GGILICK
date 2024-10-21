using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrafficLight : ClockObject
{
    [Header("실제 신호등 작동 관리")]
    [SerializeField] private bool bTrafficLightOnOff; // 현재 신호등의 상태
    [SerializeField] private bool bTrafficCharging; // 신호등이 지금 충전중인지 아닌지
    [SerializeField] private float fElectricPower; // 충전된 동작 에너지
    public float fMaxElectricPower; // 최대로 충전할 수 있는 충전량

    [Header("신호등 불빛 관리")]
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



    // #. 신호등 전력 공급 함수
    public void ChargeTrafficLight()
    {
        StopAllCoroutines();
        coroutineTrafficColor = null;

        bTrafficCharging = true;
        ChangeTrafficColor(2);
        if (fElectricPower <= fMaxElectricPower) fElectricPower += 5f * Time.deltaTime;
    }


    // #. 신호등 불 교체 함수
    private void ChangeTrafficColor(int index) // 0 = 빨간불, 1 = 노란불, 2 = 초록불
    {
        // 예외 처리
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
