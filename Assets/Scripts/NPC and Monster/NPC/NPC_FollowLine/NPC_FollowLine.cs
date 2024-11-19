using UnityEngine;
using UnityEngine.AI;

public class NPC_FollowLine : MonoBehaviour
{
    [Header("�ִϸ�����")]
    private Animator anim;

    public Transform[] checkPoints; // ��ǥ ���� �迭
    private NavMeshAgent agent; // NavMeshAgent ������Ʈ
    private int currentCheckPointIndex = 0; // ���� ��ǥ ���� �ε���
    private int iWalkAnimNum;

    private float pushBackForce = 2f; 
    private bool isPushedBack = false; 

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        iWalkAnimNum = Random.Range(1, 2); // �ɾ�ٴϴ� ������ �������� �� ����

        if (checkPoints.Length > 0) MoveToNextCheckPoint();
    }

    void Update()
    {
        if (isPushedBack || checkPoints != null)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentCheckPointIndex++;
            if (currentCheckPointIndex < checkPoints.Length)
            {
                MoveToNextCheckPoint();
            }
            else Destroy(gameObject);
        }
        else
        {
            anim.SetInteger("Walk_Num", iWalkAnimNum); // �̵� �� �ִϸ��̼�
        }
    }

    private void MoveToNextCheckPoint()
    {
        agent.SetDestination(checkPoints[currentCheckPointIndex].position);
        anim.SetInteger("Walk_Num", iWalkAnimNum); // �̵� �ִϸ��̼� ����
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPushedBack)
        {
            StopAllCoroutines();

            // NPC�� �о��
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(PushBack(pushDirection));
        }
    }

    private System.Collections.IEnumerator PushBack(Vector3 direction)
    {
        isPushedBack = true;
        agent.isStopped = true; // �̵� ����
        anim.SetTrigger("doTakeHitSholder"); // �ִϸ��̼� Ʈ����

        float pushDuration = 4.0f; // �и��� �ð�
        float elapsed = 0f;

        while (elapsed < pushDuration)
        {
            elapsed += Time.deltaTime;

            if (elapsed < 0.25f) transform.position += direction * pushBackForce * Time.deltaTime;
            else transform.position += direction * 0f * Time.deltaTime;

            yield return null;
        }

        isPushedBack = false;
        agent.isStopped = false; // �̵� �簳
    }








}
