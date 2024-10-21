using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoor : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Transform position_target_LeftDoor;
    public Transform position_target_RightDoor;

    public float doorMoveDuration;            // 문이 열리는/닫히는 데 걸리는 시간

    private Vector3 originalPosition_LeftDoor;     // 왼쪽 문의 원래 위치
    private Vector3 originalPosition_RightDoor;    // 오른쪽 문의 원래 위치


    void Start()
    {
        // 문이 처음 시작하는 위치를 저장
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;
    }


    public void StartOpen_Close(float doorTime)
    {
        StartCoroutine(OpenAndCloseDoors(doorTime));
    }
    // 문을 열고 닫는 동작을 수행하는 코루틴
    private IEnumerator OpenAndCloseDoors(float doorStayOpenDuration)
    {
        // 1. 문을 목표 위치로 이동 (문 열기)
        leftDoor.transform.DOMove(position_target_LeftDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(position_target_RightDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);

        // 2. 정거장 체류 시간보다 조금 적게 문이 열려 있음
        yield return new WaitForSeconds(doorStayOpenDuration - doorMoveDuration);

        // 3. 문을 원래 위치로 이동 (문 닫기)
        leftDoor.transform.DOMove(originalPosition_LeftDoor, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(originalPosition_RightDoor, doorMoveDuration).SetEase(Ease.InOutCubic);
    }

}
