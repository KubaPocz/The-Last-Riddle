using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private CharacterController characterController;

    #region Camera Movement Variables

    public Camera playerCamera;
    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #region Camera Zoom Variables
    public bool enableZoom = true;
    public bool holdToZoom = false;
    public KeyCode zoomKey = KeyCode.Mouse1;
    public float zoomFOV = 30f;
    public float zoomStepTime = 5f;
    private bool isZoomed = false;
    #endregion
    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;

    #region Jump
    public bool enableJump = true; // Mo¿esz w³¹czyæ/wy³¹czyæ skakanie w Unity
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    public bool isGrounded = false;
    private float verticalVelocity = 0f;
    public float gravity = 9.81f;
    #endregion

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera.fieldOfView = fov;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleMovement();
        HandleJump();
        HandleZoom();
    }

    private void LateUpdate()
    {
        HandleCameraRotation(); // Kamera w LateUpdate dla stabilnoœci
    }

    private void HandleCameraRotation()
    {
        if (cameraCanMove)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * mouseSensitivity;
            pitch -= (invertCamera ? -1 : 1) * mouseY * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        if (playerCanMove)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            moveDirection = move * walkSpeed;

            // Grawitacja
            if (characterController.isGrounded)
            {
                verticalVelocity = -gravity * Time.deltaTime;
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }

            moveDirection.y = verticalVelocity;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (enableJump && Input.GetKeyDown(jumpKey) && characterController.isGrounded)
        {
            verticalVelocity = jumpPower;
        }
    }

    private void HandleZoom()
    {
        if (enableZoom)
        {
            if (Input.GetKeyDown(zoomKey) && !holdToZoom)
            {
                isZoomed = !isZoomed;
            }

            if (holdToZoom)
            {
                isZoomed = Input.GetKey(zoomKey);
            }

            float targetFOV = isZoomed ? zoomFOV : fov;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, zoomStepTime * Time.deltaTime);
        }
    }
}
#endregion