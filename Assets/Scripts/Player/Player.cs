using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool stateChangeDebug;


    public PlayerStateMachine machine;

    public Transform camTransform;

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
    public float playerDefaultRotateLerpSpeed;
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
    public float rotateLerpSpeed;
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
    [Range(0,50)]
    public float throwPower;

    [Header("물건 밀기")]
    public GrabObject curGrabObject;
    public Transform grabPos;
    public float grabObjectInteractionDistance = 1f; // 상호작용 거리
    [SerializeField, Range(0, 60)]
    public float playerGrapRotateLerpSpeed;
    [SerializeField, Range(0, 60)]
    public float playerGrapMoveSpeed;

    [Header("Climb")]
    public float cliffCheckRayDistance = 1f; // 탐지 반경
    public RaycastHit cliffRayHit;
    [HideInInspector]
    public Vector3 hangingPos;
    public float hangingPosOffset_Front;
    public float hangingPosOffset_Height;

    [Header("물건 잡기 IK")]
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
        // 탐지 반경을 시각적으로 표시
        Gizmos.color = Color.green; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // WireSphere로 탐지 범위 그리기
    }


    public Transform tf_L_UpperArm;
    public Transform tf_L_Hand;
    public Transform tf_R_UpperArm;
    public Transform tf_R_Hand;
    public int _x;
    int _y;
    int _z;
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
        //else if (Input.GetKey(KeyCode.X))
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //        _y--;
        //    else
        //        _y++;
        //}
        //else if (Input.GetKey(KeyCode.C))
        //{
        //    if (Input.GetKey(KeyCode.LeftShift))
        //        _z--;
        //    else
        //        _z++;
        //}
        ////leftArm.localEulerAngles = new Vector3(3+_x,-14 + _y,7+ _z);
        ///

        BoxCollider _col = curCarriedObject.GetComponent<BoxCollider>();
        Vector3 boxScale = Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale);
        Vector3 leftTargetPos3D = curCarriedObject.transform.position - transform.right * boxScale.x;
        Vector2 leftTargetPos = new Vector2(leftTargetPos3D.x, leftTargetPos3D.z);

        Vector2 _leftUpperArm = new Vector2(tf_L_UpperArm.transform.position.x, tf_L_UpperArm.transform.position.z);
        Vector2 _leftHand = new Vector2(tf_L_Hand.transform.position.x, tf_L_Hand.transform.position.z);

        Vector2 upperArmToHand = _leftHand - _leftUpperArm;
        Vector2 upperArmToTarget = leftTargetPos - _leftUpperArm;

        float dotProduct = Vector2.Dot(upperArmToHand.normalized, upperArmToTarget.normalized);


        //float magnitudeUpperArmToHand = upperArmToHand.magnitude;
        //float magnitudeUpperArmToTarget = upperArmToTarget.magnitude;
        //float angle = Mathf.Acos(dotProduct / (magnitudeUpperArmToHand * magnitudeUpperArmToTarget)) * Mathf.Rad2Deg;


        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

        tf_L_UpperArm.eulerAngles = tf_L_UpperArm.eulerAngles + new Vector3(0, angle + _x, 0);


        //Vector2 rightTargetPos = new Vector2(curCarriedObject.transform.position.x * transform.right.x - boxScale.y, curCarriedObject.transform.position.z * transform.forward.z);

        //Vector2 _RightUpperArm = new Vector2(tf_R_UpperArm.transform.position.x, tf_R_UpperArm.transform.position.z);
        //Vector2 _RightHand = new Vector2(tf_R_Hand.transform.position.x, tf_R_Hand.transform.position.z);

        //upperArmToHand = _RightHand - _RightUpperArm;
        //upperArmToTarget = _RightHand - rightTargetPos;

        //dotProduct = Vector2.Dot(upperArmToHand, upperArmToTarget);
        //magnitudeUpperArmToHand = upperArmToHand.magnitude;
        //magnitudeUpperArmToTarget = upperArmToTarget.magnitude;

        //angle = Mathf.Acos(dotProduct / (magnitudeUpperArmToHand * magnitudeUpperArmToTarget)) * Mathf.Rad2Deg;

        tf_R_UpperArm.eulerAngles = tf_R_UpperArm.eulerAngles - new Vector3(0, angle + _x, 0);
    }

    //public void OnAnimatorIK(int layerIndex)
    //{
    //    if (!isHandIK)
    //        return;
    //    playerAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //    playerAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

    //    BoxCollider _col = curCarriedObject.GetComponent<BoxCollider>();
    //    Vector3 leftHandPos = curCarriedObject.transform.position - Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale);
    //    Vector3 rightHandPos = curCarriedObject.transform.position + Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale);
    //    Debug.Log(leftHandPos);

    //    playerAnim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
    //    playerAnim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
    //}

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