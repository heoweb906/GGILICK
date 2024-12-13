using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class WayPoints
{
    public Transform[] points;
}

public class Create_WanderingNPC : MonoBehaviour
{
    public GameObject[] NPC_Wandering;
    public float spawnInterval = 0.3f;

    public WayPoints[] positionArray;

    private int iRandNum;

    void Start()
    {
        if(positionArray != null)  
            StartCoroutine(SpawnerNPCs_1());
    }

    IEnumerator SpawnerNPCs_1()
    {
        while (true)
        {
            Debug.Log("NPC를 생성하였습니다.");

            int randomIndex = Random.Range(0, positionArray[0].points.Length);
            Transform spawnPosition = positionArray[0].points[randomIndex];

            randomIndex = Random.Range(0, NPC_Wandering.Length);
            GameObject npc = Instantiate(NPC_Wandering[randomIndex], spawnPosition.position, Quaternion.identity);
            NPC_Simple nPC_Wanderring = npc.GetComponent<NPC_Simple>();
            nPC_Wanderring.bWalking = true;

            // 리스트를 사용하여 목표 지점들을 추가
            List<Transform> tempCheckpoints = new List<Transform>();


            for(int i = 1; i < positionArray.Length; i++)
            {
                iRandNum = Random.Range(0, positionArray[i].points.Length);
                tempCheckpoints.Add(positionArray[i].points[iRandNum]);
            }

            nPC_Wanderring.checkPoints = tempCheckpoints.ToArray();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

   


}
