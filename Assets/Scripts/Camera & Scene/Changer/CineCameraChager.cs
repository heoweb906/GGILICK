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



       
    // #. CinemachineBrain - ����� ī�޶� ��ȯ�� �� �ҷ��ͼ� ����
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
            Debug.Log("ī�޶� ���� �����Ͱ� �����ϴ�.");
        }
       
    }



}
