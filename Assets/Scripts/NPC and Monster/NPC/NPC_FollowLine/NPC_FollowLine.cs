using UnityEngine;
using UnityEngine.AI;

public class NPC_FollowLine : MonoBehaviour
{
    [Header("애니메이터")]
    private Animator anim;

    public Transform[] checkPoints; // 목표 지점 배열
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트
    private int currentCheckPointIndex = 0; // 현재 목표 지점 인덱스
    private int iWalkAnimNum;

    private float pushBackForce = 2f; 
    private bool isPushedBack = false; 

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        iWalkAnimNum = Random.Range(1, 2); // 걸어다니는 종류를 설정해줄 수 있음

        if (checkPoints.Length > 0) MoveToNextCheckPoint();
    }

    void Update()
    {
        if (isPushedBack || checkPoints != null)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
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



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPushedBack)
        {
            StopAllCoroutines();

            // NPC를 밀어내기
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(PushBack(pushDirection));
        }
    }

    private System.Collections.IEnumerator PushBack(Vector3 direction)
    {
        isPushedBack = true;
        agent.isStopped = true; // 이동 중지
        anim.SetTrigger("doTakeHitSholder"); // 애니메이션 트리거

        float pushDuration = 4.0f; // 밀리는 시간
        float elapsed = 0f;

        while (elapsed < pushDuration)
        {
            elapsed += Time.deltaTime;

            if (elapsed < 0.25f) transform.position += direction * pushBackForce * Time.deltaTime;
            else transform.position += direction * 0f * Time.deltaTime;

            yield return null;
        }

        isPushedBack = false;
        agent.isStopped = false; // 이동 재개
    }








}
