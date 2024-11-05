using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCameraChager : MonoBehaviour
{
    public GameObject OriginalCamera;
    public GameObject TargetCamera;

    public Camera mainCamera;
    public CinemachineBrain cineBrain;


    private void Awake()
    {
        mainCamera = Camera.main;
        cineBrain  = mainCamera.GetComponent<CinemachineBrain>();
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            CameraChange();
        }
    }



    public void CameraChange()
    {
        if (OriginalCamera.activeSelf && !TargetCamera.activeSelf) 
        {
            BlendChanger(OriginalCamera, TargetCamera);
        }
        else if (!OriginalCamera.activeSelf && TargetCamera.activeSelf)
        {
            BlendChanger(TargetCamera, OriginalCamera);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(OriginalCamera.activeSelf && !TargetCamera.activeSelf)
            {
                BlendChanger(OriginalCamera, TargetCamera);
            } 
            else if(!OriginalCamera.activeSelf && TargetCamera.activeSelf)
            {
                BlendChanger(TargetCamera, OriginalCamera);
            }
        }
    }


       
    // #. CinemachineBrain - 버츄얼 카메라 전환시 값 불러와서 적용
    private void BlendChanger(GameObject firstCamera, GameObject secondCamera)
    {
        firstCamera.SetActive(false);
        secondCamera.SetActive(true);

        CameraObj camObj = secondCamera.GetComponent<CameraObj>();
        CameraBlendData blendData = camObj.blendData;
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(blendData.blendStyle, blendData.duration);
        Debug.Log(blendData.blendStyle + "      /       " + blendData.duration);
    }

}
