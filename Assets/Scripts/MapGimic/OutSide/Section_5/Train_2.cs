using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_2 : MonoBehaviour
{
    // #. 시작 지점, 정거장 지점, 끝 지점 Transform 다 설정 해야 함
    public Transform position_StartPoint_2;
    public Transform position_StationPoint_2;

    public float travelDuration;        // 출발점 -> 정거장, 정거장 -> 최종 목적지 이동 시간

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;

    public void StartTrain()
    {
        transform.position = position_StartPoint_2.position;  // 기차를 StartPoint 위치로 이동시킴
        StartCoroutine(StartTrainJourney());
    }

    // 기차 여정을 시작하는 코루틴
    private IEnumerator StartTrainJourney()
    {
        // 열리는 문 중에서 하나의 문에만 탑승할 수 있도록
        for (int i = 0; i < trainDoors.Length; i++)
        {
            crowds[i].SetActive(true);
        }
        crowds[SubWayAssist.Instance.iCrowedRanNum].SetActive(false);


        transform.position = position_StartPoint_2.position;
        yield return new WaitForSeconds(0.1f);


        // 기차가 계속 이동합니다.
        transform.DOMove(position_StationPoint_2.position, travelDuration)
                       .SetEase(Ease.OutCubic);  
        yield return new WaitForSeconds(travelDuration);


        // 문을 엽니다
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen();



    }
}
