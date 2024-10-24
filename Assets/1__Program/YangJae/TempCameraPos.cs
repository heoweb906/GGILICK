using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraPos : MonoBehaviour
{

    public GameObject player;

    [Range(-20, 20)]
    public float xPos;
    [Range(0, 20)]
    public float yPos;
    [Range(-20, 20)]
    public float zPos;

    void Update()
    {
        transform.position =
            new Vector3(player.transform.position.x + xPos,
            player.transform.position.y + yPos,
            player.transform.position.z + zPos);
    }
}
