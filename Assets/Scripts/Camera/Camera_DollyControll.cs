using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_DollyControll : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public CinemachineDollyCart dollyCart; // Cinemachine Dolly Cart
    public CinemachineSmoothPath dollyPath; // Cinemachine Path

    // 카트가 플레이어 위치에 맞춰 이동하는 속도 조정
    public float followSpeed;

    void FixedUpdate()
    {
        if (player != null && dollyPath != null)
        {
            float closestPoint = dollyPath.FindClosestPoint(player.position, 0, -1, 10);
            float targetPosition = Mathf.Clamp(closestPoint, 0, dollyPath.PathLength);

            // 현재 위치가 목표 위치와 다를 경우에만 이동
            if (Mathf.Abs(dollyCart.m_Position - targetPosition) > 0.01f)
            {
                DOTween.To(() => dollyCart.m_Position, x => dollyCart.m_Position = x, targetPosition, followSpeed);
            }
        }

    }
}