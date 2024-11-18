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
        StartShakeEffect(); // CarFrame�� ���� ȿ�� ����
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

        RotateTaeYub(); // TaeYub�� ȸ�� ȿ�� ����
    }


    // CarFrame�� ������ ȿ��
    private void StartShakeEffect()
    {
        // CarFrame�� ��ġ�� �̼��ϰ� ������ ȿ���� DOTween���� ����
        CarFrame.transform.DOShakePosition(
            duration: 0.3f, // ���� ���� �ð�
            strength: new Vector3(0.02f, 0.02f, 0f), // ���� ���� (X, Y�ุ ����)
            vibrato: 8, // ���� ��
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


}
