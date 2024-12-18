using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrackingHead_ToPlayer : MonoBehaviour
{
    public bool bFindPlayer;
    public Vector3 maxRotate; // 최대 회전값
    public Vector3 minRotate; // 최소 회전값
    private float rotationSpeed = 1f; // 회전 속도 (1초에 한번 회전)
    private float timeSinceLastRotation = 0f;

    void Update()
    {
        if (bFindPlayer && GameAssistManager.Instance.player != null)
        {
            // 플레이어를 찾았을 때의 회전
            Vector3 direction = GameAssistManager.Instance.player.transform.position - transform.position;
            direction.y = 0f;  // x, z 평면에서만 회전하도록 y값을 0으로 설정

            // 타겟 방향을 계산 (z축과 y축만 회전하도록 설정)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // x축을 0으로 고정하고, y, z축만 회전하도록 설정
            targetRotation.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y + 90f, targetRotation.eulerAngles.z);

            // 보간을 통해 부드럽게 회전
            float rotationSpeed = 5f;  // 회전 속도 조절
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // 플레이어를 찾지 못했을 때, 랜덤으로 두리번거리는 회전
            timeSinceLastRotation += Time.deltaTime;

            if (timeSinceLastRotation >= 1f) // 1초마다 랜덤 회전값을 갱신
            {
                // 최소값과 최대값 사이에서 랜덤한 회전값을 계산 (y, z축만 회전)
                float randomY = Random.Range(minRotate.y, maxRotate.y);
                float randomZ = Random.Range(minRotate.z, maxRotate.z);

                // 랜덤 회전값 (x축은 항상 0으로 고정)
                Vector3 randomRotation = new Vector3(0f, randomY, randomZ);

                // DOTween을 사용하여 부드럽게 회전
                transform.DORotate(randomRotation, rotationSpeed).SetEase(Ease.InOutSine);

                // 타이머 초기화
                timeSinceLastRotation = 0f;
            }
        }
    }

}
