using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulbBug : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent nav;
    public BulbBugStateMachine machine;
    public Rigidbody rigid;
    public Collider collider;

    public LayerMask layer; // gameObject.layer = LayerMask.NameToLayer("Player");
    public CarriedObject carriedObj;

    [Header("��ȸ ���� ��ġ��")]
    public float timeBetweenDestinations = 3f;  // ������ ���̿� ����� �ð�
    public float patrolRange = 3f;  // ��ȸ ���� (���� ��ġ ����)
    public float waitTime = 1f; // ������ ���� �� ��� �ð�
    public bool isWaiting = false;
    public float waitTimer = 0f; // ��� �ð� ī��Ʈ

    [Header("�÷��̾� ���� Ray")]
    public GameObject CheckingAreaObj_1;     // ū Ȯ�� ����
    public GameObject CheckingAreaObj_2;     // ���� Ȯ�� ����
    public PlayerCheckArea CheckingArea_1;
    public PlayerCheckArea CheckingArea_2;

    [Header("�׽�Ʈ��")]
    public GameObject LightObj;



    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        carriedObj = GetComponent<CarriedObject>();
        CheckingArea_1 = CheckingAreaObj_1.GetComponent<PlayerCheckArea>();
        CheckingArea_2 = CheckingAreaObj_2.GetComponent<PlayerCheckArea>();


        Init();
    }
    private void Init()
    {
        machine = new BulbBugStateMachine(this);
    }



    private void Update()
    {
        machine?.OnStateUpdate();

    }

    private void FixedUpdate()
    {
        machine?.OnStateFixedUpdate();
    }



    



}
