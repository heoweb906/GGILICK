using System.Collections;
using System.Collections.Generic;
<<<<<<< Updated upstream
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
=======
using UnityEngine;
>>>>>>> Stashed changes

public class StampMacine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

<<<<<<< Updated upstream
    [Header("서류 오브젝트")]
    public GameObject obj_Document;         // 커다랑 상태의 서류
    public GameObject obj_SmallDocument;    // 생성할 ColorObj
    public Transform transform_CreateDocument;

    [Header("스탬프 정보")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // 찍을 스탬프들
    public Transform transforom_stamp;  // 스탬프 찍을 위치
    private Queue<int> queueStamp = new Queue<int>(); // 생성된 스탬프 관리용 스택
    
=======
    [Header("������ ����")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // ���� ��������
    public Transform transforom_stamp;  // ������ ���� ��ġ

    private Stack<GameObject> stampStack = new Stack<GameObject>(); // ������ ������ ������ ����
>>>>>>> Stashed changes


    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(HitStamp());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
<<<<<<< Updated upstream

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        // 도장을 알맞은 모양으로 찍었다면
        if(IsQueueInOrder(queueStamp))
        {
            obj_Document.SetActive(false);
            GameObject instantiatedObject = Instantiate(obj_SmallDocument, transform_CreateDocument);
        }
    }

=======
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);


        Debug.Log(iStampNum);

    }
>>>>>>> Stashed changes


    private IEnumerator HitStamp()
    {
<<<<<<< Updated upstream
        // 배터리가 3보다 작다면
=======
        // ���͸��� 3���� �۴ٸ�
>>>>>>> Stashed changes
        if (fCurClockBattery < 3)
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
<<<<<<< Updated upstream
                yield return new WaitForSecondsRealtime(1.0f); // 1초 대기
            }
        }
        else // 배터리가 3 이상이라면
=======
                yield return new WaitForSecondsRealtime(1.0f); // 1�� ���
            }
        }
        else // ���͸��� 3 �̻��̶��
>>>>>>> Stashed changes
        {
            while (fCurClockBattery > 0)
            {
                fCurClockBattery -= 1;
<<<<<<< Updated upstream
                yield return new WaitForSecondsRealtime(1.0f); // 1초 대기
=======
                yield return new WaitForSecondsRealtime(1.0f); // 1�� ���
>>>>>>> Stashed changes
            }

            if (iStampNum > 0)
            {
<<<<<<< Updated upstream
                queueStamp.Enqueue(iStampNum);
                CreateStackedStamps();      // 도장 찍기
=======
                // Y�� ������ ���: ���ÿ� �ִ� ��� ���� ���� ����
                float yOffset = stampStack.Count * 0.01f;

                // ������ ������ ����
                GameObject newStamp = Instantiate(
                    Stamps[iStampNum - 1],
                    transforom_stamp.position + new Vector3(0, yOffset, 0),
                    Quaternion.Euler(90f, 0f, 0f) // X������ 90�� ȸ��
                );

                // �������� ���ÿ� �߰�
                stampStack.Push(newStamp);

                // �θ� ����
                newStamp.transform.SetParent(transforom_stamp);
>>>>>>> Stashed changes
            }
        }

        TurnOffObj();
        yield break;
    }
<<<<<<< Updated upstream
    // #. 도장 생성하기
    public void CreateStackedStamps()
    {
        // 이미 생성되어 있는 도장들을 지움
        foreach (Transform child in transforom_stamp)
        {
            Destroy(child.gameObject);  
        }


        // 자신 아래의 도장들을 지움
        Queue<int> tempQueue = new Queue<int>();
        while (queueStamp.Count > 0)
        {
            int element = queueStamp.Dequeue();
            if (element <= iStampNum) tempQueue.Enqueue(element);
        }
        // 원래 큐에 조건을 만족하는 요소만 다시 넣기
        while (tempQueue.Count > 0)
        {
            queueStamp.Enqueue(tempQueue.Dequeue());
        }



        if (queueStamp.Count > 0)
        {
            // 스택에서 꺼낸 요소를 저장할 리스트
            List<int> poppedElements = new List<int>();

            int count = queueStamp.Count;
            for (int i = 0; i < count; i++)
            {
                // 큐에서 Pop을 사용하여 요소를 꺼냄
                int stampIndex = queueStamp.Dequeue();  // Pop을 사용하여 큐에서 값을 제거하면서 꺼냄

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

            // 꺼낸 요소들을 다시 원래대로 스택에 복원
            foreach (int stampIndex in poppedElements)
            {
                queueStamp.Enqueue(stampIndex);
            }
        }
    }




    private bool IsQueueInOrder(Queue<int> queue)
    {
        // 올바른 순서를 미리 정의합니다.
        int[] correctOrder = { 1, 2, 3, 4 };

        // 요소 개수가 다르면 순서가 맞을 수 없습니다.
        if (queue.Count != correctOrder.Length)
            return false;

        // Queue를 배열로 변환하여 순서를 비교합니다.
        int[] queueArray = queue.ToArray();

        for (int i = 0; i < correctOrder.Length; i++)
        {
            if (queueArray[i] != correctOrder[i])
                return false;
        }

        return true;
    }
=======
>>>>>>> Stashed changes








<<<<<<< Updated upstream
    // #. IPartOwner 인터페이스
=======






    // #. IPartOwner �������̽�
>>>>>>> Stashed changes
    #region

    public void InsertOwnerFunc(GameObject stampParts, int index)
    {
        StampParts stampParts_ = stampParts.GetComponent<StampParts>();
<<<<<<< Updated upstream
        if (stampParts_ == null)
        {
            Debug.LogWarning("스탬프 파츠에 StampParts 컴포넌트가 없습니다.");
            return;
        }
=======
>>>>>>> Stashed changes
        iStampNum = stampParts_.iStampeNum;
    }

    public void RemoveOwnerFunc(int index)
    {
        iStampNum = 0;
    }



    #endregion
}
