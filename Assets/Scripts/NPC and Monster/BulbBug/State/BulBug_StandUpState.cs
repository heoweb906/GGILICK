using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulBug_StandUpState : BulbBugState
{
    public BulBug_StandUpState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float duration = 3f;
    private Tweener rotateTweener;

    public override void OnEnter()
    {
        base.OnEnter();

        bulbBug.rigid.isKinematic = true;

        rotateTweener = bulbBug.transform.DORotate(Vector3.zero, duration, RotateMode.FastBeyond360)
           .OnComplete(OnRotationComplete); // �ִϸ��̼��� ������ �� ȣ��� �޼��� ����

    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bulbBug.CheckingArea_2.isPlayerInArea)
        {
            // ȸ�� �ִϸ��̼��� ����
            if (rotateTweener != null && rotateTweener.IsPlaying())
            {
                rotateTweener.Kill(); // �ִϸ��̼� ����

                machine.OnStateChange(machine.SleepState); 
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
    }

    void OnRotationComplete()
    {
        machine.OnStateChange(machine.WanderingState);
    }

}
