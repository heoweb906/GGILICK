using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarderringAssist : MonoBehaviour
{
    public GameObject NPC_Wandering;
    public float spawnInterval = 0.3f;

    public Transform[] position_1;
    public Transform[] position_2;
    public Transform[] position_3;
    public Transform[] position_4;

    private int iRandNum;

    void Start()
    {
        StartCoroutine(SpawnerNPCs_1());
        StartCoroutine(SpawnerNPCs_2());
    }

    IEnumerator SpawnerNPCs_1()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, position_1.Length);
            Transform spawnPosition = position_1[randomIndex];

            GameObject npc = Instantiate(NPC_Wandering, spawnPosition.position, Quaternion.identity);
            NPC_Crowd nPC_Wanderring = npc.GetComponent<NPC_Crowd>();

            // 리스트를 사용하여 목표 지점들을 추가
            List<Transform> tempCheckpoints = new List<Transform>();

            iRandNum = Random.Range(0, position_2.Length);
            tempCheckpoints.Add(position_2[iRandNum]);

            iRandNum = Random.Range(0, position_3.Length);
            tempCheckpoints.Add(position_3[iRandNum]);

            iRandNum = Random.Range(0, position_4.Length);
            tempCheckpoints.Add(position_4[iRandNum]);

            // 리스트를 배열로 변환하여 checkPoints에 할당
            nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnerNPCs_2()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, position_4.Length);
            Transform spawnPosition = position_4[randomIndex];

            GameObject npc = Instantiate(NPC_Wandering, spawnPosition.position, Quaternion.identity);
            NPC_Crowd nPC_Wanderring = npc.GetComponent<NPC_Crowd>();

            // 리스트를 사용하여 목표 지점들을 추가
            List<Transform> tempCheckpoints = new List<Transform>();

            iRandNum = Random.Range(0, position_3.Length);
            tempCheckpoints.Add(position_3[iRandNum]);

            iRandNum = Random.Range(0, position_2.Length);
            tempCheckpoints.Add(position_2[iRandNum]);

            iRandNum = Random.Range(0, position_1.Length);
            tempCheckpoints.Add(position_1[iRandNum]);

            // 리스트를 배열로 변환하여 checkPoints에 할당
            nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();

            yield return new WaitForSeconds(spawnInterval);
        }
    }





}
