using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundBlockMachine : ClockBattery
{
    private Coroutine nowCoroutine;

    [Header("소리 조각 관련")]
    public SoundPiece[] soundPieces = new SoundPiece[4];
    public int[] iCorrectArray;
    private bool bScanFail;

    [Header("PartOwner 관련")]
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





    // #. 악기 작동
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

                // 배터리 감소
                iCurBattery -= 1;
                if (iCurBattery <= 0)
                {
                    fCurClockBattery = 0;
                    TrunOffObj();
                    yield break; // 코루틴 종료
                }

                yield return new WaitForSecondsRealtime(1.0f);
            }


            TrunOffObj();
            yield break; // 모든 작업이 완료되면 코루틴 종료
        }

     
    }




    // #. 연주에 성공했을 때 실행시킬 함수
    private void SuccesPlayAction()
    {

    }

    // #. 연주에 실패했을 때 실행시킬 함수
    private void FailPlayAction()
    {

    }




    // 리스트 관련 함수 모음
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
