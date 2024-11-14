using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sad_State : BaseState
{
    protected NPC_Sad npc;
    protected NPC_Sad_StateMachine machine;


    public NPC_Sad_State(NPC_Sad _guardM, NPC_Sad_StateMachine _machine)
    {
        npc = _guardM;
        machine = _machine;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    public virtual void OnFixedUpdate()
    {

    }

    public virtual void OnUpdate()
    {

    }





    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
}
