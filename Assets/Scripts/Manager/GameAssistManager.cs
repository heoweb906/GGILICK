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

    [Header("스테이지 / 리스폰 관련")]
    public int iStageNum;
    public Transform[] Transforms_Respawn;
    public GameObject[] Cameras;
    private bool bPlayerDie;   // 현재 플레이어가 죽음 상태 -> 죽음 반복 방지

    [Header("연출 관련")]
    private Volume volume;
    private Vignette vignette;
    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        Instance = this; // 인스턴스 생성
        bPlayerDie = false;

        Debug.Log(bPlayerDie);

        player = FindPlayerRoot();

        // #. 스테이지 관리
        SaveData_Manager.Instance.SetStringSceneName(SceneManager.GetActiveScene().name);

        if(SaveData_Manager.Instance.GetIntClearStageNum() < iStageNum)
        {
            SaveData_Manager.Instance.SetIntClearStageNum(iStageNum);
            SaveGameProgress(0, 0);
        }
        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());

        // #. Volume 관리
        volume = FindObjectOfType<Volume>();

        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out colorAdjustments);
    }



    // #. 갱신해야 할 포지션과 카메라를 저장하는 함수
    public void SaveGameProgress(int iTransform, int iCamera)
    {
        SaveData_Manager.Instance.SetIntTransformRespawn(iTransform);
        SaveData_Manager.Instance.SetIntCameraNum(iCamera);
    }

    // #. 플레이어의 위치와 카메라를 설정해주는 함수
    public void PlayerStartSeeting(int iTransform, int iCamera)
    {
        player.transform.position = Transforms_Respawn[iTransform].position;
        Cameras[iCamera].SetActive(true);
    }






    // #. 플레이어가 죽었을 때 실행시킬 함수
    public void DiePlayerReset(float fDieDelay = 2f)  // 죽음 함수를 실행 시키고 얼마나 뒤에 상태를 리셋할 건지 정할 수 있도록
    {
        if (!bPlayerDie)
        {
            bPlayerDie = true;
            StartCoroutine(_DiePlayerReset(fDieDelay)); // '_DiePlayerReset'이라는 코루틴을 호출합니다.
        }
    }
    IEnumerator _DiePlayerReset(float _fDieDelay) 
    {
        yield return new WaitForSeconds(_fDieDelay); 

        InGameUIController.Instance.FadeInOutImage(1f, 1f);

        yield return new WaitForSeconds(2f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }



    // #. Respawn 지점 업데이트
    // GameAssist에서 가지고 있는 위치와 동일해야 됨
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



    // #. 실제로 사용할 카메라만 True로 하고 나머지는 모두 false
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
    // #. 현재 활성화된 카메라와 변경하려는 카메라가 다른지 구분하는 함수
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



  



    // #. 내부 진입 
    public void FadeOutInEffect(float fStartImte = 3.0f, float fEndTime = 3.0f)
    {
        if (vignette == null || colorAdjustments == null) return;

        // 플레이어 위치를 화면 좌표로 변환, Vignette의 중심으로 함
        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);
        DOTween.To(() => vignette.center.value, x => vignette.center.value = x, new Vector2(playerViewportPosition.x, playerViewportPosition.y), 0f);

        // 1. 연출 진입
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, fStartImte);
        DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, -10f, fStartImte * 1.2f);
      

        // 2. 연출 아웃
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, fStartImte)
            .SetDelay(fEndTime);
        DOTween.To(() => colorAdjustments.postExposure.value, x => colorAdjustments.postExposure.value = x, 0f, fStartImte * 1.2f)
            .SetDelay(fEndTime);
    }




























    // #. Player 태그가 붙은 오브젝트 중에 가장 부모 오브젝트를 찾아오는 함수
    private GameObject FindPlayerRoot()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in playerObjects)
        {
            if (obj.transform.root.CompareTag("Player")) return obj.transform.root.gameObject;
        }
        return null; // "Player" 태그의 최고 부모 오브젝트가 없을 경우
    }


    public bool GetBoolPlayerDie()
    {
        return bPlayerDie;
    }

}
