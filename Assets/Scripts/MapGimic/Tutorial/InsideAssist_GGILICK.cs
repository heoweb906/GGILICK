using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideAssist_GGILICK : MonoBehaviour
{
    public static InsideAssist_GGILICK Instance;

    [Header("���� ����")]
    public bool bCarCreating; // ���� ���� ��

    public GameObject[] roadCars;
    public Transform[] positions_carCreate;
    public Transform postion_end;
    public float spawnRate; // �ڵ����� �����Ǵ� ��� �ð�
    public float cooldownDuration; // ������ ��ġ�� ��� �Ұ����� �ð�
    private float spawnTimer = 0f; // Ÿ�̸�
    private float[] positionCooldowns; // �� ��ġ�� ��ٿ� Ÿ�̸�



    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        positionCooldowns = new float[positions_carCreate.Length];
    }


    private void Update()
    {
        // �ڵ��� ���� �κ�
        if (bCarCreating)
        {
            spawnTimer += Time.deltaTime;

            // ��������� 3�ʿ� 2�� ���� (1.5�ʸ��� 1��)
            if (spawnTimer >= spawnRate)
            {
                SpawnCars();
                spawnTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }

            for (int i = 0; i < positionCooldowns.Length; i++) if (positionCooldowns[i] > 0) positionCooldowns[i] -= Time.deltaTime;
        }
      
    }


    private void SpawnCars()
    {
        int ranNum_posotion = Random.Range(0, positions_carCreate.Length);
        int ranNum_car = Random.Range(0, roadCars.Length);

        if (positionCooldowns[ranNum_posotion] <= 0)
        {
            GameObject car = Instantiate(roadCars[ranNum_car], positions_carCreate[ranNum_posotion].position, Quaternion.identity);
            GGILICK_Car ggilcikCar = car.GetComponent<GGILICK_Car>();
            ggilcikCar.transform_Destroy = postion_end;

            positionCooldowns[ranNum_posotion] = cooldownDuration;
        }
        else SpawnCars();
    }


}
