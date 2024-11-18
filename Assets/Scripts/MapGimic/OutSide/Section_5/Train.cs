using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    // #. ���� ����, ������ ����, �� ���� Transform �� ���� �ؾ� ��
    public Transform position_StartPoint;
    public Transform position_StationPoint;
    public Transform position_EndPoint;


    public float travelDuration;        // ����� -> ������, ������ -> ���� ������ �̵� �ð�
    public float stopDuration;          // �����忡�� ���ߴ� �ð�

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;


    public void StartTrain()
    {
        transform.position = position_StartPoint.position;  // ������ StartPoint ��ġ�� �̵���Ŵ
        StartCoroutine(StartTrainJourney());
    }

    // ���� ������ �����ϴ� �ڷ�ƾ
    private IEnumerator StartTrainJourney()
    {
        // ������ �� �߿��� �ϳ��� ������ ž���� �� �ֵ���
        SubWayAssist.Instance.iCrowedRanNum = Random.Range(0, trainDoors.Length);
        for(int i = 0; i < trainDoors.Length; i++) crowds[i].SetActive(true);
        crowds[SubWayAssist.Instance.iCrowedRanNum].SetActive(false);



        // 1. StartPoint���� StationPoint�� �̵� (������ ���ߴ� ȿ��)
        transform.DOMove(position_StationPoint.position, travelDuration)
            .SetEase(Ease.OutCubic);  // �̵��� ������ �� ���� ������
        yield return new WaitForSeconds(travelDuration);


        // 2. ���� ���� ���� ���� �ð� �ڿ� �ٽ� ���
        foreach (TrainDoor traindoor in trainDoors) traindoor.StartOpen_Close(stopDuration);
        yield return new WaitForSeconds(stopDuration);


        // 4. StationPoint���� EndPoint�� �̵� (������ �����ϴ� ȿ��)
        transform.DOMove(position_EndPoint.position, travelDuration)
            .SetEase(Ease.InCubic);   // ��� �� ������ ����
        yield return new WaitForSeconds(travelDuration);


        // ���� �÷��̾ ž���� ���� Ȯ�ε��� �ʾҴٸ� �ٽ� �ǵ���
        if (!SubWayAssist.Instance.bPlayerTakeTrain) StartCoroutine(StartTrainJourney());
    }



}
