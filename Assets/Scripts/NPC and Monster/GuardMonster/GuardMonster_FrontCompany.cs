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

    private Vector3 initialPosition; // �ʱ� ��ġ ����
    private Quaternion initialRotation; // �ʱ� ȸ���� ����
    private float rotationSpeed = 3f;
    private bool bAssist = false;

    public GuardMonaster_CheckingArea checkingArea;

    private float rayHeightOffset = 1.5f;
    public float moveSpeed = 2f; // ���� �̵� �ӵ�
    public LayerMask obstacleLayer; // ��ֹ� ���̾ ������ �� �ִ� ����

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

        // ���� ���¿����� �̵�
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

            // ��ֹ� üũ (������ ���̾ �˻�)
            if (!Physics.Raycast(rayOrigin, direction, distance, obstacleLayer) && state == MonsterState.Ready && !bAssist)
            {
                // �ִϸ��̼� Ʈ���� ����
                anim.SetTrigger("doFindPlayer");
                bAssist = true;
                StartCoroutine(StartChasingAfterAnimation());
            }
        }
        else
        {
            state = MonsterState.Ready; // �÷��̾ ���� �� ���¸� Ready�� ����
        }
    }

    private IEnumerator StartChasingAfterAnimation()
    {
        // "GuardMonster_FindPlayer" �ִϸ��̼��� ���� ������ ���
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
            transform.position = initialPosition; // ��Ȯ�� �ʱ� ��ġ�� ���߱�
            transform.rotation = initialRotation; // ���� �������� ȸ��
            state = MonsterState.Ready; // ���¸� Ready�� ����
        }

        if (anim.GetBool("isWalking") != true) anim.SetBool("isWalking", true);

        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            transform.position = initialPosition; // ��Ȯ�� �ʱ� ��ġ�� ���߱�
            transform.rotation = initialRotation; // ���� �������� ȸ��
            state = MonsterState.Ready; // ���¸� Ready�� ����

            // ���� ���� �� �ִϸ��̼� ����
            if (anim.GetBool("isWalking") != false) anim.SetBool("isWalking", false);
        }
    }

    private void ObjRotaate(Vector3 dir)
    {
        if (dir != Vector3.zero) // ������ 0�� �ƴ� ���� ȸ��
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
