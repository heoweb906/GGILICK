using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolaroidCamera : ClockBattery
{
    public PolaroidScanner[] polaroidScanners;
    public int[] iCorrectSequence;

    [Header("���� ���� ����")]
    public Transform transformPictureCreate;
    public GameObject Picture_Complete;         // ���� �ϼ���
    public GameObject Picture_Grouping;         // ���� �������� ������ ������Ʈ
    public GameObject[] PicturePieces;         // ���� ������ (�߸��� ������ ������� ��)



    private Coroutine nowCoroutine;

    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(ShutterCountStart());
    }
    public override void TurnOffObj()
    {
        base.TurnOffObj();
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        ShootCamera();
    }

    // #. ī�޶� ���� ī��Ʈ �ٿ�
    IEnumerator ShutterCountStart()
    {
        while (fCurClockBattery > 0)
        {
            yield return new WaitForSeconds(1f);
            fCurClockBattery -= 1;
        }

        TurnOffObj();
    }

    




    // #. ī�޶� �Կ�
    private void ShootCamera()
    {
        for(int i = 0; i < 4; i++)
        {
            if (polaroidScanners[i].iFigureIndex != iCorrectSequence[i])
            {
                FailShoot();
                return;
            }
        }

        SuccesShoot();
    }



    // #. �Կ� ����
    private void SuccesShoot()
    {


        
        PrintPicture(Picture_Complete);          // �ϼ��� ���� ���
    }

    // #. �Կ� ����
    private async void FailShoot()
    {




        StartCoroutine(FailShootCoroutine());    // ������ ���� ������ ���
    }
    private IEnumerator FailShootCoroutine()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < 4;)
        {
            tempList.Add(polaroidScanners[i].iFigureIndex);
            int temp = i;

            for (int j = 1; j < 4; j++)
            {
                if (temp + j < 4)
                {
                    if (polaroidScanners[temp].iFigureIndex == 0) break; // ���� ��Ұ� 0���̶�� ����

                    if (polaroidScanners[temp].iFigureIndex == polaroidScanners[temp + j].iFigureIndex - j)
                    {
                        tempList.Add(polaroidScanners[temp + j].iFigureIndex);
                        i++;
                    }
                    else break;
                }
            }

            PrintPicture(Picture_Grouping, tempList);
            tempList.Clear();

            yield return new WaitForSeconds(1); // 1�� ���

            i++;
        }
    }









    // #. ���� ��� (GameObject ����� ����)
    private void PrintPicture(GameObject picture_, List<int> _intList = null)
    {
        GameObject spawnedObject = Instantiate(picture_, transformPictureCreate.position, Quaternion.identity);

        if (_intList != null)
        {
            float spacing = 0.2f; // ���� ���� (�ʿ信 ���� ���� ����)
            int count = 0; // �ڽ� ������Ʈ�� ������ ����

            foreach (int index in _intList)
            {
                if (index >= 0 && index < PicturePieces.Length) // ��ȿ�� �ε������� Ȯ��
                {
                    GameObject piece = Instantiate(PicturePieces[index]);
                    piece.transform.SetParent(spawnedObject.transform);

                    // X �� �������� ������ �־� ��ġ
                    piece.transform.localPosition = new Vector3(count * spacing, 0, 0);

                    count++; // ���� �ڽ� ������Ʈ�� ��ġ�� ���� ����
                }
            }
        }


        // Rigidbody ����
        Rigidbody objRigidbody = spawnedObject.GetComponent<Rigidbody>();

        // �ʱ� ��ġ�� ���� (X������ ���������� �̵�)
        Vector3 startPosition = transformPictureCreate.position;
        startPosition.x += 2f; // �����ʿ��� ����
        spawnedObject.transform.position = startPosition;

        // Rigidbody �ʱ�ȭ ����
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // DOTween ���� ���� ȿ�� ��Ȱ��ȭ
        }

        // ��ǥ ��ġ ���� (�������� �߻�)
        Vector3 targetPosition = transformPictureCreate.position;
        //targetPosition.x -= 1f; // �������� �̵�

        // DOTween���� �ε巯�� � �߻� �ִϸ��̼�
        spawnedObject.transform.DOPath(
            new Vector3[] { startPosition, targetPosition },
            1f, // �ִϸ��̼� ���� �ð�
            PathType.CatmullRom)
            .SetEase(Ease.OutQuad) // �ڿ������� �߻�
            .OnComplete(() =>
            {
                // �ִϸ��̼� �Ϸ� �� ���� ȿ�� Ȱ��ȭ
                if (objRigidbody != null)
                {
                    objRigidbody.isKinematic = false; // ���� �ۿ� Ȱ��ȭ
                    objRigidbody.useGravity = true;  // �߷� Ȱ��ȭ
                    objRigidbody.AddForce(new Vector3(-2f, 0f, 0f), ForceMode.Impulse); // �������� �߰� ��
                }
            });
    }


}
