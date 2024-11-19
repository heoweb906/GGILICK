using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GGILICK_Car : MonoBehaviour
{
    [Header("�ڵ��� ���� �ִϸ��̼� ����")]
    public GameObject CarFrame;
    public GameObject TaeYub;

    [Header("�ڵ��� ������ ����")]
    public float fCarSpeed; // �ְ� �ӷ�
    public Transform transform_Destroy;
    private Rigidbody rb; // �ڵ����� Rigidbody ������Ʈ


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartShakeEffect(); // CarFrame�� ���� ȿ�� ����
    }

    private void Update()
    {
        MoveCar();

        if (transform.position.z < transform_Destroy.position.z) Destroy(gameObject); ;
    }

    private void FixedUpdate()
    {
        RotateTaeYub(); // TaeYub�� ȸ�� ȿ�� ����
    }


    // CarFrame�� ������ ȿ��
    private void StartShakeEffect()
    {
        // CarFrame�� ��ġ�� �̼��ϰ� ������ ȿ���� DOTween���� ����
        CarFrame.transform.DOShakePosition(
            duration: 0.1f, // ���� ���� �ð�
            strength: new Vector3(0.02f, 0.02f, 0f), // ���� ���� (X, Y�ุ ����)
            vibrato: 10, // ���� ��
            randomness: 90f, // ������
            fadeOut: false // ȿ���� ��� �����ǵ��� ����
        ).SetLoops(-1, LoopType.Restart) // ���� �ݺ�
         .SetEase(Ease.Linear)
         .SetRelative(true) // ���� ��ġ �������� ���� ����
         .SetUpdate(UpdateType.Fixed); // FixedUpdate���� ����ǵ��� ����
    }

    // TaeYub�� Z�� ȸ�� ȿ��
    private void RotateTaeYub()
    {
        // Z������ ȸ�� (Time.deltaTime�� ����� ������ ������ ȸ��)
        TaeYub.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
    }


    private void MoveCar()
    {
        // �ڵ����� Z�� �������� �̵� (fCarSpeed �ӵ���)
        transform.Translate(Vector3.back * fCarSpeed * Time.deltaTime);
    }



}
