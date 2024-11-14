using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGILICK_ClockWork : InteractableObject
{
    public GameObject playerCameraAssist;
    public CineCameraChager cineChager;
    public GameObject map_OutSide;
    public GameObject map_InSide;

    
    private void Start()
    {
        type = InteractableType.SingleEvent;
    }




    public override void ActiveEvent()
    {
        canInteract = false;
        playerCameraAssist.SetActive(true);
        GameAssistManager.Instance.FadeOutInEffect(4.5f, 4.5f);
        StartCoroutine(ChangeMap());
    }

    // #. �� ���� �Լ�
    IEnumerator ChangeMap()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(1.2f);
        cineChager.CameraChange();

        yield return new WaitForSeconds(3.2f);

        rigid.constraints = RigidbodyConstraints.FreezePositionY;

        map_InSide.SetActive(true);
        map_InSide.transform.position = new Vector3(
           GameAssistManager.Instance.player.transform.position.x,  // player�� X ��ǥ �״��
           GameAssistManager.Instance.player.transform.position.y - 0.2f,  // player�� Y ��ǥ���� -2��ŭ
           GameAssistManager.Instance.player.transform.position.z   // player�� Z ��ǥ �״��
                );
        map_OutSide.SetActive(false);

        yield return new WaitForSeconds(0.2f);
      
        rigid.constraints = RigidbodyConstraints.None;
        rigid.constraints = RigidbodyConstraints.FreezeRotation;


        yield return new WaitForSeconds(6f);

        playerCameraAssist.SetActive(false);
    }


}
