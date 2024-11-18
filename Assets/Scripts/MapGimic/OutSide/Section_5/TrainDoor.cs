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

    public float doorMoveDuration;            // ���� ������/������ �� �ɸ��� �ð�

    private Vector3 originalPosition_LeftDoor;     // ���� ���� ���� ��ġ
    private Vector3 originalPosition_RightDoor;    // ������ ���� ���� ��ġ

    private bool bInPlayer;
    


    public void StartOpen_Close(float doorTime)
    {
        StartCoroutine(OpenAndCloseDoors(doorTime));
    }
    // ���� ���� �ݴ� ������ �����ϴ� �ڷ�ƾ
    private IEnumerator OpenAndCloseDoors(float doorStayOpenDuration)
    {
        // 1. ���� ��ǥ ��ġ�� �̵� (�� ����)
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;


        leftDoor.transform.DOMove(position_target_LeftDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(position_target_RightDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);

        // 2. ������ ü�� �ð����� ���� ���� ���� ���� ����
        yield return new WaitForSeconds(doorStayOpenDuration - doorMoveDuration);

        // 3. ���� ���� ��ġ�� �̵� (�� �ݱ�)
        if(bInPlayer)
        {
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
        }
          


        leftDoor.transform.DOMove(originalPosition_LeftDoor, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(originalPosition_RightDoor, doorMoveDuration).SetEase(Ease.InOutCubic);


    }

    public void StartOpen()
    {
        // 1. ���� ��ǥ ��ġ�� �̵� (�� ����)
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;

        if (bInPlayer)
        {
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
            // ���߿� �÷��̾� ���� ���� �߰�
        }

        leftDoor.transform.DOMove(position_target_LeftDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
        rightDoor.transform.DOMove(position_target_RightDoor.position, doorMoveDuration).SetEase(Ease.InOutCubic);
    }





    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            bInPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            bInPlayer = false;
        }
    }

}
