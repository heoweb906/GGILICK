using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sad_WalkState : NPC_Simple_State
{
    public NPC_Sad_WalkState(NPC_Simple npc, NPC_Simple_StateMachine machine) : base(npc, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();

        npc.GetNav().enabled = true;

        int ranNum = Random.Range(0, 2);
        npc.GetAnimator().SetInteger("Walk_Num", ranNum);
    }


    public override void OnUpdate()
    {
        base.OnUpdate();


        if (npc.checkPoints == null) return;

        if (npc.GetNav().remainingDistance <= npc.GetNav().stoppingDistance && npc.bWalking)
        {
            if (npc.CurrentCheckPointIndex < npc.checkPoints.Length)
            {
                MoveToNextCheckPoint();
            }
            else
            {
                Object.Destroy(npc.gameObject);
            }
            npc.CurrentCheckPointIndex++;
        }

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        npc.GetNav().enabled = false;
    }


    private void MoveToNextCheckPoint()
    {
        npc.GetNav().SetDestination(npc.checkPoints[npc.CurrentCheckPointIndex].position);
        Debug.Log("걷는 중입니다!!!!!.");
    }
}
