using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedObject : InteractableObject
{
    public Rigidbody rigid;
    public Collider col;

    private void Start()
    {
        type = InteractableType.Carrried;
        canInteract = true;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
}
