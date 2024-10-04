using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    public float rotationAmount = 0.2f; // Amount of rotation
    public float rotationSpeed = 1.0f; // Rotation speed

    public Vector3 originalRotation;

    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    void FixedUpdate()
    {
        float xRotation = Mathf.PerlinNoise(Time.time * rotationSpeed, 0) * rotationAmount;
        float yRotation = Mathf.PerlinNoise(0, Time.time * rotationSpeed) * rotationAmount;

        transform.localEulerAngles = new Vector3(
            originalRotation.x + xRotation,
            originalRotation.y + yRotation,
            originalRotation.z
        );
    }
}
