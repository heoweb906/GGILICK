using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public GameObject leftDoor; 
    public GameObject rightDoor;

    public Transform position_target_LeftDoor;
    public Transform position_target_RightDoor;
    public Transform position_middle_LeftDoor;
    public Transform position_middle_RightDoor;


    public Vector3 originalPosition_LeftDoor;     // ���� ���� ���� ��ġ
    public Vector3 originalPosition_RightDoor;    // ������ ���� ���� ��ġ

    void Start()
    {
        originalPosition_LeftDoor = leftDoor.transform.position;
        originalPosition_RightDoor = rightDoor.transform.position;

    }

  
}
