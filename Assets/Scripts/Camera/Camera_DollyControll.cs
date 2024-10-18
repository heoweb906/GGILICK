using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_DollyControll : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public CinemachineDollyCart dollyCart; // Cinemachine Dolly Cart
    public CinemachineSmoothPath dollyPath; // Cinemachine Path

    // īƮ�� �÷��̾� ��ġ�� ���� �̵��ϴ� �ӵ� ����
    public float followSpeed;

    void FixedUpdate()
    {
        if (player != null && dollyPath != null)
        {
            float closestPoint = dollyPath.FindClosestPoint(player.position, 0, -1, 10);
            float targetPosition = Mathf.Clamp(closestPoint, 0, dollyPath.PathLength);

            // ���� ��ġ�� ��ǥ ��ġ�� �ٸ� ��쿡�� �̵�
            if (Mathf.Abs(dollyCart.m_Position - targetPosition) > 0.01f)
            {
                DOTween.To(() => dollyCart.m_Position, x => dollyCart.m_Position = x, targetPosition, followSpeed);
            }
        }

    }
}