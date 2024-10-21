using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    // #. 시작 지점, 정거장 지점, 끝 지점 Transform 다 설정 해야 함
    public Transform position_StartPoint;
    public Transform position_StationPoint;
    public Transform position_EndPoint;

    public float travelDuration;        // 출발점 -> 정거장, 정거장 -> 최종 목적지 이동 시간
    public float stopDuration;          // 정거장에서 멈추는 시간

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;




    public void StartTrain()
    {
        transform.position = position_StartPoint.position;  // 기차를 StartPoint 위치로 이동시킴
        StartCoroutine(StartTrainJourney());
    }

    // 기차 여정을 시작하는 코루틴
    private IEnumerator StartTrainJourney()
    {
        // 열리는 문 중에서 하나의 문에만 탑승할 수 있도록
        int ranNum = Random.Range(0, trainDoors.Length);
        for(int i = 0; i < trainDoors.Length; i++)
        {
            crowds[i].SetActive(true);
        }
        crowds[ranNum].SetActive(false);


        // 1. StartPoint에서 StationPoint로 이동 (서서히 멈추는 효과)
        transform.DOMove(position_StationPoint.position, travelDuration)
            .SetEase(Ease.OutCubic);  // 이동이 끝나갈 때 점점 느려짐

        yield return new WaitForSeconds(travelDuration);

        // 기차 문 열기
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen_Close(stopDuration);

        // 2. 정거장에서 일정 시간 멈춤
        yield return new WaitForSeconds(stopDuration);

        // 3. StationPoint에서 EndPoint로 이동 (서서히 가속하는 효과)
        transform.DOMove(position_EndPoint.position, travelDuration)
            .SetEase(Ease.InCubic);   // 출발 시 서서히 가속

        yield return new WaitForSeconds(travelDuration);

        Debug.Log("기차가 최종 목적지에 도착했습니다!");

        // 4. 이동이 끝나면 기차를 다시 시작 지점으로 순간 이동
        transform.position = position_StartPoint.position;

        // 5. 다시 기차 여정을 반복
        StartCoroutine(StartTrainJourney());
    }

}
