using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_PlayerCheck : MonoBehaviour
{
    public bool bPlayerNearby = false;

<<<<<<< Updated upstream
    // 트리거에 다른 Collider가 들어올 때 호출됨
=======
    // Ʈ���ſ� �ٸ� Collider�� ���� �� ȣ���
>>>>>>> Stashed changes
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = true;
            Debug.Log("Player entered the area.");
        }
    }

<<<<<<< Updated upstream
    // 트리거에서 다른 Collider가 나갈 때 호출됨
=======
    // Ʈ���ſ��� �ٸ� Collider�� ���� �� ȣ���
>>>>>>> Stashed changes
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = false;
            Debug.Log("Player left the area.");
        }
    }
}
