using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.Unicode;

public class GameMachine : ClockBattery, IPartsOwner
{
    private Coroutine nowCoroutine;


    [Header("���ӱ� ����")]
    private bool bPowerOn = false;


    [Header("���� ���� ����")]
    public bool[] bBulbBug;         // ���� ������ ������ ��������
    public GameObject[] SpritesBulbBug;





    public override void TurnOnObj()
    {
        base.TurnOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(RebootingGameMachine());
    }
    public override void TurnOffObj()
    {
        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        if (!bPowerOn)
        {
            base.TurnOffObj();
            BulbBugSpriteonOff(0, false, true);
        }
        else    // ������ ������ ���
        {
            Debug.Log("���͸� ���� ����!!!");
        }
        


       
    }




    IEnumerator RebootingGameMachine()
    {
        int iIndex = 0;
        while (fCurClockBattery > 0)
        {
            if (fCurClockBattery >= 3 && CheckIfAnyFalse(bBulbBug)) bPowerOn = true;

            yield return new WaitForSecondsRealtime(1.0f);

            if(iIndex <= 2) BulbBugSpriteonOff(iIndex, bBulbBug[iIndex]);
            iIndex++;

            fCurClockBattery -= 1;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        TurnOffObj(); // ���͸��� �� �Ǹ� ����
    }


    private void BulbBugSpriteonOff(int index, bool bOnOff, bool bAllReset = false)
    {
        if(bAllReset)
        {
            foreach (GameObject sprite in SpritesBulbBug) sprite.SetActive(false);
            return;
        }
            

        if (index == 0)
        {
            if (bOnOff) SpritesBulbBug[0].SetActive(true);
            else SpritesBulbBug[3].SetActive(true);
        }
        else if(index == 1)
        {
            if (bOnOff) SpritesBulbBug[1].SetActive(true);
            else SpritesBulbBug[4].SetActive(true);
        }
        else if (index == 2)
        {
            if (bOnOff) SpritesBulbBug[2].SetActive(true);
            else SpritesBulbBug[5].SetActive(true);
        }
    }
    private bool CheckIfAnyFalse(bool[] bArray)
    {
        foreach (bool b in bArray)
        {
            if (!b)
            {
                return false;  // �ϳ��� false�� ������ false ��ȯ
            }
        }
        return true;  // ��� ���� true�� ��� true ��ȯ
    }






    public void InsertOwnerFunc(GameObject bulbGub, int index)
    {
        bBulbBug[index] = true;
    }

    public void RemoveOwnerFunc(int index)
    {
        bBulbBug[index] = false;
    }
}
