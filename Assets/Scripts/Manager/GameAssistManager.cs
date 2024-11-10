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


    // #. 갱신해야 할 포지션과 카메라를 저장하는 함수
    public void SaveGameProgress(int iTransform, int iCamera)
    {
        SaveData_Manager.Instance.SetIntTransformRespawn(iTransform);
        SaveData_Manager.Instance.SetIntCameraNum(iCamera);
    }


    // #. 플레이어의 위치와 카메라를 설정해주는 함수
    private void PlayerStartSeeting(int iTransform, int iCamera)
    {
        player.transform.position = Transforms_Respawn[iTransform].position;
        Cameras[iCamera].SetActive(true);
    }



    // #. 플레이어가 죽었을 때 실행시킬 함수
    public void DiePlayerReset(float fDieDelay = 2f)  // 죽음 함수를 실행 시키고 얼마나 뒤에 상태를 리셋할 건지 정할 수 있도록
    {
        StartCoroutine(_DiePlayerReset(fDieDelay)); // '_DiePlayerReset'이라는 코루틴을 호출합니다.
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



    // #. 실제로 카메라를 바꾸는 함수
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
        if(camera == obj) return true; // 현재 활성화 되어 있는 카메라와 동일하면 true를 반환
                          return false;
    }



    private GameObject FindPlayerRoot()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in playerObjects)
        {
            // 최고 부모 오브젝트를 반환
            if (obj.transform.root.CompareTag("Player"))
            {
                return obj.transform.root.gameObject;
            }
        }

        return null; // "Player" 태그의 최고 부모 오브젝트가 없을 경우
    }




}
