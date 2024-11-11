using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_DollyCart : CameraObj
{
    private Transform player;  // ������ �÷��̾��� Transform
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineDollyCart dollyCart;
    public DollyRotationAndPositonOffset dollyRotation;
    public GameObject FollowCart;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        dollyCart = FollowCart.GetComponent<CinemachineDollyCart>();

        virtualCamera.Follow = FollowCart.transform;
    }

    public void FixedUpdate()
    {
        RoateCameraOnDollyTrack();
    }

    private void RoateCameraOnDollyTrack()
    {
        float currentPosition = dollyCart.m_Position;
        int currentIndex = Mathf.FloorToInt(currentPosition);
        int nextIndex = currentIndex + 1;

        // �ε����� �迭 ������ ����� �ʵ��� üũ
        if (currentIndex >= dollyRotation.Offsets.Length - 1) return;
        if (nextIndex >= dollyRotation.Offsets.Length) return;

        // ���� ��ġ������ ���� ���� ���
        float t = currentPosition - currentIndex;

        // LookAt Offset ����
        Vector3 startLookAtOffset = dollyRotation.Offsets[currentIndex].lookAtOffset;
        Vector3 endLookAtOffset = dollyRotation.Offsets[nextIndex].lookAtOffset;
        Vector3 interpolatedLookAtOffset = Vector3.Lerp(startLookAtOffset, endLookAtOffset, t);

        // ī�޶��� �ü� ����
        Vector3 lookAtPosition = player.position + interpolatedLookAtOffset;

        // ī�޶� ȸ�� �ε巴�� ����
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - virtualCamera.transform.position);
        virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, targetRotation, Time.deltaTime * 5f); // Slerp�� �ε巴�� ȸ��

        // ī�޶��� LookAt�� null�� �������� �ʰ� LookAt�� ���� �����ص� ������, �� ������� ȸ���� �ε巴�� ���� �� �ֽ��ϴ�.
    }

}
