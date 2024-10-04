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
                // ������ �ӵ��� ���� �׸��鼭 ������ (�ݰ� 5, �ֱ� 4��)
                DOTween.To(() => 0f, angle =>
                {
                    // ������ ���� x, z ��ǥ ���
                    float x = Mathf.Cos(angle) * moveDistance;
                    float z = Mathf.Sin(angle) * moveDistance;

                    // �� ��ġ�� ����
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
