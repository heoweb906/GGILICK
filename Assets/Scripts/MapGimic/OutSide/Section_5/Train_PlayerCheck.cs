using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_PlayerCheck : MonoBehaviour
{
    public bool bPlayerNearby = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = true;
            Debug.Log("Player entered the area.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bPlayerNearby = false;
            Debug.Log("Player left the area.");
        }
    }
}
