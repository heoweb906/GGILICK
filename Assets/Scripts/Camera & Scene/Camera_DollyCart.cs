using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_DollyCart : CameraObj
{
    public Transform player;  // ������ �÷��̾��� Transform
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
        float t = currentPosition - currentIndex;  // �ε巯�� ������ ���� float ���

        // ȸ�� ���� (Quaternion.Slerp�� ����Ͽ� �ε巴�� ȸ��)
        Quaternion startRotation = Quaternion.Euler(dollyRotation.Offsets[currentIndex].lookAtOffset);
        Quaternion endRotation = Quaternion.Euler(dollyRotation.Offsets[nextIndex].lookAtOffset);
        Quaternion interpolatedRotation = Quaternion.Slerp(startRotation, endRotation, t);

        // ī�޶��� ȸ�� ����
        // ���� ȸ������ �� �ε巴�� ����ǵ��� ȸ������ ���ݾ� ����
        virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, interpolatedRotation, Time.deltaTime * 5f); // 5f�� �ε巯�� �ӵ��� ���� ���Դϴ�.
    }



}
