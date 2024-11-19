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

        GameAssistManager.Instance.StartCoroutine(ChangeMap());
        // StartCoroutine();
    }

    // #. �� ���� �Լ�
    IEnumerator ChangeMap()
    {
        Rigidbody rigid = GameAssistManager.Instance.player.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(1.2f);

        cineChager.CameraChange();

        yield return new WaitForSeconds(3.2f);

        map_InSide.SetActive(true);
        map_InSide.transform.position = new Vector3(
           GameAssistManager.Instance.player.transform.position.x,  // player�� X ��ǥ �״��
           GameAssistManager.Instance.player.transform.position.y - 0.2f,  // player�� Y ��ǥ���� -2��ŭ
           GameAssistManager.Instance.player.transform.position.z   // player�� Z ��ǥ �״��
                );
        map_OutSide.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        rigid.constraints = RigidbodyConstraints.None; // FreezePosition X, Y, Z ��� false

        // Freeze Rotation ����
        rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationY |
                            RigidbodyConstraints.FreezeRotationZ;


        yield return new WaitForSeconds(6f);
        

        InsideAssist_GGILICK.Instance.bCarCreating = true;

        playerCameraAssist.SetActive(false); // �������̿� ī�޶� OFF
    }


}
