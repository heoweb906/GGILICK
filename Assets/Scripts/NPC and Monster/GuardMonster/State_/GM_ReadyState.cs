using UnityEngine;
public class GM_ReadyState : GuardMState
{
    public GM_ReadyState(GuardM guardM, GuardMStateMachine machine) : base(guardM, machine) { }


    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(guardM.area.isPlayerInArea && guardM.area.playerPosition != null)
        {
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
    }


    

}
