using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulBug_SleepState : BulbBugState
{
    public BulBug_SleepState(BulbBug bulbBug, BulbBugStateMachine machine) : base(bulbBug, machine) { }

    private float elapsedTime = 0f;  // ��� �ð� ����

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("��� ���� ���� �Ϸ�");
        bulbBug.LightObj.SetActive(false);
        bulbBug.carriedObj.enabled = true;
        bulbBug.nav.enabled = false;
        bulbBug.rigid.velocity = Vector3.zero;
        bulbBug.rigid.isKinematic = false;

        bulbBug.gameObject.layer = LayerMask.NameToLayer("Interactable");
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!bulbBug.CheckingArea_2.isPlayerInArea && bulbBug.carriedObj.canInteract)
        {
            
            elapsedTime += Time.deltaTime;  // �� ������ ��� �ð� �߰�

            if (elapsedTime >= 1f)
            {
                Vector3 rotation = bulbBug.gameObject.transform.rotation.eulerAngles;

                if (Mathf.Abs(Mathf.DeltaAngle(rotation.x, 0)) <= 4f &&
                   Mathf.Abs(Mathf.DeltaAngle(rotation.y, 0)) <= 4f &&
                   Mathf.Abs(Mathf.DeltaAngle(rotation.z, 0)) <= 4f)
                {
                    machine.OnStateChange(machine.WanderingState);
                }
                else
                {
                    machine.OnStateChange(machine.StandUpState);
                }
            }
        }

        if(bulbBug.CheckingArea_2.isPlayerInArea) elapsedTime = 0f;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }


    public override void OnExit()
    {
        base.OnExit();

        bulbBug.LightObj.SetActive(true);
        bulbBug.carriedObj.enabled = false;
        
    }

}
