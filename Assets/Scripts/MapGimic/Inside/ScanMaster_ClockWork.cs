using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanMaster_ClockWork : InteractableObject
{
    public ScanMaster scanMaster;

    private void Start()
    {
        type = InteractableType.SingleEvent;
    }

    public override void ActiveEvent()
    {
        canInteract = false;

        scanMaster.ScanStart();

    }


}
