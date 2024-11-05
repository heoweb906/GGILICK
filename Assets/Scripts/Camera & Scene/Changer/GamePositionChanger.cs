using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePositionChanger : MonoBehaviour
{
    public InGameUIController inGameUIController;
    public string sChangeSceneName = "Only_PositionChange";
    public bool bOnlyPositionChange; // �����Ǹ� �ٲٴ��� or Scene�� �Բ� �ٲٴ���

    [Header("Position �̵��� ��쿡 �ʿ��� ������")]
    public GameObject player;
    public CineCameraChager cienCamareChager;
    public Transform targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(bOnlyPositionChange) inGameUIController.ChangeScene(sChangeSceneName);
            else
            {
                if(cienCamareChager != null && targetPosition != null) ChangePosition();
            }
        }
    }


    private void ChangePosition()
    {
        StartCoroutine(ChangePosition_());
        
    }
    private IEnumerator ChangePosition_()
    {
        inGameUIController.bIsUIDoing = true;
        inGameUIController.FadeInOutImage(1f, 1.8f);

        yield return new WaitForSecondsRealtime(1.8f);

        cienCamareChager.CameraChange();
        player.transform.position = targetPosition.position;

        yield return new WaitForSecondsRealtime(0.5f);

        inGameUIController.FadeInOutImage(0f, 1.8f);
        inGameUIController.bIsUIDoing = false;

    }

}
