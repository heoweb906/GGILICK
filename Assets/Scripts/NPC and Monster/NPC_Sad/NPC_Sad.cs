using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Sad : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent nav;

    public NPC_Sad_StateMachine machine;
   
    private void Awake()
    {
        Init();

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

    }
    private void Init()
    {
        machine = new NPC_Sad_StateMachine(this);
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
