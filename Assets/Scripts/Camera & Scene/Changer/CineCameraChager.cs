using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CineCameraChager : MonoBehaviour
{
    public bool bTriggerOff;

    public GameObject TargetCamera;
    public Transform TartgetTransform;

    private Camera mainCamera;
    private CinemachineBrain cineBrain;



    private void Awake()
    {
        mainCamera = Camera.main;
        cineBrain  = mainCamera.GetComponent<CinemachineBrain>();
    }


    // #. 다른 스크립트에서 작동하는 용
    public void CameraChange()
    {
        BlendChanger(TargetCamera);
        if(TartgetTransform!= null)
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform);
    }


    // #. 트리거로 작동하는 방식
    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.root.CompareTag("Player") && !bTriggerOff) 
        {
            BlendChanger(TargetCamera);
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform); 
        }
    }


       
    // #. CinemachineBrain - 버츄얼 카메라 전환시 값 불러와서 적용
    private void BlendChanger(GameObject targetCamera)
    {
        if (GameAssistManager.Instance.BoolNowActiveCameraObj(targetCamera)) return;

        GameAssistManager.Instance.CameraChangeAssist(targetCamera);
        Debug.Log($"카메라 전환 - 호출한 오브젝트: {gameObject.name}");

        CameraObj camObj = targetCamera.GetComponent<CameraObj>();
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, camObj.duration);
    }



}
