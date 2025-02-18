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
using Unity.VisualScripting;
using JetBrains.Annotations;

public class GameAssistManager : MonoBehaviour
{
    public static GameAssistManager Instance { get; private set; }

    public GameObject player;

    [Header("스테이지 / 리스폰 관련")]
    public int iStageNum;
    public Transform[] Transforms_Respawn;
    public GameObject[] Cameras;
    private bool bPlayerDie;   // 현재 플레이어가 죽음 상태 -> 죽음 반복 방지

    [Header("연출 관련 내면 세계 진입")]
    public GameObject CameraOverlay;
    public Volume volume_1;

    private Vignette vignette_1;
    private ColorAdjustments colorAdjustments_1;
    private DepthOfField depthOfField_1;
    private Bloom bloom_1;
    private ShadowsMidtonesHighlights midtonesHighlights_1;
    private WhiteBalance whiteBalance_1;


    private void Awake()
    {
        Instance = this; // 인스턴스 생성
        bPlayerDie = false;
       
        player = FindPlayerRoot();

        // #. 스테이지 관리
        SaveData_Manager.Instance.SetStringSceneName(SceneManager.GetActiveScene().name);

        if(SaveData_Manager.Instance.GetIntClearStageNum() < iStageNum)
        {
            SaveData_Manager.Instance.SetIntClearStageNum(iStageNum);
            SaveGameProgress(0, 0);
        }
        PlayerStartSeeting(SaveData_Manager.Instance.GetIntTransformRespawn(), SaveData_Manager.Instance.GetIntCameraNum());


        // 플레이어 조작 가능
        PlayerInputLockOff();


        // #. Volume 관리
        if (volume_1 == null) Debug.Log("Volume이 비어있습니다.");
        volume_1.profile.TryGet(out vignette_1);
        volume_1.profile.TryGet(out colorAdjustments_1);
        volume_1.profile.TryGet(out depthOfField_1);
        volume_1.profile.TryGet(out midtonesHighlights_1);
        volume_1.profile.TryGet(out bloom_1);
        volume_1.profile.TryGet(out whiteBalance_1);


       
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
            PlayerInputLockOn();
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
        if (vignette_1 == null || colorAdjustments_1 == null) return;
        // if (vignette_2 == null || colorAdjustments_2 == null) return;

     
        StartCoroutine(FadeOutInEffect_(fStartImte, fEndTime));
    }
    IEnumerator FadeOutInEffect_(float fStartImte = 3.0f, float fEndTime = 3.0f)
    {
        CameraOverlay.SetActive(true);
        colorAdjustments_1.postExposure.value = 0f;
        colorAdjustments_1.contrast.value = 0f;
        colorAdjustments_1.colorFilter.value = Color.white;
        colorAdjustments_1.saturation.value = 0f;
        depthOfField_1.active = false;
        bloom_1.active = false;
        midtonesHighlights_1.active = false;
        whiteBalance_1.active = false;




        Vector3 playerViewportPosition = Camera.main.WorldToViewportPoint(player.transform.position);
        DOTween.To(() => vignette_1.center.value, x => vignette_1.center.value = x, new Vector2(playerViewportPosition.x, playerViewportPosition.y), 0f);
        DOTween.To(() => vignette_1.intensity.value, x => vignette_1.intensity.value = x, 1f, 2f).SetEase(Ease.InOutQuad);

        DOTween.To(() => colorAdjustments_1.postExposure.value, x => colorAdjustments_1.postExposure.value = x, -10f, 2f).SetEase(Ease.InOutQuad);


        yield return new WaitForSeconds(fStartImte);


        DOTween.To(() => vignette_1.center.value, x => vignette_1.center.value = x, new Vector2(0.5f, 0.05f), 0f);
        DOTween.To(() => vignette_1.intensity.value, x => vignette_1.intensity.value = x, 0.155f, fStartImte).SetEase(Ease.InOutQuad);

        DOTween.To(() => colorAdjustments_1.postExposure.value, x => colorAdjustments_1.postExposure.value = x, 0f, fStartImte).SetEase(Ease.InOutQuad);


        yield return new WaitForSeconds(fStartImte);

        CameraOverlay.SetActive(false);
        colorAdjustments_1.postExposure.value = -0.46f;
        colorAdjustments_1.contrast.value = 38f;
        colorAdjustments_1.colorFilter.value = new Color(0.737f, 0.71f, 0.71f);
        colorAdjustments_1.saturation.value = -14f;
        depthOfField_1.active = true;
        bloom_1.active = true;
        midtonesHighlights_1.active = true;
        whiteBalance_1.active = true;

    }
















    // 플레이어 관련
    #region



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
    public GameObject GetPlayer()
    {
        return player;
    }

    public bool GetBoolPlayerDie()
    {
        return bPlayerDie;
    }
    



    public void PlayerInputLockOn()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null) return;

        playerScript.machine.OnStateChange(playerScript.machine.UC_IdleState);
    }


    public void PlayerInputLockOff()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript == null) return;

        playerScript.machine.OnStateChange(playerScript.machine.IdleState);

        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        playerRigidbody.constraints = RigidbodyConstraints.None;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // playerScript.machine.UC_DieState


    #endregion

}
