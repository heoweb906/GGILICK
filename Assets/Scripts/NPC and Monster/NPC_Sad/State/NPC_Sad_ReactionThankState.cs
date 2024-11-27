using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NPC_Sad_ReactionThankState : NPC_Sad_State
{
    public NPC_Sad_ReactionThankState(NPC_Sad npc, NPC_Sad_StateMachine machine) : base(npc, machine) { }


    private float elapsedTime; // �ð� ������ ����
    private const float duration = 2f; // 2�� �Ŀ� ���� ����


    public override void OnEnter()
    {
        base.OnEnter();

        npc.GetAnimator().SetTrigger("doReactionHappy");

        elapsedTime = 0f; 

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        // 2�ʰ� ������ ChangeStateNPC ����
        if (elapsedTime >= duration)
        {
            npc.CurrentCheckPointIndex--;
            machine.OnStateChange(npc.bWalking ? machine.WalkState : machine.IDLEState);
        }

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
