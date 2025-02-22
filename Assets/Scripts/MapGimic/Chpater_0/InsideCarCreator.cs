using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideCarCreator : MonoBehaviour
{
    public static InsideCarCreator Instance;

    public bool bCanCarCreate;

    public CinemachineSmoothPath[] path;
    private float fTimer;
    public GameObject[] Cars;

    public float fCreateInterval = 3f; // 3초마다 생성

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!bCanCarCreate || path == null || path.Length == 0 || Cars.Length == 0) return;

        fTimer += Time.deltaTime;

        if (fTimer >= fCreateInterval)
        {
            CreateCarAtPath();
            fTimer = 0f;
        }
    }

    private void CreateCarAtPath()
    {
        int iRandomNum = Random.Range(0, path.Length);
        Vector3 spawnPosition = path[iRandomNum].EvaluatePosition(0f);
        GameObject selectedCar = GetRandomCarByWeight();

        RoadCarOnTrack roadCar = Instantiate(selectedCar, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();

        roadCar.transform.SetParent(transform);
        roadCar.m_Path = path[iRandomNum];
    }

    private GameObject GetRandomCarByWeight()
    {
        int rand = Random.Range(0, 100); // 0 ~ 99 사이 난수 생성

        if (rand < 60) return Cars[0];  // 60% 확률
        else if (rand < 85) return Cars[1]; // 25% 확률
        else return Cars[2]; // 15% 확률
    }





}
