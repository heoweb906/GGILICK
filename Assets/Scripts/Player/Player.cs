using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine machine;

    [Header("ThisComponent")]
    [SerializeField]
    public Rigidbody rigid;
    public List<BoxCollider> playerCollider;
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
    public int cliffLayer;
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
    public InteractableObject curInteractableObject;

    [Header("�¿� ������Ʈ")]
    public float detectionRadius = 10f; // Ž�� �ݰ�
    public float clockWorkInteractionDistance = 1f; // ��ȣ�ۿ� �Ÿ�
    public ClockWork curClockWork; // ���� ����� ClockWork ������Ʈ
    public Vector3 targetPos; // ���� ����� ClockWork ������Ʈ
    public bool isGoToTarget;

    [Header("���� �ű��")]
    public CarriedObject curCarriedObject;
    public Transform CarriedObjectPos;
    public float carriedObjectInteractionDistance = 1f; // ��ȣ�ۿ� �Ÿ�
    public bool isCarryObject;

    [Header("Climb")]
    public float cliffCheckRayDistance = 1f; // Ž�� �ݰ�
    public RaycastHit cliffRayHit;
    [HideInInspector]
    public Vector3 hangingPos;
    public float hangingPosOffset_Front;
    public float hangingPosOffset_Height;


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
        cliffLayer = (1 << LayerMask.NameToLayer("Cliff"));
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

    public void SetColliderTrigger(bool _bool)
    {
        foreach (var item in playerCollider)
        {
            item.enabled = _bool;
        }
    }

    public void SetRootMotion()
    {
        StartCoroutine(C_SetRootMotion());
    }
    IEnumerator C_SetRootMotion()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.applyRootMotion = false;
    }

    public void SetPlayerPhysicsIgnore(Collider _col, bool _bool)
    {
        int layer1 = LayerMask.NameToLayer("Player");
        int layer2 = LayerMask.NameToLayer("Interactable");
        Physics.IgnoreLayerCollision(layer1, layer2, _bool);
    }
}