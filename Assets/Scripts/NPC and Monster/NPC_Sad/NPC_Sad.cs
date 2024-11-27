using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Sad : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public NPC_Sad_StateMachine machine;


    [Header("NPC 상태들")]
    public bool bSad; // true = Sad <-> false = NotSad
    public bool bWalking; // true = Walking <-> false = IDLE
    public bool bEventNPC; // true = 태엽을 돌려준 이후에 특정한 이벤트를 취하는 캐릭터인지

    [Header("기타 오브젝트들")]
    public NPC_ClockWork clockWork;


    [Header("Walk 관련 컴포넌트들")]
    public Transform[] checkPoints; // 목표 지점 배열
    private int currentCheckPointIndex = 0; // 현재 목표 지점 인덱스


  
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Bool_Walk", bWalking);
        anim.SetBool("Bool_Sad", bSad);

        agent = GetComponent<NavMeshAgent>();


        machine = new NPC_Sad_StateMachine(this);
        clockWork.machine = machine;
    }


    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }




    public Animator GetAnimator()
    {
        return anim;
    }



    public NavMeshAgent GetNav()
    {
        return agent;
    }



    public int CurrentCheckPointIndex
    {
        get => currentCheckPointIndex;
        set => currentCheckPointIndex = value;
    }



}
