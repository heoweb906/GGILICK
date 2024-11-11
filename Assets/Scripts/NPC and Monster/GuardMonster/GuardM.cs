using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GuardM : MonoBehaviour
{
    private GameObject player;
    private LayerMask obstacleLayerMask;
    private Vector3 guardRayOriginOffset = Vector3.up; // 감시자 Ray 시작 위치 오프셋
    private Vector3 playerRayTargetOffset = Vector3.up; // 플레이어 Ray 목표 위치 오프셋
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

    // #. 플레이어랑 경비병 사이에 장애물이 있는지 확인
    public bool IsObstacleBetween()
    {
        // 감시자와 플레이어의 위치에 오프셋 적용
        Vector3 guardPosition = transform.position + guardRayOriginOffset;
        Vector3 playerPosition = player.transform.position + playerRayTargetOffset;
        Vector3 direction = (playerPosition - guardPosition).normalized;
        float distance = Vector3.Distance(guardPosition, playerPosition);

        // Ray를 눈으로 확인하기 위해 Debug.DrawRay 사용
        Debug.DrawRay(guardPosition, direction * distance, Color.red);

        // Raycast로 감시자와 플레이어 사이를 검사 (Obstacle 레이어만 감지)
        if (Physics.Raycast(guardPosition, direction, out RaycastHit hit, distance, obstacleLayerMask))
        {
            Debug.Log("장애물이 감지됩니다");
            return true;
        }

        Debug.Log("장애물이 감지되지 않습니다.");
        return false;
    }



}
