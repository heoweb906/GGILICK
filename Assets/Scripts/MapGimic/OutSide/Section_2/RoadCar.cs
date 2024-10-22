using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadCar : MonoBehaviour
{
    [HideInInspector] public TrafficLight trafficLight; // ���� ��ȣ
    public bool bMoveActive;

    public float maxSpeed; // �ְ� �ӷ�
    public float safeDistance; // �����Ÿ�
    public float deceleration; // ���� ����
    public float acceleration; // ���� ����

    private Rigidbody rb; // �ڵ����� Rigidbody ������Ʈ
    private float currentSpeed; // ���� �ӷ�

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = maxSpeed; // �ʱ� �ӷ��� �ְ� �ӷ����� ����
        Debug.Log("������");
    }

    private void Update()
    {
        if (bMoveActive)
        {
            MoveCar(); // �ڵ��� �̵� �޼��� ȣ��
            // positionEnd�� z���� �Ѿ�� �ڵ��� ����
            if (transform.position.z > trafficLight.postion_end.position.z) RemoveCar();
        }
    }

    private void MoveCar()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 2; // Ray �߻� ��ġ�� ��¦ �÷���

        // �浹�� �����Ǹ� ����
        if (Physics.Raycast(rayStart, transform.forward, out hit, safeDistance))
        {
            if (hit.collider.GetComponent<RoadCar>() != null)
            {
                // ���� �Ÿ� Ȯ���� ���� ������ ����
                currentSpeed -= deceleration * Time.deltaTime;
                // �ӵ��� 0 ���Ϸ� �������� �ʵ��� ���� (�ڷ� ���� �ʵ��� ����)
                currentSpeed = Mathf.Max(currentSpeed, 0);

                // �ӵ��� 0�� �Ǹ� ����
                if (currentSpeed == 0) rb.velocity = Vector3.zero;
            }
        }
        else
        {
            // ���� �Ÿ��� Ȯ���Ǹ� ����
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
        }

        // �ڵ����� �ӵ��� ���� (�ڷ� ���� ���� ����)
        rb.velocity = transform.forward * Mathf.Max(currentSpeed, 0);
    }

    private void RemoveCar()
    {
        // ����Ʈ���� �� �ڵ��� ����
        if (trafficLight != null && trafficLight.spawnedCars.Contains(gameObject)) trafficLight.spawnedCars.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayStart = transform.position + Vector3.up * 2;
        Gizmos.DrawLine(rayStart, rayStart + transform.forward * safeDistance);
    }
}
