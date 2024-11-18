using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWayAssist : MonoBehaviour
{
    public static SubWayAssist Instance;

    [Header("�÷��̾� ��ġ üũ")]
    public bool bPlayerInSubway;   // ����ö ���� �Ϸ�
    public bool bPlayerTakeTrain;  // ���� ž�� �Ϸ�
    public bool bPlayerTeleport;   // ����ö�� 2���� ���� �Ϸ�

    public GameObject TrainObj;


    public int iCrowedRanNum;
   

    private void Awake()
    {
        Instance = this; 
    }


    public void LetsStartTrain()
    {
        bPlayerInSubway = true;
        Train train = TrainObj.GetComponent<Train>();
        train.StartTrain();
    }


}
