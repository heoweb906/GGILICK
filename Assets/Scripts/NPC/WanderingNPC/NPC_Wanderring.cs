using UnityEngine;
using UnityEngine.AI;

public class NPC_Wanderring : MonoBehaviour
{
    [Header("�ִϸ�����")]
    private Animator anim;

    public Transform[] checkPoints; // ��ǥ ���� �迭
    private NavMeshAgent agent; // NavMeshAgent ������Ʈ
    private int currentCheckPointIndex = 0; // ���� ��ǥ ���� �ε���
    private int iWalkAnimNum;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        iWalkAnimNum = Random.Range(1, 3);

        if (checkPoints.Length > 0) MoveToNextCheckPoint();
    }

    void Update()
    {
        // ���� ��ǥ ������ �����ߴ��� Ȯ��
        if (agent.pathPending)
            return;

        // ���� ��ǥ ������ ��������� ��
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // ���� ��ǥ �������� �̵�
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
}
