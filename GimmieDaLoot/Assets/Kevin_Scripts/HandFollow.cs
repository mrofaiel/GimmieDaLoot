using UnityEngine;

public class HandFollow : MonoBehaviour
{
    [Header("Camera to follow")]
    public Transform cameraTransform;

    [Header("Offsets")]
    public Vector3 positionOffset = new Vector3(0.3f, -0.3f, 0.6f); 
    public Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

    void LateUpdate()
    {
        // Make the hand follow the camera
        transform.position = cameraTransform.position 
                             + cameraTransform.TransformDirection(positionOffset);

        // Rotate the hand with the camera
        transform.rotation = cameraTransform.rotation * Quaternion.Euler(rotationOffset);
    }
}

