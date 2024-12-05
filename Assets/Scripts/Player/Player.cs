using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public bool stateChangeDebug;

    [Header("���� ���� �̵�")]
    public bool isWorldAxis;
    [Range(0, 360)]
    public float yAxis;

    public PlayerStateMachine machine;

    public Transform camTransform;

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
    public float playerDefaultRotateLerpSpeed;
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
    public float rotateLerpSpeed;
    public bool isRun = false;

    public Vector3 curDirection = Vector3.zero;
    public Vector3 preDirection = Vector3.zero;

    [Header("��ȣ�ۿ� ������Ʈ")]
    public InteractableObject curInteractableObject;

    [Header("�¿� ������Ʈ")]
    public float detectionRadius = 10f; // Ž�� �ݰ�
    public float clockWorkInteractionDistance_Wall = 1f; // ��ȣ�ۿ� �Ÿ�
    public float clockWorkInteractionDistance_Floor = 1f; // ��ȣ�ۿ� �Ÿ�
    public ClockWork curClockWork; // ���� ����� ClockWork ������Ʈ
    public Vector3 targetPos; // ���� ����� ClockWork ������Ʈ
    public bool isGoToTarget;

    [Header("���� �ű��")]
    public CarriedObject curCarriedObject;
    public Transform CarriedObjectPos;
    public float carriedObjectInteractionDistance = 1f; // ��ȣ�ۿ� �Ÿ�
    public bool isCarryObject;
    [Range(0, 50)]
    public float throwPower;

    [Header("���� �б�")]
    public GrabObject curGrabObject;
    public Transform grabPos;
    public float grabObjectInteractionDistance = 1f; // ��ȣ�ۿ� �Ÿ�
    [SerializeField, Range(0, 60)]
    public float playerGrapRotateLerpSpeed;
    [SerializeField, Range(0, 60)]
    public float playerGrapMoveSpeed;

    [Header("Climb")]
    public float cliffCheckRayDistance = 1f; // Ž�� �ݰ�
    public RaycastHit cliffRayHit;
    [HideInInspector]
    public Vector3 hangingPos;
    public float hangingPosOffset_Front;
    public float hangingPosOffset_Height;


    [Header("���� ��� IK")]
    public bool isHandIK = false;


    private void Awake()
    {
        playerAnimationData.Initialize();
        Init();
        rigid = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        isGoToTarget = false;
        Application.targetFrameRate = 180;
        isHandIK = false;
        camTransform = FindObjectOfType<Camera>().transform;
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


    public Transform tf_L_UpperArm;
    public Transform tf_L_Hand;
    public Transform tf_R_UpperArm;
    public Transform tf_R_Hand;
    public int _x;
    float angle = 0;

    public void LateUpdate()
    {
        if (!isHandIK)
            return;

        if (Input.GetKey(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                _x--;
            else
                _x++;
        }

        BoxCollider _col = curCarriedObject.GetComponent<BoxCollider>();
        Vector3 boxScale = Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale);
        Vector3 leftTargetPos3D = curCarriedObject.transform.position - transform.right * boxScale.x;
        Vector2 leftTargetPos = new Vector2(leftTargetPos3D.x, leftTargetPos3D.z);

        Vector2 _leftUpperArm = new Vector2(tf_L_UpperArm.transform.position.x, tf_L_UpperArm.transform.position.z);
        Vector2 _leftHand = new Vector2(tf_L_Hand.transform.position.x, tf_L_Hand.transform.position.z);

        Vector2 upperArmToHand = _leftHand - _leftUpperArm;
        Vector2 upperArmToTarget = leftTargetPos - _leftUpperArm;

        float dotProduct = Vector2.Dot(upperArmToHand.normalized, upperArmToTarget.normalized);


        float targetAngle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg + _x;

        
        DOTween.To(() => angle, x => angle = x, targetAngle, 0.6f);

        tf_L_UpperArm.eulerAngles = tf_L_UpperArm.eulerAngles + new Vector3(0, angle, 0);

        tf_R_UpperArm.eulerAngles = tf_R_UpperArm.eulerAngles - new Vector3(0, angle, 0);
    }
    public float carryWeight = 1;

    public void SetCarryWeight()
    {
        carryWeight = 1;
        DOTween.To(() => carryWeight, x => carryWeight = x, 0, 0.3f);
    }

    public void SetHandIKAngle()
    {

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
        playerAnim.updateMode = AnimatorUpdateMode.Normal;
    }

    public void SetPlayerPhysicsIgnore(Collider _col, bool _bool)
    {
        int layer1 = LayerMask.NameToLayer("Player");
        int layer2 = LayerMask.NameToLayer("Interactable");
        Physics.IgnoreLayerCollision(layer1, layer2, _bool);
    }
}