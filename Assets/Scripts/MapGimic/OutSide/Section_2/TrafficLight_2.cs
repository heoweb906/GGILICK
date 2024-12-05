using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficLight_2 : MonoBehaviour
{
    [Header("��ȣ�� �Һ� ����")]
    public GameObject[] TrafficThreeColors;   // ���� ��ȣ��
    public GameObject[] TrafficTwoClolors;    // ���� ��ȣ��

    public TrafficClockWorkAssist[] trafficClockWorkAssists;




    // #. ��ȣ�� �� ��ü �Լ� 
    // 0 = ������, 1 = �����, 2 = �ʷϺ�
    public void ChangeTrafficColor_(int index)
    {
        if (index < 0 || index >= TrafficThreeColors.Length) return;

        for (int i = 0; i < TrafficThreeColors.Length; i++) TrafficThreeColors[i].SetActive(false);
        for (int i = 0; i < TrafficTwoClolors.Length; i++) TrafficTwoClolors[i].SetActive(false);

        // ���� ��ȣ�� ����
        TrafficThreeColors[index].SetActive(true);


        // �ε� ��ȣ�� ����
        if (index == 0) TrafficTwoClolors[1].SetActive(true);
        else TrafficTwoClolors[0].SetActive(true);
    }

    public void SpinClockWork(int spinTime)
    {
        for (int i = 0; i < trafficClockWorkAssists?.Length; i++)
        {
            trafficClockWorkAssists[i].RotateObject(spinTime + 3, i % 2 == 0 ? 1f : -1f);
        }
    }
}
