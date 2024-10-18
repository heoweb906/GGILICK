using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_SimpleFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public Vector3 offset; // Camera offset
    public float followSpeed = 0.1f; // Speed of camera follow

    public Vector3 maxWorldPosition; // Maximum world coordinates
    public Vector3 minWorldPosition; // Minimum world coordinates

    public float threshold; // Movement threshold for camera to respond

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Clamp the desired position within the defined limits
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minWorldPosition.x, maxWorldPosition.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minWorldPosition.y, maxWorldPosition.y);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, minWorldPosition.z, maxWorldPosition.z);

            // Calculate the difference in position
            Vector3 positionDifference = desiredPosition - transform.position;

            // Move only if the difference exceeds the threshold
            if (positionDifference.magnitude > threshold)
            {
                // Use DOTween to smoothly move the camera
                transform.DOMove(desiredPosition, followSpeed).SetEase(Ease.OutQuad);
            }
        }
    }
}