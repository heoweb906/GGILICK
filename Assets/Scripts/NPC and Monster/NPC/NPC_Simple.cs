using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum ClockWorkEventList
{
    None   // Num 0
}

public enum ActionEventList
{ 
    None,            // Num 0;
    WorkInCompany    // Num 1;
}


public class NPC_Simple : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    public NPC_Simple_StateMachine machine;


    [Header("NPC ���µ�")]
    public bool bSad; // true = Sad <-> false = NotSad
    public bool bWalking; // true = Walking <-> false = IDLE
    public bool bClockWorkEventNPC; // true = �¿��� ������ ���Ŀ� Ư���� �̺�Ʈ�� ���ϴ� ĳ��������
    public bool bActionEventNPC; // ���� �������� Ư���� �ൿ�� ���ϰ� �ִ� NPC;
    public ClockWorkEventList clockworkEvent;
    public ActionEventList actionEventList;

    [Header("��Ÿ ������Ʈ��")]
    public NPC_ClockWork npc_ClockWork; // �ڱ� � ���� �ִ� �¿�
    private ClockWork clockWork;        // ���� ȸ�� ��ų �¿�


    [Header("Walk ���� ������Ʈ��")]
    public Transform[] checkPoints; // ��ǥ ���� �迭
    private int currentCheckPointIndex = 0; // ���� ��ǥ ���� �ε���


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Bool_Walk", bWalking);
        anim.SetBool("Bool_Sad", bSad);
        anim.SetBool("Bool_ActionEvent", bActionEventNPC);

        agent = GetComponent<NavMeshAgent>();


        machine = new NPC_Simple_StateMachine(this);
        npc_ClockWork.machine = machine;
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


    public ClockWork ClockWork
    {
        get { return clockWork; }  
        set { clockWork = value; }
    }

    public void SpinClockWork()
    {
        clockWork.ClockWorkRotate();
    }





}
