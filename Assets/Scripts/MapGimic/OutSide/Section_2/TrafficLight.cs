using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TrafficLight : ClockBattery
{
    [Header("실제 신호등 작동 관리")]
    [SerializeField] private bool bTrafficLightOnOff; // 현재 신호등의 상태
    public GameObject crossWalk_Assist; // 횡단보도 작동 시에 장애물 역할 해줄 오브젝트 

    [Header("신호등 불빛 관리")]
    public GameObject[] TrafficThreeColors;

    [Space(30f)]
    // 도로의 차들을 생성하고 관리하는 부분입니다. 

    // 따로 분리할 필요가 없다고 판단되어 신호등 스크립트 하나에서 관리합니다.
    [Header("차량 관리")] 
    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform postion_end;
    public float spawnRate; // 자동차가 생성되는 평균 시간
    public float cooldownDuration; // 생성된 위치가 사용 불가능한 시간
    public int iMaxCarCnt; // 하나의 도로에서 생성될 수 있는 최대 차량 수
    private float spawnTimer = 0f; // 타이머
    private float[] positionCooldowns; // 각 위치의 쿨다운 타이머
    public List<GameObject> spawnedCars = new List<GameObject>(); // 생성된 자동차를 담을 리스트

    private Coroutine nowCoroutine;

    private void Awake()
    {
        ChangeTrafficColor(2);
        positionCooldowns = new float[positions_carCreate.Length];
    }

    private void Update()
    {
        if (bDoing) TurningClockWork();

        // 자동차 관련 부분
        spawnTimer += Time.deltaTime;
        if (spawnedCars.Count < iMaxCarCnt && bTrafficLightOnOff) // 생성된 자동차가 100대 미만일 때
        {
            // 평균적으로 3초에 2대 생성 (1.5초마다 1대)
            if (spawnTimer >= spawnRate)
            {
                SpawnCars();
                spawnTimer = 0f; // 타이머 초기화
            }
        }
        for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
    }


    public override void TrunOnObj()
    {
        base.TrunOnObj();

        nowCoroutine = StartCoroutine(ChangeToYellowAndRed());
    }


    public override void TrunOffObj()
    {
        base.TrunOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        ChangeTrafficColor(2);
    }



    // #. 태엽 동작
    private IEnumerator ChangeToYellowAndRed()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeTrafficColor(1);
        yield return new WaitForSeconds(1.1f);
        ChangeTrafficColor(0);

        // 배터리가 있는 동안 fCurClockBattery 감소
        while (fCurClockBattery > 0)
        {
            fCurClockBattery -= Time.deltaTime;
            yield return null;
        }

        TrunOffObj();
    }

   





    // #. 신호등 불 교체 함수 
    // 0 = 빨간불, 1 = 노란불, 2 = 초록불
    private void ChangeTrafficColor(int index) 
    {
        if (index < 0 || index >= TrafficThreeColors.Length) return;
        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);

        if (index == 2)
        {
            bTrafficLightOnOff = true;
            crossWalk_Assist.SetActive(false);
        }
        else
        {
            bTrafficLightOnOff = false;
            crossWalk_Assist.SetActive(true);
        }
   
        TrafficThreeColors[index].SetActive(true);
    }


    





    // #. 자동차 관련 부분
    private void SpawnCars()
    {
        int ranNum_posotion = Random.Range(0, positions_carCreate.Length);
        int ranNum_car = Random.Range(0, roadCars.Length);

        if (positionCooldowns[ranNum_posotion] <= 0)
        {
            GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, Quaternion.identity);
            RoadCar roadCar = car.GetComponent<RoadCar>();

            roadCar.trafficLight = this;
            roadCar.bMoveActive = true;

            spawnedCars.Add(car); // 생성된 자동차를 리스트에 추가
            positionCooldowns[ranNum_posotion] = cooldownDuration; 
        }
        else SpawnCars();
    }



}
