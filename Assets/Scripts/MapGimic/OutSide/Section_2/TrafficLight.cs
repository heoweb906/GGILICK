using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrafficLight : ClockPower
{
    [Header("���� ��ȣ�� �۵� ����")]
    [SerializeField] private bool bTrafficLightOnOff; // ���� ��ȣ���� ����
    [SerializeField] private bool bTrafficCharging; // ��ȣ���� ���� ���������� �ƴ���
    [SerializeField] private float fElectricPower; // ������ ���� ������
    public float fMaxElectricPower; // �ִ�� ������ �� �ִ� ������
    public GameObject crossWalk_Assist; // Ⱦ�ܺ��� �۵� �ÿ� ��ֹ� ���� ���� ������Ʈ 

    [Header("��ȣ�� �Һ� ����")]
    public GameObject[] TrafficThreeColors;
    private Coroutine coroutineTrafficColor;


    [Space(30f)]
    // ������ ������ �����ϰ� �����ϴ� �κ��Դϴ�. 
    // ���� �и��� �ʿ䰡 ���ٰ� �ǴܵǾ� ��ȣ�� ��ũ��Ʈ �ϳ����� �����մϴ�.
    [Header("���� ����")] 
    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform postion_end;
    public float spawnRate; // �ڵ����� �����Ǵ� ��� �ð�
    public float cooldownDuration; // ������ ��ġ�� ��� �Ұ����� �ð�
    public int iMaxCarCnt; // �ϳ��� ���ο��� ������ �� �ִ� �ִ� ���� ��
    private float spawnTimer = 0f; // Ÿ�̸�
    private float[] positionCooldowns; // �� ��ġ�� ��ٿ� Ÿ�̸�
    public List<GameObject> spawnedCars = new List<GameObject>(); // ������ �ڵ����� ���� ����Ʈ


    private void Awake()
    {
        fElectricPower = 0;
        ChangeTrafficColor(2);
        positionCooldowns = new float[positions_carCreate.Length];
    }

    private void Update()
    {
        if(bTrafficCharging) ChargeTrafficLight();
        else
        {
            if (1 <= fElectricPower)
            {
                fElectricPower -= 2f * Time.deltaTime;
                if (coroutineTrafficColor == null) coroutineTrafficColor = StartCoroutine(ChangeToYellowAndRed());
            }
            else ChangeTrafficColor(2);
        }



        // �ڵ��� ���� �κ�
        spawnTimer += Time.deltaTime;
        if (spawnedCars.Count < iMaxCarCnt && bTrafficLightOnOff) // ������ �ڵ����� 100�� �̸��� ��
        {
            // ��������� 3�ʿ� 2�� ���� (1.5�ʸ��� 1��)
            if (spawnTimer >= spawnRate)
            {
                SpawnCars();
                spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
        for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
    }

    // #. ClockPower ���� �׸�
    public override void OnClockPower() { bTrafficCharging = true; }
    public override void OffClockPower() { bTrafficCharging = false; }


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
    // 0 = ������, 1 = �����, 2 = �ʷϺ�
    private void ChangeTrafficColor(int index) 
    {
        if (index < 0 || index >= TrafficThreeColors.Length) return;
        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);

        if (index == 2)
        {
            bTrafficLightOnOff = true;
            crossWalk_Assist.SetActive(false);
            coroutineTrafficColor = null;
        }
        else
        {
            bTrafficLightOnOff = false;
            crossWalk_Assist.SetActive(true);
        }
           

        TrafficThreeColors[index].SetActive(true);
    }
    private IEnumerator ChangeToYellowAndRed()
    {
        yield return new WaitForSeconds(0.7f);
        ChangeTrafficColor(1);
        yield return new WaitForSeconds(1.1f);
        ChangeTrafficColor(0);
    }




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

            spawnedCars.Add(car); // ������ �ڵ����� ����Ʈ�� �߰�
            positionCooldowns[ranNum_posotion] = cooldownDuration; 
        }
        else SpawnCars();
    }



}
