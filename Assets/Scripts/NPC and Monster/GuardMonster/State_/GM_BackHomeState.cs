using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_BackHomeState : GuardMState
{
    public GM_BackHomeState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }

    bool bAnimEnd;

    public override void OnEnter()
    {
        base.OnEnter();

        guardM.StartGuardCoroutine(AssistAnim(2f));
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bAnimEnd)
        {
            if (guardM.area.playerPosition != null)
            {
                bAnimEnd = false;
                machine.OnStateChange(machine.ChaseState);
            }
            else
            {
                
                guardM.nav.SetDestination(guardM.transformHome);

                if (Vector3.Distance(guardM.transform.position, guardM.transformHome) <= 0.1f)
                {
                    float rotationTimer = 0;

                    rotationTimer += Time.deltaTime;

                    guardM.nav.isStopped = true;
                    guardM.anim.SetBool("isWalking", false);

                    // 목표는 'transformHome'을 기준으로 회전
                    Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
                    guardM.transform.rotation = Quaternion.Slerp(guardM.transform.rotation, targetRotation, rotationTimer / 1f);


                    if (rotationTimer >= 1f)
                    {
                        guardM.transform.rotation = targetRotation;  

                        bAnimEnd = false;
                        machine.OnStateChange(machine.ReadyState); 
                    }
                }
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
