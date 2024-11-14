using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameAssistManager : MonoBehaviour
{
    public static GameAssistManager Instance { get; private set; }

    public GameObject player;

    [Header("�������� / ������ ����")]
    public int iStageNum;
    public Transform[] Transforms_Respawn;
    public GameObject[] Cameras;

    [Header("���� ����")]
    private Volume volume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        Instance = this; // �ν��Ͻ� ����
        player = FindPlayerRoot();


        // #. �������� ����
        SaveData_Manager.Instance.SetStringSceneName(SceneManager.GetActiveScene().name);

        if(SaveData_Manager.Instance.GetIntClearStageNum() > iStageNum)
        {
            SaveData_Manager.Instance.SetIntClearStageNum(iStageNum);
            SaveGameProgress(0, 0);
        }
        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());



        // #. Volume ����
        volume = FindObjectOfType<Volume>();



        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);

        
    }


   


    // #. �����ؾ� �� �����ǰ� ī�޶� �����ϴ� �Լ�
    public void SaveGameProgress(int iTransform, int iCamera)
    {
        SaveData_Manager.Instance.SetIntTransformRespawn(iTransform);
        SaveData_Manager.Instance.SetIntCameraNum(iCamera);
    }

    // #. �÷��̾��� ��ġ�� ī�޶� �������ִ� �Լ�
    public void PlayerStartSeeting(int iTransform, int iCamera)
    {
        player.transform.position = Transforms_Respawn[iTransform].position;
        Cameras[iCamera].SetActive(true);
    }






    // #. �÷��̾ �׾��� �� �����ų �Լ�
    public void DiePlayerReset(float fDieDelay = 2f)  // ���� �Լ��� ���� ��Ű�� �󸶳� �ڿ� ���¸� ������ ���� ���� �� �ֵ���
    {
        StartCoroutine(_DiePlayerReset(fDieDelay)); // '_DiePlayerReset'�̶�� �ڷ�ƾ�� ȣ���մϴ�.
    }
    IEnumerator _DiePlayerReset(float _fDieDelay) 
    {
        yield return new WaitForSeconds(_fDieDelay); 

        InGameUIController.Instance.FadeInOutImage(1f, 1f);

        yield return new WaitForSeconds(2f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }



    // #. Respawn ���� ������Ʈ
    // GameAssist���� ������ �ִ� ��ġ�� �����ؾ� ��
    public void RespawnChangeAssist(Transform transform)
    {
        for (int i = 0; i < Transforms_Respawn.Length; i++)
        {
            if (Transforms_Respawn[i] == transform)
            {
                SaveData_Manager.Instance.SetIntTransformRespawn(i);
                break;
            }
        }
    }



    // #. ������ ����� ī�޶� True�� �ϰ� �������� ��� false
    public void CameraChangeAssist(GameObject camera)
    {
        for (int i = 0; i < Cameras.Length; i++)
        {
            if (Cameras[i] != camera) Cameras[i].SetActive(false);
            else
            {
                Cameras[i].SetActive(true);
                SaveData_Manager.Instance.SetIntCameraNum(i);
            }
               
        }
    }
    // #. ���� Ȱ��ȭ�� ī�޶�� �����Ϸ��� ī�޶� �ٸ��� �����ϴ� �Լ�
    public bool BoolNowActiveCameraObj(GameObject camera)
    {
        GameObject obj = null;
        for(int i = 0; i < Cameras.Length; i++)
        {
            if (Cameras[i].activeSelf == true)
            {
                obj = Cameras[i];
                break;
            }
        }
        if(camera == obj) return true; // ���� Ȱ��ȭ �Ǿ� �ִ� ī�޶�� �����ϸ� true�� ��ȯ
                          return false;
    }



  



    // #. ���� ���� 
    public void FadeOutInEffect(float fStartImte = 3.0f, float fEndTime = 3.0f)
    {
        if (vignette == null || colorAdjustments == null) return;


        // �÷��̾� ��ġ�� ȭ�� ��ǥ�� ��ȯ, Vignette�� �߽����� ��
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);
        DOTween.To(() => vignette.center.value, x => vignette.center.value = x, new Vector2(playerViewportPosition.x, playerViewportPosition.y), 0f);

        // 1. ���� ����
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, fStartImte);
        DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, -10f, fStartImte * 1.2f);



      

        // 2. ���� �ƿ�
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, fStartImte)
            .SetDelay(fEndTime);
        DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, 0f, fStartImte * 1.2f)
            .SetDelay(fEndTime);
    }








































    // #. Player �±װ� ���� ������Ʈ �߿� ���� �θ� ������Ʈ�� ã�ƿ��� �Լ�
    private GameObject FindPlayerRoot()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playerObjects)
        {
            if (obj.transform.root.CompareTag("Player")) return obj.transform.root.gameObject;
        }
        return null; // "Player" �±��� �ְ� �θ� ������Ʈ�� ���� ���
    }


}
