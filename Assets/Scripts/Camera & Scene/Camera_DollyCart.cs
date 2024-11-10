using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_DollyCart : CameraObj
{
    private Transform player;  // ������ �÷��̾��� Transform
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

        // �ε����� �迭 ������ ����� �ʵ��� üũ
        if (currentIndex >= dollyRotation.rotation_OffSet.Length - 1) return;
        if (nextIndex >= dollyRotation.rotation_OffSet.Length) return;

        // ���� ��ġ������ ���� ���� ���
        float t = dollyCart.m_Position - currentIndex;

        // ȸ�� ����
        Vector3 interpolatedRotation = Vector3.Lerp(dollyRotation.rotation_OffSet[currentIndex], dollyRotation.rotation_OffSet[nextIndex], t);

        // ������Ʈ ȸ�� ����
        transform.eulerAngles = interpolatedRotation;
    }


}
