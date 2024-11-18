using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RoadCarOnTrack : CinemachineDollyCart
{
    public GameObject CarFrame;
    public GameObject TaeYub;


    private void Start()
    {
        StartShakeEffect(); // CarFrame의 떨림 효과 시작
    }


    private void FixedUpdate()
    {
        var trackLength = m_Path.PathLength;

        if (m_Path.Looped)
        {
            m_Position = m_Position % trackLength;
        }
        else if (m_Position >= trackLength)
        {
            Destroy(gameObject);
        }

        RotateTaeYub(); // TaeYub의 회전 효과 적용
    }


    // CarFrame이 떨리는 효과
    private void StartShakeEffect()
    {
        // CarFrame의 위치를 미세하게 떨리는 효과를 DOTween으로 적용
        CarFrame.transform.DOShakePosition(
            duration: 0.3f, // 떨림 지속 시간
            strength: new Vector3(0.02f, 0.02f, 0f), // 떨림 강도 (X, Y축만 떨림)
            vibrato: 8, // 떨림 빈도
            randomness: 90f, // 랜덤성
            fadeOut: false // 효과가 계속 유지되도록 설정
        ).SetLoops(-1, LoopType.Restart) // 무한 반복
         .SetEase(Ease.Linear)
         .SetRelative(true) // 로컬 위치 기준으로 떨림 적용
         .SetUpdate(UpdateType.Fixed); // FixedUpdate에서 실행되도록 설정
    }

    // TaeYub의 Z축 회전 효과
    private void RotateTaeYub()
    {
        // Z축으로 회전 (Time.deltaTime을 사용해 프레임 독립적 회전)
        TaeYub.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }


}
