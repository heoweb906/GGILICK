using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAssistManager : MonoBehaviour
{
    public static GameAssistManager Instance { get; private set; }

    public GameObject player;

    public int iStageNum;

    public Transform[] Transforms_Respawn;
    public GameObject[] Cameras;

    private void Start()
    {
        Instance = this;
        player = FindPlayerRoot();

        SaveData_Manager.Instance.SetStringSceneName(SceneManager.GetActiveScene().name);

        if(SaveData_Manager.Instance.GetIntClearStageNum() > iStageNum)
        {
            SaveData_Manager.Instance.SetIntClearStageNum(iStageNum);
            SaveGameProgress(0, 0);
        }

        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            DiePlayerReset();
        }
    }


    // #. �����ؾ� �� �����ǰ� ī�޶� �����ϴ� �Լ�
    public void SaveGameProgress(int iTransform, int iCamera)
    {
        SaveData_Manager.Instance.SetIntTransformRespawn(iTransform);
        SaveData_Manager.Instance.SetIntCameraNum(iCamera);
    }


    // #. �÷��̾��� ��ġ�� ī�޶� �������ִ� �Լ�
    private void PlayerStartSeeting(int iTransform, int iCamera)
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

        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());

        yield return new WaitForSeconds(2f);

        InGameUIController.Instance.FadeInOutImage(0f, 1f);
    }



    public void RespawnChangeAssist(Transform transform)
    {
        for (int i = 0; i < Transforms_Respawn.Length; i++)
        {
            if (Transforms_Respawn[i] == transform)
            {
                Debug.Log(i);
                SaveData_Manager.Instance.SetIntTransformRespawn(i);
                break;
            }
        }
    }



    // #. ������ ī�޶� �ٲٴ� �Լ�
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



    private GameObject FindPlayerRoot()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in playerObjects)
        {
            // �ְ� �θ� ������Ʈ�� ��ȯ
            if (obj.transform.root.CompareTag("Player"))
            {
                return obj.transform.root.gameObject;
            }
        }

        return null; // "Player" �±��� �ְ� �θ� ������Ʈ�� ���� ���
    }




}
