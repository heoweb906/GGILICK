using UnityEngine;
public class P_GroundState : PlayerMovementState
{
    public P_GroundState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GroundParameterHash);
        player.moveLerpSpeed = player.playerMoveLerpSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GroundParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (GetCurDirection() != Vector3.zero && (machine.CheckCurrentState(machine.IdleState) || machine.CheckCurrentState(machine.SoftLandingState) 
            || machine.CheckCurrentState(machine.SoftStopState)|| machine.CheckCurrentState(machine.HardStopState)
            || machine.CheckCurrentState(machine.RunningState) || machine.CheckCurrentState(machine.WalkingState)))
        {
            if(player.isRun)
                machine.OnStateChange(machine.RunningState);
            else
                machine.OnStateChange(machine.WalkingState);
        }
        else if (GetCurDirection() == Vector3.zero)
        {
            if(machine.CheckCurrentState(machine.WalkingState))
                machine.OnStateChange(machine.SoftStopState);
            else if(machine.CheckCurrentState(machine.RunningState) || machine.CheckCurrentState(machine.MoveLandingState))
                machine.OnStateChange(machine.HardStopState);

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (GetCurDirection() == Vector3.zero)
            {
                machine.OnStateChange(machine.JumpStartIdleState);
            }
            else
            {
                machine.OnStateChange(machine.JumpStartMoveState);
            }
        }

        if (Input.GetButtonDown("Fire1")) // 좌클릭
        {
            FindClosestClockWorkObject();
        }

        if (Input.GetButton("Fire1") && player.closestClockWork != null) // 좌클릭을 누르고 있는 동안
        {
            player.isGoToTarget = true;
            player.targetPos = player.closestClockWork.transform.position + new Vector3(-1.5f, 0, 0);
            if(Vector3.Distance(new Vector3(player.targetPos.x, 0, player.targetPos.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 0.1f)
            {
                machine.OnStateChange(machine.InteractionState);
            }

            //player.closestClockWork.ChargingBattery(); // OnClockWork 함수 호출
        }

        if (Input.GetButtonUp("Fire1")) // 마우스를 떼면
        {
            if (player.closestClockWork != null)
            {
                player.closestClockWork = null; // 가장 가까운 ClockWork 참조 초기화
                player.isGoToTarget = false;
            }
        }
    }

    public override void SetDirection()
    {
        if(!player.isGoToTarget)
            base.SetDirection();
        else
        {
            player.curDirection = player.targetPos - player.transform.position;
        }
    }

    public void FindClosestClockWorkObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, player.detectionRadius);
        player.closestClockWork = null; // 이전 참조 초기화
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            // ClockWork 스크립트가 있는지 확인
            ClockWork clockWork = collider.GetComponent<ClockWork>();
            if (clockWork != null && !clockWork.clockBattery.bDoing)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.closestClockWork = clockWork; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        // 부모 오브젝트의 모든 자식에서도 ClockWork 스크립트를 검사
        foreach (Transform child in player.transform)
        {
            ClockWork clockWork = child.GetComponent<ClockWork>();
            if (clockWork != null && !clockWork.clockBattery.bDoing)
            {
                float distance = Vector3.Distance(player.transform.position, child.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.closestClockWork = clockWork; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        if (player.closestClockWork != null)
        {
            Debug.Log("가장 가까운 ClockWork 오브젝트: " + player.closestClockWork.gameObject.name);
            // 여기에서 추가적인 로직을 구현할 수 있습니다.
        }
        else
        {
            Debug.Log("ClockWork 오브젝트를 찾을 수 없습니다.");
        }
    }

    
}


