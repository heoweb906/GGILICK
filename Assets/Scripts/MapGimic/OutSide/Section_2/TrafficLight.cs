using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class TrafficLight : ClockBattery
{
    [Header("���� ��ȣ�� �۵� ����")]
    [SerializeField] private bool bTrafficLightOnOff; // ���� ��ȣ���� ����
    public GameObject crossWalk_Assist; // Ⱦ�ܺ��� �۵� �ÿ� ��ֹ� ���� ���� ������Ʈ 
    
    [Header("��ȣ�� �Һ� ����")]
    public GameObject[] TrafficThreeColors;   // ���� ��ȣ��
    public GameObject[] TrafficTwoClolors;    // ���� ��ȣ��
    public TrafficLight_2 trraficLight_2;

    [Header("�߰� �¿���")]
    public GameObject testObj;
    private bool bInClockWork; // ���峭 �¿��� ������ �־�����
    private List<TrafficClockWorkAssist> trafficClockWorkAssists = new List<TrafficClockWorkAssist>();
    public TrafficClockWorkAssist plusClockWorkObj;
    

    [Space(30f)]

    // ���� �и��� �ʿ䰡 ���ٰ� �ǴܵǾ� ��ȣ�� ��ũ��Ʈ �ϳ����� �����մϴ�.
    [Header("���� ����")] 
    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform[] positions_carCreate_2;
    public Transform[] postions_end;


    private float[] positionCooldowns; // �� ��ġ�� ��ٿ� Ÿ�̸�
    public float spawnRate_1; // �ڵ����� �����Ǵ� ��� �ð�
    public float cooldownDuration_1; // ������ ��ġ�� ��� �Ұ����� �ð�
    public int iMaxCarCnt_1; // �ϳ��� ���ο��� ������ �� �ִ� �ִ� ���� ��
    private float spawnTimer_1 = 0f; // Ÿ�̸�
    public List<GameObject> spawnedCars_1 = new List<GameObject>(); // ������ �ڵ����� ���� ����Ʈ
    public List<GameObject> spawnedCars_2 = new List<GameObject>(); 


    private Coroutine nowCoroutine;


    private void Awake()
    {
        ChangeTrafficColor(2);
        positionCooldowns = new float[positions_carCreate.Length];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            InsertClockWorkPiece(testObj);
        }

        // �ڵ��� ���� �κ�
        spawnTimer_1 += Time.deltaTime;
        if (spawnedCars_1.Count < iMaxCarCnt_1 && bTrafficLightOnOff) // ������ �ڵ����� 100�� �̸��� ��
        {
            if (spawnTimer_1 >= spawnRate_1)
            {
                SpawnCars_1();
                spawnTimer_1 = 0f;
            }
        }
        for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
    }




    public override void TrunOnObj()
    {
        base.TrunOnObj();
        RotateObject((int)fCurClockBattery + 2);

        if(bInClockWork)
        {
            for (int i = 0; i < trafficClockWorkAssists?.Count; i++)
            {
                trafficClockWorkAssists[i].RotateObject((int)fCurClockBattery + 3, i % 2 == 0 ? 1f : -1f);
                trraficLight_2.SpinClockWork((int)fCurClockBattery);
            }
        }

        nowCoroutine = StartCoroutine(ChangeToYellowAndRed());
    }

    public override void TrunOffObj()
    {
        base.TrunOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);
        ChangeTrafficColor(2);
    }





    // #. �¿� ����
    private IEnumerator ChangeToYellowAndRed()
    {
        if (bInClockWork)
        {
            yield return new WaitForSeconds(1.6f);
            ChangeTrafficColor(1);
            yield return new WaitForSeconds(1.5f);
            ChangeTrafficColor(0);

            // ���͸��� �ִ� ���� fCurClockBattery ����
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= Time.deltaTime;
                yield return null;
            }

            TrunOffObj();
        }
        else
        {
            yield return new WaitForSeconds(4.1f);
            TrunOffObj();
        }

           
    }

   

    // #. ��ȣ�� �� ��ü �Լ� 
    // 0 = ������, 1 = �����, 2 = �ʷϺ�
    private void ChangeTrafficColor(int index) 
    {
        trraficLight_2.ChangeTrafficColor_(index);

        if (index < 0 || index >= TrafficThreeColors.Length) return;

        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);
        for (int i = 0; i < TrafficTwoClolors.Length; i++) TrafficTwoClolors[i].SetActive(false);

        bTrafficLightOnOff = (index == 2);
        crossWalk_Assist.SetActive(!bTrafficLightOnOff);

        // ���� ��ȣ�� ����
        TrafficThreeColors[index].SetActive(true);


        // �ε� ��ȣ�� ����
        if(index == 0) TrafficTwoClolors[1].SetActive(true);
        else TrafficTwoClolors[0].SetActive(true);
    }






    // #. �ڵ��� ���� �κ�
    private void SpawnCars_1()
    {
        int ranNum_posotion = UnityEngine.Random.Range(0, positions_carCreate.Length);
        int ranNum_car = UnityEngine.Random.Range(0, roadCars.Length);


        if (ranNum_posotion < 3)
        {
            if (positionCooldowns[ranNum_posotion] <= 0)
            {
                Quaternion rotation = Quaternion.Euler(0, 180, 0);
                GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, rotation);
                car.transform.SetParent(gameObject.transform);
                RoadCar roadCar = car.GetComponent<RoadCar>();

                roadCar.trafficLight = this;
                roadCar.bMoveActive = true;
                roadCar.bDirection = true;

                spawnedCars_1.Add(car); // ������ �ڵ����� ����Ʈ�� �߰�
                positionCooldowns[ranNum_posotion] = cooldownDuration_1;
            }
            else SpawnCars_1();
        }
        else
        {
            if (positionCooldowns[ranNum_posotion] <= 0)
            {
                GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, Quaternion.identity);
                RoadCar roadCar = car.GetComponent<RoadCar>();

                roadCar.trafficLight = this;
                roadCar.bMoveActive = true;

                spawnedCars_2.Add(car); // ������ �ڵ����� ����Ʈ�� �߰�
                positionCooldowns[ranNum_posotion] = cooldownDuration_1;
            }
            else SpawnCars_1();
        }
    }





    // #. �¿��� �ȾƼ� �־��ִ� �Լ�
    public void InsertClockWorkPiece(GameObject clockWorkObj)
    {
        TrafficClockWorkAssist assist = clockWorkObj.GetComponent<TrafficClockWorkAssist>();
        trafficClockWorkAssists.Add(assist);  
        trafficClockWorkAssists.Add(plusClockWorkObj);  // plusClockWorkObj�� �߰�


        ClockWork clockWorkMine = clockWork.GetComponent<ClockWork>(); ;

        ClockWork clockwork_1 = trafficClockWorkAssists[0].GetComponent<ClockWork>();
        ClockWork clockwork_2 = trafficClockWorkAssists[1].GetComponent<ClockWork>();

        // clockWorkMine.plusClockWorks�� List�� �����Ͽ� �߰�
        clockWorkMine.plusClockWorksList.Add(clockwork_1);
        clockWorkMine.plusClockWorksList.Add(clockwork_2);
 
        bInClockWork = true;
    }

}
