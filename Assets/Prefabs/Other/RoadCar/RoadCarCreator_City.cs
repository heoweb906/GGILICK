using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoadCarCreator_City : MonoBehaviour
{
    public CinemachineSmoothPath path;
    public float fCreateInterval;
    private float fTimer;

    public GameObject[] Cars;
    

    


    private void Update()
    {
        fTimer += Time.deltaTime;

        if (fTimer >= fCreateInterval)
        {
            CreateCarAtPath();
            fTimer = 0f;
        }
    }

    private void CreateCarAtPath()
    {
        if (Cars.Length == 0 || path == null) return;

        Vector3 spawnPosition = path.EvaluatePosition(0f);
        GameObject randomCar = Cars[Random.Range(0, Cars.Length)];
        RoadCarOnTrack roadCar = Instantiate(randomCar, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();

        roadCar.m_Path = path;

    }
}
