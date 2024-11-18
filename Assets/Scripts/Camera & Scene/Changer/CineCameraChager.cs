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


    // #. �ٸ� ��ũ��Ʈ���� �۵��ϴ� ��
    public void CameraChange()
    {
        BlendChanger(TargetCamera);
        if(TartgetTransform!= null)
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform);
    }


    // #. Ʈ���ŷ� �۵��ϴ� ���
    private void OnTriggerExit(Collider other) 
    {
        if (other.transform.root.CompareTag("Player") && !bTriggerOff) 
        {
            BlendChanger(TargetCamera);
            GameAssistManager.Instance.RespawnChangeAssist(TartgetTransform); 
        }
    }



       
    // #. CinemachineBrain - ����� ī�޶� ��ȯ�� �� �ҷ��ͼ� ����
    private void BlendChanger(GameObject targetCamera)
    {
        if (GameAssistManager.Instance.BoolNowActiveCameraObj(targetCamera)) return;

        GameAssistManager.Instance.CameraChangeAssist(targetCamera);
        Debug.Log($"ī�޶� ��ȯ - ȣ���� ������Ʈ: {gameObject.name}");

        CameraObj camObj = targetCamera.GetComponent<CameraObj>();
        cineBrain.m_DefaultBlend = new CinemachineBlendDefinition(camObj.blendStyle, camObj.duration);
    }



}
