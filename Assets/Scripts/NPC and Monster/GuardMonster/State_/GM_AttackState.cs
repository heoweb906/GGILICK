using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_AttackState : GuardMState
{
    public GM_AttackState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.anim.SetTrigger("doAttack");

        GameAssistManager.Instance.DiePlayerReset(2f);

        guardM.StartGuardCoroutine(AssistAnim(2f));
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bAnimEnd)
        {
            
            bAnimEnd = false;
            machine.OnStateChange(machine.BackHomeState);
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



    IEnumerator AssistAnim(float fWaitSecond)
    {
        yield return new WaitForSeconds(fWaitSecond);

        bAnimEnd = true;
    }

}
