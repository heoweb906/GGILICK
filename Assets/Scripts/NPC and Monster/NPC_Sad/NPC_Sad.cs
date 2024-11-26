using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Sad : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent nav;
    public NPC_Sad_StateMachine machine;


    [Header("NPC ���µ�")]
    public bool bSad; // true = Sad <-> false = NotSad
    public bool bWalking; // true = Walking <-> false = IDLE
    public bool bEventNPC; // true = �¿��� ������ ���Ŀ� Ư���� �̺�Ʈ�� ���ϴ� ĳ��������

    [Header("��Ÿ ������Ʈ��")]
    public NPC_ClockWork clockWork;


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Bool_Walk", bWalking);
        anim.SetBool("Bool_Sad", bSad);

        nav = GetComponent<NavMeshAgent>();

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
        return nav;
    }
}
