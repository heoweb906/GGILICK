using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShakeEffect : MonoBehaviour
{
    public GameObject CarFrame; // ������ ���� �ڵ��� ������
    private Rigidbody rb;       

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        StartShakeEffect(); 
    }

 

    // CarFrame�� ������ ȿ��
    private void StartShakeEffect()
    {
        // CarFrame�� ��ġ�� �̼��ϰ� ������ ȿ���� DOTween���� ����
        CarFrame.transform.DOShakePosition(
            duration: 0.3f, // ���� ���� �ð�
            strength: new Vector3(0.01f, 0.01f, 0f), // ���� ���� (X, Y�ุ ����)
            vibrato: 2, // ���� ��
            randomness: 90f, // ������
            fadeOut: false // ȿ���� ��� �����ǵ��� ����
        ).SetLoops(-1, LoopType.Restart) // ���� �ݺ�
         .SetEase(Ease.Linear)
         .SetRelative(true) // ���� ��ġ �������� ���� ����
         .SetUpdate(UpdateType.Fixed); // FixedUpdate���� ����ǵ��� ����
    }
}
