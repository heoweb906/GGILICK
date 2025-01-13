using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyClockWorkAssist : MonoBehaviour, IPartsOwner
{
    // 0 = Carried , 1 = ClockWork
    private BoxCollider boxColliders;       
    private CarriedObject carriedObject;
    private ClockWork clockWork;

    private void Awake()
    {
        boxColliders = GetComponent<BoxCollider>();
        carriedObject = GetComponent<CarriedObject>();
        clockWork = GetComponent<ClockWork>();
    }


    public void InsertOwnerFunc(GameObject soundPieceObj, int index)
    {
        boxColliders.size = new Vector3(0.6f, 0.37f, 0.5f);

        Destroy(carriedObject, 1.1f);
        clockWork.enabled = true;
    }

    public void RemoveOwnerFunc(int index)
    {
       
    }
}
