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
        int ranNum = Random.Range(0, trainDoors.Length);
        for(int i = 0; i < trainDoors.Length; i++)
        {
            crowds[i].SetActive(true);
        }
        crowds[ranNum].SetActive(false);


        // 1. StartPoint���� StationPoint�� �̵� (������ ���ߴ� ȿ��)
        transform.DOMove(position_StationPoint.position, travelDuration)
            .SetEase(Ease.OutCubic);  // �̵��� ������ �� ���� ������

        yield return new WaitForSeconds(travelDuration);

        // ���� �� ����
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen_Close(stopDuration);

        // 2. �����忡�� ���� �ð� ����
        yield return new WaitForSeconds(stopDuration);

        // 3. StationPoint���� EndPoint�� �̵� (������ �����ϴ� ȿ��)
        transform.DOMove(position_EndPoint.position, travelDuration)
            .SetEase(Ease.InCubic);   // ��� �� ������ ����

        yield return new WaitForSeconds(travelDuration);

        Debug.Log("������ ���� �������� �����߽��ϴ�!");

        // 4. �̵��� ������ ������ �ٽ� ���� �������� ���� �̵�
        transform.position = position_StartPoint.position;

        // 5. �ٽ� ���� ������ �ݺ�
        StartCoroutine(StartTrainJourney());
    }

}
