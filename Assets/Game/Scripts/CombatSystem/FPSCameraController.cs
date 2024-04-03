using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    public float lookSensitivity = 0.1f;
    public float maxYAngle = 80f;
    private Vector2 currentLookRotation;
    private Transform cameraTransform;

    private PlayerInputActions playerInputActions;
    private Vector2 lookInput;

    private void Awake()
    {
        cameraTransform = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().transform;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Update()
    {
        // Apply the look input to the current rotation, considering the sensitivity
        currentLookRotation.x += lookInput.x * lookSensitivity;
        currentLookRotation.y -= lookInput.y * lookSensitivity;

        // Clamp the vertical rotation
        currentLookRotation.y = Mathf.Clamp(currentLookRotation.y, -maxYAngle, maxYAngle);

        // Apply the rotation to the camera for up/down look
        cameraTransform.localEulerAngles = new Vector3(currentLookRotation.y, 0, 0);

        // Apply the rotation to the player for left/right look
        transform.eulerAngles = new Vector2(0, currentLookRotation.x);
    }
}
