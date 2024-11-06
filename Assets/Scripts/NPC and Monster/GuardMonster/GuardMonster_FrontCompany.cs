using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum MonsterState
{
    Ready,
    Attack
}

public class GuardMonster_FrontCompany : MonoBehaviour
{
    private Transform playerPosition;
    private Animator anim;
    private MonsterState state;

    private Vector3 initialPosition; // 초기 위치 저장
    private Quaternion initialRotation; // 초기 회전값 저장
    private float rotationSpeed = 3f;
    private bool bAssist = false;

    public GuardMonaster_CheckingArea checkingArea;

    private float rayHeightOffset = 1.5f;
    public float moveSpeed = 2f; // 몬스터 이동 속도
    public LayerMask obstacleLayer; // 장애물 레이어를 설정할 수 있는 변수

    private void Start()
    {
        anim = GetComponent<Animator>();
        state = MonsterState.Ready;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        Debug.Log(state);


     StateCheck();

        // 공격 상태에서만 이동
        if (state == MonsterState.Attack) MoveTowardsPlayer();
        else if (state == MonsterState.Ready) BackHome();
    }

    private void StateCheck()
    {
        if (checkingArea.IsPlayerInArea() && checkingArea.GetPlayerPosition() != null)
        {
            Transform playerTransform = checkingArea.GetPlayerPosition();

            Vector3 direction = (playerTransform.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;

            // 장애물 체크 (지정된 레이어만 검사)
            if (!Physics.Raycast(rayOrigin, direction, distance, obstacleLayer) && state == MonsterState.Ready && !bAssist)
            {
                // 애니메이션 트리거 실행
                anim.SetTrigger("doFindPlayer");
                bAssist = true;
                StartCoroutine(StartChasingAfterAnimation());
            }
        }
        else
        {
            state = MonsterState.Ready; // 플레이어가 없을 때 상태를 Ready로 설정
        }
    }

    private IEnumerator StartChasingAfterAnimation()
    {
        // "GuardMonster_FindPlayer" 애니메이션이 끝날 때까지 대기
        yield return new WaitForSecondsRealtime(2f);
            
            //WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("GuardMonster_FindPlayer") &&
            //                               anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        state = MonsterState.Attack;
        bAssist = false;
    }

    private void MoveTowardsPlayer()
    {
     
        Transform playerTransform = checkingArea.GetPlayerPosition();
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        ObjRotaate(direction);
        if (anim.GetBool("isWalking") != true) anim.SetBool("isWalking", true);
    }

    private void BackHome()
    {
        Vector3 direction = (initialPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        ObjRotaate(direction);

        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            transform.position = initialPosition; // 정확히 초기 위치에 맞추기
            transform.rotation = initialRotation; // 원래 방향으로 회전
            state = MonsterState.Ready; // 상태를 Ready로 유지
        }

        if (anim.GetBool("isWalking") != true) anim.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            transform.position = initialPosition; // 정확히 초기 위치에 맞추기
            transform.rotation = initialRotation; // 원래 방향으로 회전
            state = MonsterState.Ready; // 상태를 Ready로 유지

            // 멈춰 있을 때 애니메이션 설정
            if (anim.GetBool("isWalking") != false) anim.SetBool("isWalking", false);
        }
    }

    private void ObjRotaate(Vector3 dir)
    {
        if (dir != Vector3.zero) // 방향이 0이 아닐 때만 회전
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
