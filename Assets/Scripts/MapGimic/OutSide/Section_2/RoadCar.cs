using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RoadCar : MonoBehaviour
{
    [HideInInspector] public TrafficLight trafficLight; // ���� ��ȣ
    public bool bMoveActive;
    public bool bDirection;

    public float maxSpeed; // �ְ� �ӷ�
    public float safeDistance; // �����Ÿ�
    public float deceleration; // ���� ����
    public float acceleration; // ���� ����

    private Rigidbody rb; // �ڵ����� Rigidbody ������Ʈ
    private float currentSpeed; // ���� �ӷ�

    public JustRotate[] justRotates;  // Ÿ�̾�� ȸ�� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = maxSpeed; // �ʱ� �ӷ��� �ְ� �ӷ����� ����
    }

    private void Update()
    {
        if (bMoveActive)
        {
            MoveCar(); // �ڵ��� �̵� �޼��� ȣ��

            // positionEnd�� z���� �Ѿ�� �ڵ��� ����
            if ((bDirection && transform.position.z < trafficLight.postions_end[0].position.z) ||
                (!bDirection && transform.position.z > trafficLight.postions_end[1].position.z))
            {
                RemoveCar();
            }
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
                currentSpeed = Mathf.Max(currentSpeed, 0);
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

        // MovePosition�� ����� ������Ʈ �̵�
        Vector3 targetPosition = transform.position + transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(targetPosition);

        // JustRotate �迭�� �� ��� ȸ�� �ӵ� ������Ʈ
        foreach (var rotate in justRotates)
        {
            if (rotate != null)
            {
                rotate.rotationSpeed = currentSpeed * 10f; // �ӵ��� ����ϵ��� ���� (10f�� ���� ������)
            }
        }
    }


    private void RemoveCar()
    {
        // ����Ʈ���� �� �ڵ��� ����
        if(bDirection)
        {
            if (trafficLight != null && trafficLight.spawnedCars_1.Contains(gameObject)) trafficLight.spawnedCars_1.Remove(gameObject);
        }
        else
        {
            if (trafficLight != null && trafficLight.spawnedCars_2.Contains(gameObject)) trafficLight.spawnedCars_2.Remove(gameObject);
        }

        DOTween.Kill(gameObject);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayStart = transform.position + Vector3.up * 2;
        Gizmos.DrawLine(rayStart, rayStart + transform.forward * safeDistance);
    }


    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �±װ� "Player"���� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Detect On Player");
            GameAssistManager.Instance.DiePlayerReset(2f);
        }
    }
}
