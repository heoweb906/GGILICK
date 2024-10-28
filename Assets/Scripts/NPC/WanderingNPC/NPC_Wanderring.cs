using UnityEngine;
using UnityEngine.AI;

public class NPC_Wanderring : MonoBehaviour
{
    [Header("애니메이터")]
    private Animator anim;

    public Transform[] checkPoints; // 목표 지점 배열
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트
    private int currentCheckPointIndex = 0; // 현재 목표 지점 인덱스
    private int iWalkAnimNum;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        iWalkAnimNum = Random.Range(1, 3);

        if (checkPoints.Length > 0) MoveToNextCheckPoint();
    }

    void Update()
    {
        // 현재 목표 지점에 도착했는지 확인
        if (agent.pathPending)
            return;

        // 현재 목표 지점에 가까워졌을 때
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // 다음 목표 지점으로 이동
            currentCheckPointIndex++;
            if (currentCheckPointIndex < checkPoints.Length)
            {
                MoveToNextCheckPoint();
            }
            else Destroy(gameObject);
        }
        else
        {
            anim.SetInteger("Walk_Num", iWalkAnimNum); // 이동 중 애니메이션
        }
    }

    private void MoveToNextCheckPoint()
    {
        agent.SetDestination(checkPoints[currentCheckPointIndex].position);
        anim.SetInteger("Walk_Num", iWalkAnimNum); // 이동 애니메이션 시작
    }
}
