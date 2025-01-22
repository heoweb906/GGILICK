using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulBug_SleepState : BulbBugState
{
    public BulBug_SleepState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("¿‡µÎ ªÛ≈¬ ¡¯¿‘ øœ∑·");
        bulbBug.LightObj.SetActive(false);
        bulbBug.carriedObj.enabled = true;

        bulbBug.rigid.isKinematic = true;

        bulbBug.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!bulbBug.CheckingArea_2.isPlayerInArea) machine.OnStateChange(machine.WanderingState);


    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        bulbBug.LightObj.SetActive(true);
        bulbBug.carriedObj.enabled = false;
        
    }

}
