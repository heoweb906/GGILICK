using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // �� ��Ʈ����Ʈ�� �߰��ؾ� Inspector���� �νĵ˴ϴ�.
public class Offset
{
    public float fPositionOffest;
    public Vector3 lookAtOffset;
}

public class DollyRotationAndPositonOffset : MonoBehaviour
{
    public Offset[] Offsets;
}
