using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrackingHead_ToPlayer : MonoBehaviour
{
    public bool bFindPlayer;
    public Vector3 maxRotate; // �ִ� ȸ����
    public Vector3 minRotate; // �ּ� ȸ����
    private float rotationSpeed = 1f; // ȸ�� �ӵ� (1�ʿ� �ѹ� ȸ��)
    private float timeSinceLastRotation = 0f;

    void Update()
    {
        if (bFindPlayer && GameAssistManager.Instance.player != null)
        {
            // �÷��̾ ã���� ���� ȸ��
            Vector3 direction = GameAssistManager.Instance.player.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction);

            // x�࿡ +90���� �߰�
            Quaternion targetRotation = Quaternion.Euler(toRotation.eulerAngles.x - 80f, toRotation.eulerAngles.y, toRotation.eulerAngles.z);

            // ������ ���� �ε巴�� ȸ��
            float rotationSpeed = 5f;  // ȸ�� �ӵ� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // �÷��̾ ã�� ������ ��, �������� �θ����Ÿ��� ȸ��
            timeSinceLastRotation += Time.deltaTime;

            if (timeSinceLastRotation >= 1f) // 1�ʸ��� ���� ȸ������ ����
            {
                // �ּҰ��� �ִ밪 ���̿��� ������ ȸ������ ���
                float randomX = Random.Range(minRotate.x, maxRotate.x);
                float randomZ = Random.Range(minRotate.z, maxRotate.z);

                // ���� ȸ����
                Vector3 randomRotation = new Vector3(randomX, 0f, randomZ);

                // DOTween�� ����Ͽ� �ε巴�� ȸ��
                transform.DORotate(randomRotation, rotationSpeed).SetEase(Ease.InOutSine);

                // Ÿ�̸� �ʱ�ȭ
                timeSinceLastRotation = 0f;
            }
        }
    }

}
