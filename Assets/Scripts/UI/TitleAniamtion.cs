using UnityEngine;
using DG.Tweening;

public class TitleAniamtion : MonoBehaviour
{
    public float moveDistanceX = 50f; // �¿� �̵� �Ÿ�
    public float moveDistanceY = 20f; // ���� �̵� �Ÿ�
    public float moveDuration = 2f; // �̵� �ð�

    void Start()
    {
        Vector3 startPos = transform.localPosition;

        // �¿� + ���Ϸ� �ε巴�� �̵��ϴ� �ִϸ��̼�
        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(transform.DOLocalMoveX(startPos.x + moveDistanceX, moveDuration)
            .SetLoops(2, LoopType.Yoyo) // �� �� �Դ� ����
            .SetEase(Ease.InOutSine));

        moveSequence.Join(transform.DOLocalMoveY(startPos.y + moveDistanceY, moveDuration)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine));

        moveSequence.SetLoops(-1); // ���� �ݺ�
    }
}
