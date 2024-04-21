using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler playerInputHandler;

    [Header("Movement Speeds")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float lookSpeed = 50.0f;
    [SerializeField] private Transform cameraTransform;

    [Header("Rotation Sensitivity")]
    private float verticalRotation = 0.0f;
    [SerializeField] private float upDownRange = 80.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6.0f; // Jump force added
    private bool isGrounded = true; // To check if the player is grounded

    [Space(5)]
    [SerializeField] private CameraAnimationManager cameraAnimationManager;
    [SerializeField] private bool activeCameraMovementAnimation;

    [Header("Components")]
    [SerializeField] private Rigidbody rb; // Rigidbody reference

    private void Start()
    {
        rb.GetComponent<Rigidbody>();

        isGrounded = true;

        activeCameraMovementAnimation = false;
    }

    void Update()
    {
        if (playerInputHandler)
        {
            // Movement
            Movement();

            // Look rotation
            Rotation();

            // Jump
            if (playerInputHandler.JumpTriggered)
            {
                Jump();
            }

        }
    }

    // Collisions
    private void OnCollisionEnter(Collision other)
    {
        // Check if the player has collided with the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    // -----------

    private void Movement()
    {
        // Calculate the direction based on camera orientation and input
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0; // Zero out the y component to keep the movement horizontal
        right.y = 0; // Zero out the y component to keep the movement horizontal
        forward.Normalize(); // Normalize to ensure consistent movement speed
        right.Normalize(); // Normalize to ensure consistent movement speed

        // Calculate the desired move direction relative to the camera
        Vector3 moveDirection = (forward * playerInputHandler.MoveInput.y + right * playerInputHandler.MoveInput.x);

        // Move the player
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);

        // camera animation based on player's movement
        if (!activeCameraMovementAnimation && moveDirection != Vector3.zero)
        {
            cameraAnimationManager.EnableCameraMovementAnimation();
            activeCameraMovementAnimation = true;
            //Debug.Log("Camera Movement Enable");
        }
        if (activeCameraMovementAnimation && moveDirection == Vector3.zero)
        {
            cameraAnimationManager.DisableCameraMovementAnimation();
            activeCameraMovementAnimation = false;
            //Debug.Log("Camera Movement Disable");
        }
    }

    private void Rotation()
    {
        // Look rotation
        Vector2 look = lookSpeed * Time.deltaTime * playerInputHandler.LookInput;
        transform.Rotate(0, look.x, 0);  // Horizontal rotation

        // Apply vertical rotation
        verticalRotation -= look.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cameraTransform.localEulerAngles = new Vector3(verticalRotation, cameraTransform.localEulerAngles.y, 0);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}