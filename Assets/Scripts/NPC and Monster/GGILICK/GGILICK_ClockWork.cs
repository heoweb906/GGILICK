using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork : InteractableObject
{
    public CineCameraChager cineChager;
    public Transform transformTeleport_Inside;
    public GameObject gamObejct;
    

    private void Start()
    {
        type = InteractableType.SingleEvent;
    }



    public override void ActiveEvent()
    {
        canInteract = false;
        GameAssistManager.Instance.InsideInEffect();
        GameAssistManager.Instance.StartCoroutine(InsideOn_GGILICK());
        // StartCoroutine();
    }

    // #. 맵 변경 함수
    IEnumerator InsideOn_GGILICK()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY;

        yield return new WaitForSeconds(1.2f);

        cineChager.CameraChange();

        yield return new WaitForSeconds(1.0f);

        DOTween.To(() => RenderSettings.fogDensity, x => RenderSettings.fogDensity = x, 0f, 2f);
        DOTween.To(() => RenderSettings.ambientIntensity, x => RenderSettings.ambientIntensity = x, 0.8f, 2f);

        yield return new WaitForSeconds(2.2f);


        // 플레이어랑 카메라 순간이동
        Vector3 teleportPosition = transformTeleport_Inside.position;
        Vector3 playerPosition = GameAssistManager.Instance.GetPlayer().transform.position;
        Vector3 offset = gamObejct.transform.position - playerPosition; // 플레이어와 gamObject 간의 상대적 위치
        gamObejct.transform.position = teleportPosition + offset; // gamObject를 새로운 위치에 배치
        yield return new WaitForEndOfFrame();
        GameAssistManager.Instance.GetPlayer().transform.position = teleportPosition; // 플레이어를 순간이동


        



        yield return new WaitForSeconds(0.4f);


        GameAssistManager.Instance.InsideOutEffect();

        rigid.constraints = RigidbodyConstraints.None; // FreezePosition X, Y, Z 모두 false
        rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationY |
                            RigidbodyConstraints.FreezeRotationZ;

        yield return new WaitForSeconds(5f);

        InsideCarCreator.Instance.bCanCarCreate = true;


    }


}
