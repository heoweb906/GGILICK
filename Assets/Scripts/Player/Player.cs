using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine machine;

    [Header("ThisComponent")]
    [SerializeField]
    public Rigidbody rigid;
    [Header("Animation")]
    public Animator playerAnim;
    [field: SerializeField] public PlayerAnimationData playerAnimationData { get; private set; }

    [Header("이동속도")]
    [SerializeField, Range(0, 60)]
    public float playerWalkSpeed;
    [SerializeField, Range(0, 60)]
    public float playerRunSpeed;
    [SerializeField, Range(0, 60)]
    public float playerMoveLerpSpeed;
    [SerializeField, Range(0, 60)]
    public float playerMoveLerpSpeedOnJump;
    [SerializeField, Range(0, 60)]
    public float playerRotateLerpSpeed;
    [Header("점프")]
    [SerializeField, Range(0, 20)]
    public float firstJumpPower;
    [SerializeField, Range(0, 100)]
    public float fallingPower;
    [SerializeField, Range(0, 20)]
    public float maxFallingSpeed;

    [HideInInspector]
    public float moveLerpSpeed;

    
    [Header("경사로")]
    [SerializeField, Range(0, 2)]
    public float rayDistance = 1f;
    public RaycastHit slopeHit;
    public int groundLayer;
    [SerializeField, Range(0, 90)]
    public int maxSlopeAngle;
    [SerializeField]
    public GameObject raycastOrigin;


    [Header("Platform")]
    [Range(0, 60)] public float platformVelocityLerp;
    [HideInInspector] public Vector3 platformVelocity;

    [Header("Foot IK")]
    [SerializeField, Range(0, 1f)]
    public float distanceGround;


    public float playerMoveSpeed;
    public bool isRun = false;

    public Vector3 curDirection = Vector3.zero;
    public Vector3 preDirection = Vector3.zero;

    private void Awake()
    {
        playerAnimationData.Initialize();
        Init();
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    private void Init()
    {
        machine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        groundLayer = ~(1 << LayerMask.NameToLayer("Player"));
    }

    private void Update()
    {
        machine?.OnStateUpdate();
    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (playerAnim)
    //    {

    //        // Left Foot
    //        // Position 과 Rotation weight 설정
    //        playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //        playerAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.5f);

    //        ///<summary>
    //        /// GetIKPosition 
    //        ///   => IK를 하려는 객체의 위치 값 ( 아래에선 아바타에서 LeftFoot에 해당하는 객체의 위치 값 )
    //        /// Vector3.up을 더한 이유 
    //        ///   => origin의 위치를 위로 올려 바닥에 겹쳐 바닥을 인식 못하는 걸 방지하기 위해
    //        ///      (LeftFoot이 발목 정도에 있기 때문에 발바닥과 어느 정도 거리가 있고, Vector3.up을 더해주지 않으면 발목 기준으로 처리가 되어 발 일부가 바닥에 들어간다.)
    //        ///</summary>
    //        Ray leftRay = new Ray(playerAnim.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down);
    //        Debug.DrawRay(leftRay.origin, leftRay.direction * (distanceGround), Color.magenta);
    //        // distanceGround: LeftFoot에서 땅까지의 거리
    //        // +1을 해준 이유: Vector3.up을 해주었기 때문
    //        if (Physics.Raycast(leftRay, out RaycastHit leftHit, distanceGround, groundLayer))
    //        {

    //            // 걸을 수 있는 땅이라면
    //            if (leftHit.transform.tag == "WalkableGround")
    //            {
    //                Vector3 footPosition = leftHit.point;
    //                Debug.Log(footPosition);
    //                //footPosition.y += distanceGround;

    //                playerAnim.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
    //                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, leftHit.normal);
    //                playerAnim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(forward, leftHit.normal));
    //            }
    //        }

    //        // Right Foot
    //        playerAnim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
    //        playerAnim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0.5f);

    //        Ray rightRay = new Ray(playerAnim.GetIKPosition(AvatarIKGoal.RightFoot), Vector3.down);
    //        Debug.DrawRay(rightRay.origin, rightRay.direction * (distanceGround), Color.magenta);

    //        if (Physics.Raycast(rightRay, out RaycastHit rightHit, distanceGround, groundLayer))
    //        {
    //            if (rightHit.transform.tag == "WalkableGround")
    //            {
    //                Vector3 footPosition = rightHit.point;
    //                //footPosition.y += distanceGround;

    //                playerAnim.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
    //                Vector3 forward = Vector3.ProjectOnPlane(transform.forward, rightHit.normal);
    //                playerAnim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(forward, rightHit.normal));
    //            }
    //        }
    //    }
    //}

    // 플레이어 빠르게 떨어지도록

    //public void ControllGravity()
    //{
    //    if (rigid.velocity.y < 3 && groundList.Count == 0)
    //    {
    //        if (rigid.velocity.y > -maxFallingSpeed)
    //            rigid.velocity -= new Vector3(0, fallingPower * Time.fixedDeltaTime, 0);
    //        else rigid.velocity = new Vector3(rigid.velocity.x, -maxFallingSpeed, rigid.velocity.z);
    //    }

    //}

    //public Vector3 platformVelocity;
    //public float platformVelocityLerp;

    //private float playerMoveSpeed;

    //private float _horizontal = 0;
    //private float _vertical = 0;
    //private bool isRun = false;

    //public Vector3 GetCurDirection() { return new Vector3(_horizontal,0,_vertical); }
    //// 플레이어 기본 이동
    //public void PlayerVelocityControll()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q))
    //        machine.OnStateChange(machine.IdleState);

    //    //Debug.Log("State: " + machine.CurrentState.GetType().Name);
    //    _horizontal = Input.GetAxisRaw("Horizontal");
    //    _vertical = Input.GetAxisRaw("Vertical");
    //    isRun = Input.GetKey(KeyCode.LeftShift);

    //    curDirection = new Vector3(_horizontal, 0, _vertical);

    //    if (machine.CheckCurrentState(machine.WalkingState))
    //        playerMoveSpeed = playerWalkSpeed;
    //    else if (machine.CheckCurrentState(machine.RunningState))
    //        playerMoveSpeed = playerRunSpeed;

    //    // Rotation 이동 방향으로 조절
    //    if (curDirection != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(curDirection);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateLerpSpeed * Time.fixedDeltaTime);
    //    }

    //    // Rigid의 속도 조절로 이동, 보간 사용
    //    curDirection = Vector3.Lerp(preDirection, curDirection, playerMoveLerpSpeed * Time.fixedDeltaTime);

    //    Vector3 velocity = CalculateNextFrameGroundAngle(playerMoveSpeed) < maxSlopeAngle ? curDirection : Vector3.zero;
    //    Vector3 gravity;


    //    if (IsOnSlope()) // 경사로라면 경사에 맞춰서 방향값 세팅
    //    {
    //        velocity = AdjustDirectionToSlope(curDirection);
    //        gravity = Vector3.zero;
    //        rigid.useGravity = false;
    //    }
    //    else
    //    {
    //        gravity = new Vector3(0, rigid.velocity.y, 0);
    //        rigid.useGravity = true;
    //    }

    //    rigid.velocity = velocity * playerMoveSpeed + gravity;

    //    if (curMovingPlatform != null)
    //    {
    //        platformVelocity = curMovingPlatform.GetPlatformVelocity();
    //    }
    //    else
    //    {
    //        if (groundList.Count == 0)
    //        {
    //            platformVelocity = Vector3.Lerp(platformVelocity, Vector3.zero, platformVelocityLerp);

    //        }
    //        else
    //            platformVelocity = Vector3.zero;
    //    }
    //    rigid.velocity += platformVelocity;
    //    preDirection = curDirection;
    //}

    //// 현재 밟고 있는 땅 경사 체크
    //private bool IsOnSlope()
    //{
    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    Debug.DrawRay(ray.origin, Vector3.down * rayDistance, Color.red);
    //    if (Physics.Raycast(ray, out slopeHit, rayDistance, groundLayer))
    //    {
    //        var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //        return angle != 0f && angle < maxSlopeAngle && jumpCount == 0;
    //    }
    //    return false;
    //}

    //// 밟고 있는 땅 기준으로 방향 재설정
    //private Vector3 AdjustDirectionToSlope(Vector3 direction)
    //{
    //    return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    //}

    //private float CalculateNextFrameGroundAngle(float _moveSpeed)
    //{
    //    var nextFramePlayerPosition = raycastOrigin.transform.position + curDirection * _moveSpeed * Time.fixedDeltaTime;

    //    if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, 1f))
    //    {
    //        return Vector3.Angle(Vector3.up, hitInfo.normal);
    //    }
    //    return 0f;
    //}

    //public void PlayerJump()
    //{
    //    rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);
    //    rigid.AddForce(new Vector3(0, firstJumpPower, 0), ForceMode.VelocityChange);
    //}


    public void OnMovementStateAnimationEnterEvent()
    {
        machine.OnAnimationEnterEvent();
    }

    public void OnMovementStateAnimationExitEvent()
    {
        machine.OnAnimationExitEvent();
    }

    public void OnMovementStateAnimationTransitionEvent()
    {
        machine.OnAnimationTransitionEvent();
    }


    [SerializeField]
    public List<GameObject> groundList = new List<GameObject>();
    public MovingPlatform curMovingPlatform;
    private void OnTriggerEnter(Collider other)
    {
        machine.OnTriggerEnter(other);
    }


    private void OnTriggerExit(Collider other)
    {
        machine.OnTriggerExit(other);
    }
}
