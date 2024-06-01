using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class CameraContoller : MonoBehaviour
{
    public GameObject camera;

    void Update()
    {
        Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        float yaw = thumbstickInput.x * Time.deltaTime * rotationSpeed;
        camera.transform.Rotate(Vector3.up, yaw);

        float moveSpeed = 3.0f;
        float forwardMovement = thumbstickInput.y * Time.deltaTime * moveSpeed;
        camera.transform.Translate(Vector3.forward * forwardMovement);
    }
}
