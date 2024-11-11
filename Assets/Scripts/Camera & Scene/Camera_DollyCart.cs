using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_DollyCart : CameraObj
{
    private Transform player;  // 추적할 플레이어의 Transform
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

        // 인덱스가 배열 범위를 벗어나지 않도록 체크
        if (currentIndex >= dollyRotation.Offsets.Length - 1) return;
        if (nextIndex >= dollyRotation.Offsets.Length) return;

        // 현재 위치에서의 보간 비율 계산
        float t = currentPosition - currentIndex;

        // LookAt Offset 보간
        Vector3 startLookAtOffset = dollyRotation.Offsets[currentIndex].lookAtOffset;
        Vector3 endLookAtOffset = dollyRotation.Offsets[nextIndex].lookAtOffset;
        Vector3 interpolatedLookAtOffset = Vector3.Lerp(startLookAtOffset, endLookAtOffset, t);

        // 카메라의 시선 보정
        Vector3 lookAtPosition = player.position + interpolatedLookAtOffset;

        // 카메라 회전 부드럽게 보간
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - virtualCamera.transform.position);
        virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, targetRotation, Time.deltaTime * 5f); // Slerp로 부드럽게 회전

        // 카메라의 LookAt을 null로 설정하지 않고 LookAt을 따로 설정해도 되지만, 이 방법으로 회전을 부드럽게 만들 수 있습니다.
    }

}
