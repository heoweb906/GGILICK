using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
    
    public float rotationAmount; // Amount of rotation
    public float rotationSpeed; // Rotation speed

    private Quaternion originalRotation;
<<<<<<< Updated upstream
    private Camera_PlayerFollow camera_PlayerFollow;
    private Camera_MaxMinFollow camera_MaxMinFollow; 

    private void Start()
    {
        camera_PlayerFollow = GetComponent<Camera_PlayerFollow>();
        camera_MaxMinFollow = GetComponent<Camera_MaxMinFollow>();
        // ì´ˆê¸° ë¡œí…Œì´ì…˜ ì €ìž¥

        if(camera_PlayerFollow != null) originalRotation = Quaternion.Euler(camera_PlayerFollow.rotationOffset);
        else if(camera_MaxMinFollow != null) originalRotation = Quaternion.Euler(camera_MaxMinFollow.rotationOffset);
=======

    private void Start()
    {
        // ÃÊ±â ·ÎÅ×ÀÌ¼Ç ÀúÀå
        originalRotation = transform.localRotation;
>>>>>>> Stashed changes
    }

    void FixedUpdate()
    {
<<<<<<< Updated upstream
        // Perlin Noiseë¥¼ ì‚¬ìš©í•´ í”ë“¤ë¦¼ ê°’ ê³„ì‚°
        float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0) - 0.5f) * 2 * rotationAmount;
        float yRotation = (Mathf.PerlinNoise(0, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

        // í˜„ìž¬ ë¡œí…Œì´ì…˜ì— í”ë“¤ë¦¼ íš¨ê³¼ ì¶”ê°€
=======
        // Perlin Noise¸¦ »ç¿ëÇØ Èçµé¸² °ª °è»ê
        float xRotation = (Mathf.PerlinNoise(Time.time * rotationSpeed, 0) - 0.5f) * 2 * rotationAmount;
        float yRotation = (Mathf.PerlinNoise(0, Time.time * rotationSpeed) - 0.5f) * 2 * rotationAmount;

        // ÇöÀç ·ÎÅ×ÀÌ¼Ç¿¡ Èçµé¸² È¿°ú Ãß°¡
>>>>>>> Stashed changes
        Quaternion shakeRotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.localRotation = originalRotation * shakeRotation;
    }
}
