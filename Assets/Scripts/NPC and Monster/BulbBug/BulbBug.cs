using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulbBug : MonoBehaviour
{
    public Animator anim;

    public BulbBugStateMachine machine;

    [Header("CarriedObj 관련")]
    private LayerMask layer;
    private CarriedObject carriedObj;
    // gameObject.layer = LayerMask.NameToLayer("Player");


    [Header("플레이어 감지 Ray")]
    public GameObject CheckingAreaObj_1;
    public GameObject CheckingAreaObj_2;
    private PlayerCheckArea CheckingArea_1;
    private PlayerCheckArea CheckingArea_2;



    private void Awake()
    {
        Init();

        anim = GetComponent<Animator>();
 
    }
    private void Init()
    {
        machine = new BulbBugStateMachine(this);

        carriedObj = GetComponent<CarriedObject>();
        CheckingArea_1 = CheckingAreaObj_1.GetComponent<PlayerCheckArea>();
        CheckingArea_2 = CheckingAreaObj_2.GetComponent<PlayerCheckArea>();
    }



    private void Update()
    {
        machine?.OnStateUpdate();

    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }



    



}
