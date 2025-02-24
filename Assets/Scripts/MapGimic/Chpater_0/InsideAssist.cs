using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsideAssist : MonoBehaviour
{
    public static InsideAssist Instance { get; private set; }

    [Header("자동타 생성 관련")]
    public bool bCanCarCreate;
    public CinemachineSmoothPath[] path;
    public GameObject[] Cars;
    private float fTimer;

    public float fCreateInterval = 3f; // 3초마다 생성

    private void Start()
    {
        Instance = this;


        if(SaveData_Manager.Instance.GetBoolInside())
        {
            StartCoroutine(StartInsideAgain());
        }

     
    }

    private void Update()
    {



        if (bCanCarCreate) CalculateCarTime();

    }


    // #. 내면 세계에서 죽었을 때 호출할 함수
    //    플레이어 스폰 위치, 카메라 위치 등을 수정
    private IEnumerator StartInsideAgain()
    {
        Debug.Log("실행됨");

        GameAssistManager.Instance.AnimateFogDensity(0f, 2f);
        GameAssistManager.Instance.AnimateAmbientIntensity(0.8f, 2f);

        yield return new WaitForSecondsRealtime(6f);

        bCanCarCreate = true;
    }




    // #. 차량 생성 함수
    private void CalculateCarTime()
    {
        if (path == null || path.Length == 0 || Cars.Length == 0) return;

        fTimer += Time.deltaTime;

        if (fTimer >= fCreateInterval)
        {
            CreateCarAtPath();
            fTimer = 0f;
        }
    }
    private void CreateCarAtPath()
    {
        // 경로에서 랜덤한 인덱스 선택
        int iRandomNum = Random.Range(0, path.Length);
        Vector3 spawnPosition = path[iRandomNum].EvaluatePosition(0f);

        // 랜덤한 차량을 확률에 따라 선택
        int rand = Random.Range(0, 100); // 0 ~ 99 사이 난수 생성
        GameObject selectedCar;

        if (rand < 60) selectedCar = Cars[0];  // 60% 확률
        else if (rand < 85) selectedCar = Cars[1]; // 25% 확률
        else selectedCar = Cars[2]; // 15% 확률

        // 선택한 차량 인스턴스화 및 설정
        RoadCarOnTrack roadCar = Instantiate(selectedCar, spawnPosition, Quaternion.identity).GetComponent<RoadCarOnTrack>();
        roadCar.transform.SetParent(transform);
        roadCar.m_Path = path[iRandomNum];
    }





}
