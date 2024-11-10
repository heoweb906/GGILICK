using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_DollyCart : CameraObj
{
    private Transform player;  // 추적할 플레이어의 Transform
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineDollyCart dollyCart;
    public DollyRotation dollyRotation;
    public GameObject FollowCart;



    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        dollyCart = FollowCart.GetComponent<CinemachineDollyCart>();

        virtualCamera.Follow = FollowCart.transform;
    }


    public void Update()
    {
        RoateCameraOnDollyTrack();
    }



   private void RoateCameraOnDollyTrack()
    {
        int currentIndex = Mathf.FloorToInt(dollyCart.m_Position);
        int nextIndex = currentIndex + 1;

        // 인덱스가 배열 범위를 벗어나지 않도록 체크
        if (currentIndex >= dollyRotation.rotation_OffSet.Length - 1) return;
        if (nextIndex >= dollyRotation.rotation_OffSet.Length) return;

        // 현재 위치에서의 보간 비율 계산
        float t = dollyCart.m_Position - currentIndex;

        // 회전 보간
        Vector3 interpolatedRotation = Vector3.Lerp(dollyRotation.rotation_OffSet[currentIndex], dollyRotation.rotation_OffSet[nextIndex], t);

        // 오브젝트 회전 적용
        transform.eulerAngles = interpolatedRotation;
    }


}
