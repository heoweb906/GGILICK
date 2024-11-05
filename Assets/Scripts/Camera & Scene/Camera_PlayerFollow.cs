using Cinemachine;
using UnityEngine;

public class Camera_PlayerFollow : CameraObj
{
    public Transform player;  // ������ �÷��̾��� Transform
    public Vector3 offset;    // ī�޶�� �÷��̾� ���� ������
    public float smoothSpeed = 0.125f;  // ī�޶� �̵� �ӵ�
    public Vector3 rotationOffset; // ī�޶��� ȸ�� ������

    void FixedUpdate()
    {
        // �÷��̾��� ��ġ�� �������� ���� ��ǥ ��ġ
        Vector3 targetPosition = player.position + offset;

        // �ε巴�� ī�޶� ��ǥ ��ġ�� �̵�
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ī�޶��� ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(rotationOffset);
        transform.rotation = targetRotation;
    }
}
