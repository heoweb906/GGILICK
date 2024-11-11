using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GuardM : MonoBehaviour
{
    private GameObject player;
    private LayerMask obstacleLayerMask;
    private Vector3 guardRayOriginOffset = Vector3.up; // ������ Ray ���� ��ġ ������
    private Vector3 playerRayTargetOffset = Vector3.up; // �÷��̾� Ray ��ǥ ��ġ ������
    private Vector3 transformHome;

    public Animator anim;

    public GuardMStateMachine machine;
    public GuardM_CheckingArea area;
    public NavMeshAgent nav;
    public GameObject transformPlayerGrab; 

    public float fAttackRange;
    

    private void Awake()
    {
        Init();

        anim = GetComponent<Animator>();
        transformHome = transform.position;
        nav = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player").transform.root.gameObject;
        obstacleLayerMask = LayerMask.GetMask("Obstacle");
    }
    private void Init()
    {
        machine = new GuardMStateMachine(this);
    }



    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }



    public void StartGuardCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public Vector3 GetHomeTransform()
    {
        return transformHome;
    }

    public GameObject GetPlayerObj()
    {
        return player;
    }

    // #. �÷��̾�� ��� ���̿� ��ֹ��� �ִ��� Ȯ��
    public bool IsObstacleBetween()
    {
        // �����ڿ� �÷��̾��� ��ġ�� ������ ����
        Vector3 guardPosition = transform.position + guardRayOriginOffset;
        Vector3 playerPosition = player.transform.position + playerRayTargetOffset;
        Vector3 direction = (playerPosition - guardPosition).normalized;
        float distance = Vector3.Distance(guardPosition, playerPosition);

        // Ray�� ������ Ȯ���ϱ� ���� Debug.DrawRay ���
        Debug.DrawRay(guardPosition, direction * distance, Color.red);

        // Raycast�� �����ڿ� �÷��̾� ���̸� �˻� (Obstacle ���̾ ����)
        if (Physics.Raycast(guardPosition, direction, out RaycastHit hit, distance, obstacleLayerMask))
        {
            Debug.Log("��ֹ��� �����˴ϴ�");
            return true;
        }

        Debug.Log("��ֹ��� �������� �ʽ��ϴ�.");
        return false;
    }



}
