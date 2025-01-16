using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampMacine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;

    [Header("������ ����")]
    private int iStampNum = 0;
    public GameObject[] Stamps;         // ���� ��������
    public Transform transforom_stamp;  // ������ ���� ��ġ

    private Stack<GameObject> stampStack = new Stack<GameObject>(); // ������ ������ ������ ����


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


        Debug.Log(iStampNum);

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
            }
        }

        TurnOffObj();
        yield break;
    }














    // #. IPartOwner �������̽�
    #region

    public void InsertOwnerFunc(GameObject stampParts, int index)
    {
        StampParts stampParts_ = stampParts.GetComponent<StampParts>();
        iStampNum = stampParts_.iStampeNum;
    }

    public void RemoveOwnerFunc(int index)
    {
        iStampNum = 0;
    }



    #endregion
}
