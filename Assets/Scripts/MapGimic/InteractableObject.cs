using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractableObject : MonoBehaviour
{
    [HideInInspector]
    public InteractableType type;
    public bool canInteract;
}

public enum InteractableType
{
    ClockWork,
    Carrried
}