using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckArea : MonoBehaviour
{
    public bool isPlayerInArea = false; // �÷��̾ ���� ���� �ִ��� ����
    public Transform playerPosition; // �÷��̾��� ��ġ�� ������ ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = true;
            playerPosition = other.transform.root; // �÷��̾��� �ֻ��� �θ� ������Ʈ ��ġ ����
            Debug.Log("Player has entered the area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            playerPosition = null; // �÷��̾� ��ġ �ʱ�ȭ
            Debug.Log("Player has exited the area.");
        }
    }

    public bool IsPlayerInArea()
    {
        return isPlayerInArea;
    }

    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
}
