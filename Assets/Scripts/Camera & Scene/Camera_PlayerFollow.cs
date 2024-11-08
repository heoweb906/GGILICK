using Cinemachine;
using UnityEngine;

public class Camera_PlayerFollow : CameraObj
{
    private Transform player;  // ������ �÷��̾��� Transform
    public Vector3 offset;    // ī�޶�� �÷��̾� ���� ������
    public Vector3 rotationOffset; // ī�޶��� ȸ�� ������
    public float smoothSpeed;  // ī�޶� �̵� �ӵ�
    

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

    }

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
