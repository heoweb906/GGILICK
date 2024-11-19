using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_2 : MonoBehaviour
{
    // #. ���� ����, ������ ����, �� ���� Transform �� ���� �ؾ� ��
    public Transform position_StationPoint_2;

    public float travelDuration;        // ����� -> ������, ������ -> ���� ������ �̵� �ð�

    public TrainDoor[] trainDoors;
    public GameObject[] crowds;

    public void StartTrain()
    {
        StartCoroutine(StartTrainJourney());
    }

    // ���� ������ �����ϴ� �ڷ�ƾ
    private IEnumerator StartTrainJourney()
    {
        GameAssistManager.Instance.player.transform.SetParent(transform);

        // ������ �� �߿��� �ϳ��� ������ ž���� �� �ֵ���
        for (int i = 0; i < trainDoors.Length; i++)
        {
            crowds[i].SetActive(true);
        }
        crowds[SubWayAssist.Instance.iCrowedRanNum].SetActive(false);

        yield return new WaitForSeconds(0.1f);


        // ������ ��� �̵��մϴ�.
        transform.DOMove(position_StationPoint_2.position, travelDuration)
                       .SetEase(Ease.OutCubic);  
        yield return new WaitForSeconds(travelDuration);


        // ���� ���ϴ�
        GameAssistManager.Instance.player.transform.SetParent(null);
        foreach (TrainDoor traindoor in trainDoors)
            traindoor.StartOpen();

    }
}
