using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_DollyCart : CameraObj
{
    private Transform player;  // ������ �÷��̾��� Transform
    private CinemachineVirtualCamera virtualCamera;
    public GameObject FollowCart;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        virtualCamera.Follow = FollowCart.transform;
    }
}
