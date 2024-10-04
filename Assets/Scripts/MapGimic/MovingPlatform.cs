using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MovingPlatform : MonoBehaviour
{
    public enum PlatformMoveType
    {
        X,
        Y,
        Circle
    }

    private Vector3 prePosition;
    private Vector3 curPosition;

    [SerializeField]
    private PlatformMoveType moveType;

    [SerializeField, Range(0, 20)]
    private float moveDistance;
    [SerializeField, Range(0, 10)]
    private float moveTime;


    private void Start()
    {
        switch (moveType)
        {
            case PlatformMoveType.X:
                gameObject.transform.DOLocalMoveX(moveDistance, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
            case PlatformMoveType.Y:
                gameObject.transform.DOLocalMoveY(moveDistance, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
            case PlatformMoveType.Circle:
                // 일정한 속도로 원을 그리면서 움직임 (반경 5, 주기 4초)
                DOTween.To(() => 0f, angle =>
                {
                    // 각도에 따른 x, z 좌표 계산
                    float x = Mathf.Cos(angle) * moveDistance;
                    float z = Mathf.Sin(angle) * moveDistance;

                    // 새 위치를 설정
                    transform.position = new Vector3(x, transform.position.y, z);
                }, Mathf.PI * 2, moveTime).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
                break;
        }
    }

    private void FixedUpdate()
    {
        prePosition = curPosition;
        curPosition = gameObject.transform.position;
        //Debug.Log(GetPlatformVelocity());
    }

    public Vector3 GetPlatformVelocity()
    {
        Vector3 velo = (curPosition - prePosition) / Time.deltaTime;
        velo.y = 0;
        return velo;
    }

}
