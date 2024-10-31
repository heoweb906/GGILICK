using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraPos : MonoBehaviour
{

    public GameObject player;
    public GameObject parentObj;

    [Range(-50, 50)]
    public float xPos;
    [Range(-50, 50)]
    public float yPos;
    [Range(-50, 50)]
    public float zPos;

    void Update()
    {
        transform.localPosition =
            new Vector3(xPos,
            yPos,
            zPos);

        parentObj.transform.position = player.transform.position;

        if (Input.GetKeyDown(KeyCode.O))
        {
            parentObj.transform.rotation =
                Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y - 5, parentObj.transform.eulerAngles.z));
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            parentObj.transform.rotation =
                Quaternion.Euler(new Vector3(parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y + 5, parentObj.transform.eulerAngles.z));
        }
    }
}
