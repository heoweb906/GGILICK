using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedObject : InteractableObject
{
    public PartOwnerType partOwnerType = PartOwnerType.Nothing;

    public Rigidbody rigid;
    public Collider col;

    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    private void Start()
    {
        type = InteractableType.Carrried;
        canInteract = true;
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
}
