using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StampMacine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    [Header("���� ������Ʈ")]
    public GameObject obj_Document;         // Ŀ�ٶ� ������ ����
    public GameObject obj_SmallDocument;    // ������ ColorObj
    public Transform transform_CreateDocument;

    [Header("������ ����")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // ���� ��������
    public Transform transforom_stamp;  // ������ ���� ��ġ
    private Queue<int> queueStamp = new Queue<int>(); // ������ ������ ������ ����
    


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(HitStamp());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        // ������ �˸��� ������� ����ٸ�
        if(IsQueueInOrder(queueStamp))
        {
            obj_Document.SetActive(false);
            GameObject instantiatedObject = Instantiate(obj_SmallDocument, transform_CreateDocument);
        }
    }



    private IEnumerator HitStamp()
    {
        // ���͸��� 3���� �۴ٸ�
        if (fCurClockBattery < 3)
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
                yield return new WaitForSecondsRealtime(1.0f); // 1�� ���
            }
        }
        else // ���͸��� 3 �̻��̶��
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
                yield return new WaitForSecondsRealtime(1.0f); // 1�� ���
            }

            if (iStampNum > 0)
            {
                queueStamp.Enqueue(iStampNum);
                CreateStackedStamps();      // ���� ���
            }
        }

        TurnOffObj();
        yield break;
    }
    // #. ���� �����ϱ�
    public void CreateStackedStamps()
    {
        // �̹� �����Ǿ� �ִ� ������� ����
        foreach (Transform child in transforom_stamp)
        {
            Destroy(child.gameObject);  
        }


        // �ڽ� �Ʒ��� ������� ����
        Queue<int> tempQueue = new Queue<int>();
        while (queueStamp.Count > 0)
        {
            int element = queueStamp.Dequeue();
            if (element <= iStampNum) tempQueue.Enqueue(element);
        }
        // ���� ť�� ������ �����ϴ� ��Ҹ� �ٽ� �ֱ�
        while (tempQueue.Count > 0)
        {
            queueStamp.Enqueue(tempQueue.Dequeue());
        }



        if (queueStamp.Count > 0)
        {
            // ���ÿ��� ���� ��Ҹ� ������ ����Ʈ
            List<int> poppedElements = new List<int>();

            int count = queueStamp.Count;
            for (int i = 0; i < count; i++)
            {
                // ť���� Pop�� ����Ͽ� ��Ҹ� ����
                int stampIndex = queueStamp.Dequeue();  // Pop�� ����Ͽ� ť���� ���� �����ϸ鼭 ����

                poppedElements.Add(stampIndex);

                if (stampIndex >= 0)
                {
                    Vector3 spawnPosition = transforom_stamp.position + new Vector3(0, 0.01f * i, 0);
                    GameObject newStamp = Instantiate(Stamps[stampIndex - 1], spawnPosition, Quaternion.Euler(90, 0, 0));
                    newStamp.transform.SetParent(transforom_stamp);
                }
                else
                {
                    Debug.LogWarning("Invalid stamp index in the stack");
                }
            }

            // ���� ��ҵ��� �ٽ� ������� ���ÿ� ����
            foreach (int stampIndex in poppedElements)
            {
                queueStamp.Enqueue(stampIndex);
            }
        }
    }




    private bool IsQueueInOrder(Queue<int> queue)
    {
        // �ùٸ� ������ �̸� �����մϴ�.
        int[] correctOrder = { 1, 2, 3, 4 };

        // ��� ������ �ٸ��� ������ ���� �� �����ϴ�.
        if (queue.Count != correctOrder.Length)
            return false;

        // Queue�� �迭�� ��ȯ�Ͽ� ������ ���մϴ�.
        int[] queueArray = queue.ToArray();

        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (queueArray[i] != correctOrder[i])
                return false;
        }

        return true;
    }








    // #. IPartOwner �������̽�
    #region

    public void InsertOwnerFunc(GameObject stampParts, int index)
    {
        StampParts stampParts_ = stampParts.GetComponent<StampParts>();
        if (stampParts_ == null)
        {
            Debug.LogWarning("������ ������ StampParts ������Ʈ�� �����ϴ�.");
            return;
        }
        iStampNum = stampParts_.iStampeNum;
    }

    public void RemoveOwnerFunc(int index)
    {
        iStampNum = 0;
    }



    #endregion
}
