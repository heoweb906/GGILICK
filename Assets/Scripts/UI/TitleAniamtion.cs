using UnityEngine;
using DG.Tweening;

public class TitleAniamtion : MonoBehaviour
{
    public float moveDistanceX = 50f; // 좌우 이동 거리
    public float moveDistanceY = 20f; // 상하 이동 거리
    public float moveDuration = 2f; // 이동 시간

    void Start()
    {
        Vector3 startPos = transform.localPosition;

        // 좌우 + 상하로 부드럽게 이동하는 애니메이션
        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(transform.DOLocalMoveX(startPos.x + moveDistanceX, moveDuration)
            .SetLoops(2, LoopType.Yoyo) // 한 번 왔다 갔다
            .SetEase(Ease.InOutSine));

        moveSequence.Join(transform.DOLocalMoveY(startPos.y + moveDistanceY, moveDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine));

        moveSequence.SetLoops(-1); // 무한 반복
    }
}
