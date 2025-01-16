using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_PlayerCheck : MonoBehaviour
{
    public bool bPlayerNearby = false;

    // Ʈ���ſ� �ٸ� Collider�� ���� �� ȣ���
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = true;
            Debug.Log("Player entered the area.");
        }
    }

    // Ʈ���ſ��� �ٸ� Collider�� ���� �� ȣ���
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = false;
            Debug.Log("Player left the area.");
        }
    }
}
