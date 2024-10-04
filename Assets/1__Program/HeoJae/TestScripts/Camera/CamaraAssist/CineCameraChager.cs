using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineCameraChager : MonoBehaviour
{
    public GameObject OriginalCamera;
    public GameObject TargetCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(OriginalCamera.activeSelf && !TargetCamera.activeSelf)
            {
                OriginalCamera.SetActive(false);
                TargetCamera.SetActive(true);
            } 
            else if(!OriginalCamera.activeSelf && TargetCamera.activeSelf)
            {
                OriginalCamera.SetActive(true);
                TargetCamera.SetActive(false);
            }
        }
    }
}
