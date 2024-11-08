using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GuardM : MonoBehaviour
{
    public Animator anim;

    public GuardMStateMachine machine;
    public GuardM_CheckingArea area;
    public Vector3 transformHome;
    public NavMeshAgent nav;

    public float fAttackRange;
    

    private void Awake()
    {
        Init();

        anim = GetComponent<Animator>();
        transformHome = transform.position;
        nav = GetComponent<NavMeshAgent>(); 
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




}
