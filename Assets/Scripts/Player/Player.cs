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

    [Header("�̵��ӵ�")]
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
    [Header("����")]
    [SerializeField, Range(0, 20)]
    public float firstJumpPower;
    [SerializeField, Range(0, 100)]
    public float fallingPower;
    [SerializeField, Range(0, 20)]
    public float maxFallingSpeed;

    [HideInInspector]
    public float moveLerpSpeed;

    
    [Header("����")]
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

    [Header("��ȣ�ۿ� ������Ʈ")]
    public float detectionRadius = 10f; // Ž�� �ݰ�
    public ClockWork closestClockWork; // ���� ����� ClockWork ������Ʈ
    public Vector3 targetPos; // ���� ����� ClockWork ������Ʈ
    public bool isGoToTarget;

    private void Awake()
    {
        playerAnimationData.Initialize();
        Init();
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        isGoToTarget = false;
        Application.targetFrameRate = 180;
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

    ///////////////////
    private void OnDrawGizmos()
    {
        // Ž�� �ݰ��� �ð������� ǥ��
        Gizmos.color = Color.green; // ����� ���� ����
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere�� Ž�� ���� �׸���
    }
}
