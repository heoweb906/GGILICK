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

        // �÷��̾� ��ġ�� ���ų� Obstacle�� �ִ� ��� ������ ���ư�
        if (guardM.area.playerPosition == null || guardM.IsObstacleBetween())
        {
            ReturnHome();
        }
        else
        {
            // ��ֹ��� ���� �÷��̾� ��ġ�� �ִ� ��� �߰� ���·� ��ȯ
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

        // ���� ������ ���
        if (Vector3.Distance(guardM.transform.position, guardM.GetHomeTransform()) <= 0.1f)
        {
            float rotationTimer = 0;
            rotationTimer += Time.deltaTime;

            guardM.nav.isStopped = true;
            guardM.anim.SetBool("isWalking", false);

            // ȸ�� ó��
            Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
            guardM.transform.rotation = Quaternion.Slerp(guardM.transform.rotation, targetRotation, rotationTimer / 1f);

            // ȸ���� �Ϸ�Ǹ� ReadyState�� ��ȯ
            if (rotationTimer >= 1f)
            {
                guardM.transform.rotation = targetRotation;
                bAnimEnd = false;
                machine.OnStateChange(machine.ReadyState);
            }
        }
    }


}
