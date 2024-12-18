using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundBlockMachine : ClockBattery
{
    private Coroutine nowCoroutine;

    [Header("�Ҹ� ���� ����")]
    public SoundPiece[] soundPieces = new SoundPiece[4];
    public int[] iCorrectArray;
    private bool bScanFail;

    [Header("PartOwner ����")]
    public SoundBlockMachine_PartOwner[] partOwners;





    private void Awake()
    {
        foreach (var partOwner in partOwners) partOwner.SoundBlockMachine_ = this;
    }


    public override void TrunOnObj()
    {
        base.TrunOnObj();

        RotateObject((int)fCurClockBattery);
        nowCoroutine = StartCoroutine(PlayPitchSoundsCoroutine());
    }
    public override void TrunOffObj()
    {
        base.TrunOffObj();

        if (nowCoroutine != null) StopCoroutine(nowCoroutine);

        if (bScanFail)
            FailPlayAction();
        else
            SuccesPlayAction();
    }





    // #. �Ǳ� �۵�
    private IEnumerator PlayPitchSoundsCoroutine()
    {
        int iCurBattery = (int)fCurClockBattery;
        bScanFail = false;

        while (fCurClockBattery > 0)
        {
            for (int i = 0; i < soundPieces.Length; i++)
            {
                if (soundPieces[i] != null)
                {
                    if (soundPieces[i].iSoundPieceNum != iCorrectArray[i]) bScanFail = true;
                    soundPieces[i].PlayingPitchSound();
                }
                else
                {
                    bScanFail = true;
                    SoundAssistManager.Instance.GetSFXAudioBlock("POP Brust 08", gameObject.transform);
                }

                // ���͸� ����
                iCurBattery -= 1;
                if (iCurBattery <= 0)
                {
                    fCurClockBattery = 0;
                    TrunOffObj();
                    yield break; // �ڷ�ƾ ����
                }

                yield return new WaitForSecondsRealtime(1.0f);
            }


            TrunOffObj();
            yield break; // ��� �۾��� �Ϸ�Ǹ� �ڷ�ƾ ����
        }

     
    }




    // #. ���ֿ� �������� �� �����ų �Լ�
    private void SuccesPlayAction()
    {

    }

    // #. ���ֿ� �������� �� �����ų �Լ�
    private void FailPlayAction()
    {

    }




    // ����Ʈ ���� �Լ� ����
    #region

    public void AddSoundPiece(GameObject soundPieceObj, int index)
    {
        SoundPiece soundPiece = soundPieceObj.GetComponent<SoundPiece>();
        soundPieces[index] = soundPiece;
    }
    public void RemoveSoundPiece(int index)
    {
        soundPieces[index] = null;
    }
    #endregion





    



}
