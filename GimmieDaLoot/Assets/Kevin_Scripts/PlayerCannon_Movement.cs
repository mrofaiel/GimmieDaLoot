using UnityEngine;

public class PlayerCannon_Movement : MonoBehaviour
{
    [Header("Movement Limits")]
    public float minCameraX = -60f;
    public float maxCameraX = 60f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float jumpCooldown = 1f;
    private float nextJumpTime = 0f;

    [Header("Camera Look (Mouse)")]
    public Transform cameraTransform;
    public float lookSpeed = 80f;
    private float xRotation = 0f;

    [Header("Animation (Optional)")]
    public Animator anim;   // optional

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            Debug.LogError("PlayerMovement needs a CharacterController on the same GameObject!");

        if (cameraTransform == null)
            Debug.LogError("PlayerMovement needs Camera Transform assigned in Inspector!");

        // Initialize pitch from current camera rotation (so it doesn't snap)
        if (cameraTransform != null)
            xRotation = cameraTransform.localEulerAngles.x;

        // Optional animator auto-grab
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (controller == null || cameraTransform == null) return;

        RotateCameraWithMouse();
        MoveWithWASD_CameraRelative();
        HandleJump();
        ApplyGravity();
    }

    void MoveWithWASD_CameraRelative()
    {
        // Camera-relative directions (flattened so you don't fly up/down)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += camForward;
        if (Input.GetKey(KeyCode.S)) move -= camForward;
        if (Input.GetKey(KeyCode.A)) move -= camRight;
        if (Input.GetKey(KeyCode.D)) move += camRight;

        float rawSpeed = move.magnitude;

        // Optional animation (safe if you don’t use it)
        if (anim != null)
        {
            float animSpeed = Mathf.Clamp01(rawSpeed);
            anim.SetFloat("Vert", animSpeed);
            anim.SetFloat("State", 0f);
        }

        if (rawSpeed > 1f)
            move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void RotateCameraWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

        // Pitch camera up/down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

        // Only set X for camera pitch
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Yaw the player/cannon left/right
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        // Only jump if grounded + cooldown
        if (controller.isGrounded && Time.time >= nextJumpTime && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            nextJumpTime = Time.time + jumpCooldown;
        }
    }

    void ApplyGravity()
    {
        // Keep grounded stick so you don't “float”
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
