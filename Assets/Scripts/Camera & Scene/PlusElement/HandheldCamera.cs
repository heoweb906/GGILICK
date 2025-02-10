using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    public float rotationAmount; // Amount of rotation
    public float rotationSpeed; // Rotation speed

    private Quaternion originalRotation;

    private void Start()
    {
        // 초기 로테이션 저장
        originalRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        // Perlin Noise를 사용해 흔들림 값 계산
        float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0) - 0.5f) * 2 * rotationAmount;
        float yRotation = (Mathf.PerlinNoise(0, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

        // 현재 로테이션에 흔들림 효과 추가
        Quaternion shakeRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.localRotation = originalRotation * shakeRotation;
    }
}
