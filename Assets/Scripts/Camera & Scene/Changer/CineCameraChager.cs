using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CineCameraChager : MonoBehaviour
{
    public GameObject TargetCamera; 

    private Camera mainCamera;
    private CinemachineBrain cineBrain;



    private void Awake()
    {
        mainCamera = Camera.main;
        cineBrain  = mainCamera.GetComponent<CinemachineBrain>();
    }


    public void CameraChange()
    {
        BlendChanger(TargetCamera);
        GameAssistManager.Instance.RespawnChangeAssist(gameObject.transform);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            BlendChanger(TargetCamera);
            GameAssistManager.Instance.RespawnChangeAssist(gameObject.transform);
        }
    }



       
    // #. CinemachineBrain - 버츄얼 카메라 전환시 값 불러와서 적용
    private void BlendChanger(GameObject targetCamera)
    {
        if (GameAssistManager.Instance.BoolNowActiveCameraObj(targetCamera)) return;

        GameAssistManager.Instance.CameraChangeAssist(targetCamera);

        CameraObj camObj = targetCamera.GetComponent<CameraObj>();
        if(camObj.blendData != null)
        {
            CameraBlendData blendData = camObj.blendData;
            cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(blendData.blendStyle, blendData.duration);
            Debug.Log(blendData.blendStyle + "      /       " + blendData.duration);
        }
        else
        {
            Debug.Log("카메라 블렌드 데이터가 없습니다.");
        }
       
    }



}
