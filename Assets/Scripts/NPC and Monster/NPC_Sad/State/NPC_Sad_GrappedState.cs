using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sad_GrappedState : NPC_Sad_State
{
    public NPC_Sad_GrappedState(NPC_Sad npc, NPC_Sad_StateMachine machine) : base(npc, machine) { }


    private float elapsedTime; // �ð� ������ ����
    private const float duration = 1.5f; // 2�� �Ŀ� ���� ����


    public override void OnEnter()
    {
        base.OnEnter();

        npc.GetAnimator().SetTrigger("doReactionGrapped");

        elapsedTime = 0f; // ���� ���� �� �ð� �ʱ�ȭ
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        // 2�ʰ� ������ ChangeStateNPC ����
        if (elapsedTime >= duration)
        {
            ChangeStateNPC();
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


    private void ChangeStateNPC()
    {
        npc.bSad = false;
        npc.GetAnimator().SetBool("Bool_Sad",false);


        if (npc.bEventNPC)
        {


        }
        else
        {
            machine.OnStateChange(machine.ThankState);
        }
    }

    
}
