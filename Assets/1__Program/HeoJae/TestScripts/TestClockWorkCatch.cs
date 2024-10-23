using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClockWorkCatch : MonoBehaviour
{
    public float detectionRadius = 10f; // 탐지 반경
    private ClockWork closestClockWork; // 가장 가까운 ClockWork 오브젝트

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            FindClosestClockWorkObject();
        }

        if (Input.GetMouseButton(0) && closestClockWork != null) // 좌클릭을 누르고 있는 동안
        {
            closestClockWork.ChargingBattery(); // OnClockWork 함수 호출
        }

        if (Input.GetMouseButtonUp(0)) // 마우스를 떼면
        {
            if(closestClockWork != null)
            {
                closestClockWork.EndCharging_To_BatteryStart();
                closestClockWork = null; // 가장 가까운 ClockWork 참조 초기화
            }
        }
    }

    private void FindClosestClockWorkObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        closestClockWork = null; // 이전 참조 초기화
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            // ClockWork 스크립트가 있는지 확인
            ClockWork clockWork = collider.GetComponent<ClockWork>();
            if (clockWork != null)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClockWork = clockWork; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        // 부모 오브젝트의 모든 자식에서도 ClockWork 스크립트를 검사
        foreach (Transform child in transform)
        {
            ClockWork clockWork = child.GetComponent<ClockWork>();
            if (clockWork != null)
            {
                float distance = Vector3.Distance(transform.position, child.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestClockWork = clockWork; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        if (closestClockWork != null)
        {
            Debug.Log("가장 가까운 ClockWork 오브젝트: " + closestClockWork.gameObject.name);
            // 여기에서 추가적인 로직을 구현할 수 있습니다.
        }
        else
        {
            Debug.Log("ClockWork 오브젝트를 찾을 수 없습니다.");
        }
    }

    private void OnDrawGizmos()
    {
        // 탐지 반경을 시각적으로 표시
        Gizmos.color = Color.green; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere로 탐지 범위 그리기
    }
}
