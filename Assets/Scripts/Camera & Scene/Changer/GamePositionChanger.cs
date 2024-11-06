using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePositionChanger : MonoBehaviour
{
    private GameObject player;
    public string sChangeSceneName = "Only_PositionChange";

    [Header("Position 이동일 경우에 필요한 정보들")]

    public CineCameraChager cienCamareChager;
    public Transform targetPosition;

    private void Start()
    {
        player = GameObject.Find("DolDolE");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(sChangeSceneName != "NULL") InGameUIController.Instance.ChangeScene(sChangeSceneName);
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
        InGameUIController.Instance.bIsUIDoing = true;
        InGameUIController.Instance.FadeInOutImage(1f, 1.8f);

        yield return new WaitForSecondsRealtime(1.8f);

        cienCamareChager.CameraChange();
        player.transform.position = targetPosition.position;

        yield return new WaitForSecondsRealtime(0.5f);

        InGameUIController.Instance.FadeInOutImage(0f, 1.8f);
        InGameUIController.Instance.bIsUIDoing = false;

    }

}
