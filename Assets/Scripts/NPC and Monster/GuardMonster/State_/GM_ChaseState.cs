using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GM_ChaseState : GuardMState
{
    public GM_ChaseState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;


    public override void OnEnter()
    {
        base.OnEnter();

        guardM.anim.SetTrigger("doFindPlayer");

        guardM.StartGuardCoroutine(AssistAnim(2f));
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bAnimEnd)
        {
            if (guardM.area.playerPosition != null && guardM.area.isPlayerInArea)
            {
                guardM.nav.SetDestination(guardM.area.playerPosition.position);

                float distanceToTarget = Vector3.Distance(guardM.transform.position, guardM.area.playerPosition.position);
                if (distanceToTarget <= guardM.fAttackRange)
                {
                    bAnimEnd = false;
                    machine.OnStateChange(machine.AttackState);
                }
            }
            else
            {
                bAnimEnd = false;
                guardM.nav.isStopped = true;
                machine.OnStateChange(machine.BackHomeState);
            }
            
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        guardM.anim.SetBool("isWalking", false);
    }



    IEnumerator AssistAnim(float fWaitSecond)
    {
        yield return new WaitForSeconds(fWaitSecond);

        guardM.anim.SetBool("isWalking", true);
        guardM.nav.isStopped = false;

        bAnimEnd = true;
    }
   


}
