using Cinemachine;
using UnityEngine;

public class Camera_MaxMinFollow : CameraObj
{
    private Transform player;  // ������ �÷��̾��� Transform
    public Vector3 offset;     // ī�޶�� �÷��̾� ���� ������
    public Vector3 rotationOffset; // ī�޶��� ȸ�� ������
    public float smoothSpeed;  // ī�޶� �̵� �ӵ�

    public Vector3 maxPosition; // ī�޶��� �ִ� ��ġ ����
    public Vector3 minPosition; // ī�޶��� �ּ� ��ġ ����

    private void Start()
    {
        player = GameAssistManager.Instance.player.transform;
        //GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // �÷��̾��� ��ġ�� �������� ���� ��ǥ ��ġ
        Vector3 targetPosition = player.position + offset;

        // ��ǥ ��ġ�� minPosition�� maxPosition���� ����
        targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minPosition.z, maxPosition.z);

        // �ε巴�� ī�޶� ��ǥ ��ġ�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ī�޶��� ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(rotationOffset);
        transform.rotation = targetRotation;
    }
}
