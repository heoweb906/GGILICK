using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine machine;

    [Header("ThisComponent")]
    [SerializeField]
    private Rigidbody playerRigid;
    [Header("Animation")]
    public Animator playerAnim;
    [field: SerializeField] public PlayerAnimationData playerAnimationData { get; private set; }

    [Header("�̵��ӵ�")]
    [SerializeField, Range(0, 60)]
    private float playerMoveSpeed;
    [SerializeField, Range(0, 60)]
    private float playerMoveLerpSpeed;
    [SerializeField, Range(0, 60)]
    private float playerRotateLerpSpeed;
    [Header("����")]
    [SerializeField, Range(0, 20)]
    private float firstJumpPower;
    [SerializeField, Range(0, 100)]
    private float fallingPower;
    [SerializeField, Range(0, 20)]
    private float maxFallingSpeed;

    private Vector3 curDirection = Vector3.zero;
    private Vector3 preDirection = Vector3.zero;

    private int jumpCount;

    [Header("����")]
    [SerializeField, Range(0, 2)]
    private float rayDistance = 1f;
    private RaycastHit slopeHit;
    private int groundLayer;
    [SerializeField, Range(0, 90)]
    private int maxSlopeAngle;
    [SerializeField]
    private GameObject raycastOrigin;

    [Header("Foot IK")]
    [SerializeField, Range(0, 1f)]
    private float distanceGround;

    private void Awake()
    {
        playerAnimationData.Initialize();
        Init();
        playerRigid = GetComponent<Rigidbody>();
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
    //        // Position �� Rotation weight ����
    //        playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //        playerAnim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0.5f);

    //        ///<summary>
    //        /// GetIKPosition 
    //        ///   => IK�� �Ϸ��� ��ü�� ��ġ �� ( �Ʒ����� �ƹ�Ÿ���� LeftFoot�� �ش��ϴ� ��ü�� ��ġ �� )
    //        /// Vector3.up�� ���� ���� 
    //        ///   => origin�� ��ġ�� ���� �÷� �ٴڿ� ���� �ٴ��� �ν� ���ϴ� �� �����ϱ� ����
    //        ///      (LeftFoot�� �߸� ������ �ֱ� ������ �߹ٴڰ� ��� ���� �Ÿ��� �ְ�, Vector3.up�� �������� ������ �߸� �������� ó���� �Ǿ� �� �Ϻΰ� �ٴڿ� ����.)
    //        ///</summary>
    //        Ray leftRay = new Ray(playerAnim.GetIKPosition(AvatarIKGoal.LeftFoot), Vector3.down);
    //        Debug.DrawRay(leftRay.origin, leftRay.direction * (distanceGround), Color.magenta);
    //        // distanceGround: LeftFoot���� �������� �Ÿ�
    //        // +1�� ���� ����: Vector3.up�� ���־��� ����
    //        if (Physics.Raycast(leftRay, out RaycastHit leftHit, distanceGround, groundLayer))
    //        {

    //            // ���� �� �ִ� ���̶��
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

    // �÷��̾� ������ ����������

    public void ControllGravity()
    {
        if (playerRigid.velocity.y < 3 && groundList.Count == 0)
        {
            if (playerRigid.velocity.y > -maxFallingSpeed)
                playerRigid.velocity -= new Vector3(0, fallingPower * Time.fixedDeltaTime, 0);
            else playerRigid.velocity = new Vector3(playerRigid.velocity.x, -maxFallingSpeed, playerRigid.velocity.z);
        }

    }

    public Vector3 platformVelocity;
    public float platformVelocityLerp;

    // �÷��̾� �⺻ �̵�
    public void PlayerWalk()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            machine.OnStateChange(machine.IdleState);

        //Debug.Log("State: " + machine.CurrentState.GetType().Name);
        float _horizontal = Input.GetAxisRaw("Horizontal");
        float _vertical = Input.GetAxisRaw("Vertical");

        curDirection = new Vector3(_horizontal, 0, _vertical);

        if(curDirection != Vector3.zero && (machine.CheckCurrentState(machine.IdleState)|| machine.CheckCurrentState(machine.SoftLandingState)))
        {
            machine.OnStateChange(machine.WalkStartState);
        }
        else if(curDirection == Vector3.zero && (machine.CheckCurrentState(machine.WalkingState) || machine.CheckCurrentState(machine.WalkStartState)))
        {
            machine.OnStateChange(machine.SoftStopState);
        }

        // Rotation �̵� �������� ����
        if (curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(curDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateLerpSpeed * Time.fixedDeltaTime);
        }

        // Rigid�� �ӵ� ������ �̵�, ���� ���
        curDirection = Vector3.Lerp(preDirection, curDirection, playerMoveLerpSpeed * Time.fixedDeltaTime);
        playerAnim.SetFloat("moveSpeed", curDirection.magnitude); // �ִϸ����� moveSpeed�� ����

        Vector3 velocity = CalculateNextFrameGroundAngle(playerMoveSpeed) < maxSlopeAngle ? curDirection : Vector3.zero;
        Vector3 gravity;


        if (IsOnSlope()) // ���ζ�� ��翡 ���缭 ���Ⱚ ����
        {
            velocity = AdjustDirectionToSlope(curDirection);
            gravity = Vector3.zero;
            playerRigid.useGravity = false;
        }
        else
        {
            gravity = new Vector3(0, playerRigid.velocity.y, 0);
            playerRigid.useGravity = true;
        }

        playerRigid.velocity = velocity * playerMoveSpeed + gravity;

        if (curMovingPlatform != null)
        {
            platformVelocity = curMovingPlatform.GetPlatformVelocity();
        }
        else
        {
            if (groundList.Count == 0)
            {
                platformVelocity = Vector3.Lerp(platformVelocity, Vector3.zero, platformVelocityLerp);

            }
            else
                platformVelocity = Vector3.zero;
        }
        playerRigid.velocity += platformVelocity;
        preDirection = curDirection;
    }

    // ���� ��� �ִ� �� ��� üũ
    private bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, Vector3.down * rayDistance, Color.red);
        if (Physics.Raycast(ray, out slopeHit, rayDistance, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle && jumpCount == 0;
        }
        return false;
    }

    // ��� �ִ� �� �������� ���� �缳��
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal);
    }

    private float CalculateNextFrameGroundAngle(float _moveSpeed)
    {
        var nextFramePlayerPosition = raycastOrigin.transform.position + curDirection * _moveSpeed * Time.fixedDeltaTime;

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, 1f))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    public void PlayerJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount == 0)
        {
            playerRigid.velocity = new Vector3(playerRigid.velocity.x, 0, playerRigid.velocity.z);
            playerRigid.AddForce(new Vector3(0, firstJumpPower, 0), ForceMode.VelocityChange);
            //jumpCount = 1;
            //playerAnim.SetInteger("jumpCount", jumpCount);
            machine.OnStateChange(machine.JumpStartState);
        }
    }


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
    private List<GameObject> groundList = new List<GameObject>();
    public MovingPlatform curMovingPlatform;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            groundList.Add(other.gameObject);
            jumpCount = 0;
            playerAnim.SetInteger("jumpCount", jumpCount);
            if(machine.CheckCurrentState(machine.JumpStartState) || machine.CheckCurrentState(machine.FallingState))
                machine.OnStateChange(machine.SoftLandingState);
            if (other.CompareTag("MovingPlatform"))
                curMovingPlatform = other.GetComponent<MovingPlatform>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //if (!other.CompareTag("Player") && playerRigid.velocity.y < 0)
        //{
        //    jumpCount = 0;
        //    playerAnim.SetInteger("jumpCount", jumpCount);
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (groundList.Contains(other.gameObject))
                groundList.Remove(other.gameObject);
            if (groundList.Count > 0) return;
            jumpCount = 1;
            playerAnim.SetInteger("jumpCount", jumpCount);

            if(!machine.CheckCurrentState(machine.JumpStartState))
                machine.OnStateChange(machine.FallingState);

            if (other.CompareTag("MovingPlatform"))
                curMovingPlatform = null;
        }
    }
}
