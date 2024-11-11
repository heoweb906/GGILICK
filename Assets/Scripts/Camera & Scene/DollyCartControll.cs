using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DollyCartControll : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    public CinemachineDollyCart dollyCart; // Cinemachine Dolly Cart
    public CinemachineSmoothPath dollyPath; // Cinemachine Path
    public DollyRotationAndPositonOffset dollyRotation;

    // īƮ�� �÷��̾� ��ġ�� ���� �̵��ϴ� �ӵ� ����
    public float followSpeed;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player != null && dollyPath != null)
        {
            // �÷��̾� ��ġ���� ���� ����� ������ ã��
            float closestPoint = dollyPath.FindClosestPoint(player.position, 0, -1, 10);
            float targetPosition = Mathf.Clamp(closestPoint, 0, dollyPath.PathLength);

            // ���� �ε����� ���� �ε��� ���
            int currentIndex = Mathf.FloorToInt(targetPosition);
            int nextIndex = currentIndex + 1;

            // �ε����� �迭 ������ ����� ���, ������ �ε����� ���
            if (currentIndex >= dollyRotation.Offsets.Length - 1)
            {
                currentIndex = dollyRotation.Offsets.Length - 1;
                nextIndex = currentIndex;
            }
            else if (nextIndex >= dollyRotation.Offsets.Length)
            {
                nextIndex = dollyRotation.Offsets.Length - 1;
            }

            // ���� ���� ���
            float t = targetPosition - currentIndex;

            // fPositionOffset ����
            float startOffset = dollyRotation.Offsets[currentIndex].fPositionOffest;
            float endOffset = dollyRotation.Offsets[nextIndex].fPositionOffest;
            float interpolatedOffset = Mathf.Lerp(startOffset, endOffset, t);

            // ������ ��ǥ ��ġ ���
            float adjustedTargetPosition = Mathf.Clamp(targetPosition + interpolatedOffset, 0, dollyPath.PathLength);

            // ���� ��ġ�� ��ǥ ��ġ�� �ٸ� ��쿡�� �̵�
            if (Mathf.Abs(dollyCart.m_Position - adjustedTargetPosition) > 0.01f)
            {
                DOTween.To(() => dollyCart.m_Position, x => dollyCart.m_Position = x, adjustedTargetPosition, followSpeed);
            }
        }
    }
}
