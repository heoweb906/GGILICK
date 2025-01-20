using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    
    public float rotationAmount; // Amount of rotation
    public float rotationSpeed; // Rotation speed

    private Quaternion originalRotation;
    private Camera_PlayerFollow camera_PlayerFollow;
    private Camera_MaxMinFollow camera_MaxMinFollow; 

    private void Start()
    {
        camera_PlayerFollow = GetComponent<Camera_PlayerFollow>();
        camera_MaxMinFollow = GetComponent<Camera_MaxMinFollow>();
        // �ʱ� �����̼� ����

        if(camera_PlayerFollow != null) originalRotation = Quaternion.Euler(camera_PlayerFollow.rotationOffset);
        else if(camera_MaxMinFollow != null) originalRotation = Quaternion.Euler(camera_MaxMinFollow.rotationOffset);
    }

    void FixedUpdate()
    {
        // Perlin Noise�� ����� ��鸲 �� ���
        float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0) - 0.5f) * 2 * rotationAmount;
        float yRotation = (Mathf.PerlinNoise(0, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

        // ���� �����̼ǿ� ��鸲 ȿ�� �߰�
        Quaternion shakeRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.localRotation = originalRotation * shakeRotation;
    }
}
