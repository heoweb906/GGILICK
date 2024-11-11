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

        if (!bAnimEnd) return;

        // 플레이어 위치가 없거나 Obstacle이 있는 경우 집으로 돌아감
        if (guardM.area.playerPosition == null || guardM.IsObstacleBetween())
        {
            ReturnHome();
        }
        else
        {
            // 장애물이 없고 플레이어 위치가 있는 경우 추격 상태로 전환
            bAnimEnd = false;
            machine.OnStateChange(machine.ChaseState);
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



    private void ReturnHome()
    {
        guardM.nav.SetDestination(guardM.GetHomeTransform());

        // 집에 도착한 경우
        if (Vector3.Distance(guardM.transform.position, guardM.GetHomeTransform()) <= 0.1f)
        {
            float rotationTimer = 0;
            rotationTimer += Time.deltaTime;

            guardM.nav.isStopped = true;
            guardM.anim.SetBool("isWalking", false);

            // 회전 처리
            Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
            guardM.transform.rotation = Quaternion.Slerp(guardM.transform.rotation, targetRotation, rotationTimer / 1f);

            // 회전이 완료되면 ReadyState로 전환
            if (rotationTimer >= 1f)
            {
                guardM.transform.rotation = targetRotation;
                bAnimEnd = false;
                machine.OnStateChange(machine.ReadyState);
            }
        }
    }


}
