using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClockWorkCatch : MonoBehaviour
{
    public float detectionRadius = 10f; // Ž�� �ݰ�
    private ClockWork closestClockWork; // ���� ����� ClockWork ������Ʈ

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ��Ŭ��
        {
            FindClosestClockWorkObject();
        }

        if (Input.GetMouseButton(0) && closestClockWork != null) // ��Ŭ���� ������ �ִ� ����
        {
            closestClockWork.ChargingBattery(); // OnClockWork �Լ� ȣ��
        }

        if (Input.GetMouseButtonUp(0)) // ���콺�� ����
        {
            if(closestClockWork != null)
            {
                closestClockWork.EndCharging_To_BatteryStart();
                closestClockWork = null; // ���� ����� ClockWork ���� �ʱ�ȭ
            }
        }
    }

    private void FindClosestClockWorkObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        closestClockWork = null; // ���� ���� �ʱ�ȭ
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            // ClockWork ��ũ��Ʈ�� �ִ��� Ȯ��
            ClockWork clockWork = collider.GetComponent<ClockWork>();
            if (clockWork != null)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClockWork = clockWork; // ���� ����� ClockWork ���� ����
                }
            }
        }

        // �θ� ������Ʈ�� ��� �ڽĿ����� ClockWork ��ũ��Ʈ�� �˻�
        foreach (Transform child in transform)
        {
            ClockWork clockWork = child.GetComponent<ClockWork>();
            if (clockWork != null)
            {
                float distance = Vector3.Distance(transform.position, child.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClockWork = clockWork; // ���� ����� ClockWork ���� ����
                }
            }
        }

        if (closestClockWork != null)
        {
            Debug.Log("���� ����� ClockWork ������Ʈ: " + closestClockWork.gameObject.name);
            // ���⿡�� �߰����� ������ ������ �� �ֽ��ϴ�.
        }
        else
        {
            Debug.Log("ClockWork ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void OnDrawGizmos()
    {
        // Ž�� �ݰ��� �ð������� ǥ��
        Gizmos.color = Color.green; // ����� ���� ����
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere�� Ž�� ���� �׸���
    }
}
