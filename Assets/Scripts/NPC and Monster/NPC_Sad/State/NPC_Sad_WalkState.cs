using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sad_WalkState : NPC_Sad_State
{
    public NPC_Sad_WalkState(NPC_Sad npc, NPC_Sad_StateMachine machine) : base(npc, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        int ranNum = Random.Range(0, 2);
        npc.GetAnimator().SetInteger("Walk_Num", ranNum);
    }


    public override void OnUpdate()
    {
        base.OnUpdate();



    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();
    }
}
