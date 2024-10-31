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

    [Header("상호작용 오브젝트")]
    public InteractableObject curInteractableObject;

    [Header("태엽 오브젝트")]
    public float detectionRadius = 10f; // 탐지 반경
    public float clockWorkInteractionDistance = 1f; // 상호작용 거리
    public ClockWork curClockWork; // 가장 가까운 ClockWork 오브젝트
    public Vector3 targetPos; // 가장 가까운 ClockWork 오브젝트
    public bool isGoToTarget;

    [Header("물건 옮기기")]
    public CarriedObject curCarriedObject;
    public Transform CarriedObjectPos;
    public float carriedObjectInteractionDistance = 1f; // 상호작용 거리
    public bool isCarryObject;

    [Header("Climb")]
    public float cliffCheckRayDistance = 1f; // 탐지 반경
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
        // 탐지 반경을 시각적으로 표시
        Gizmos.color = Color.green; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere로 탐지 범위 그리기
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