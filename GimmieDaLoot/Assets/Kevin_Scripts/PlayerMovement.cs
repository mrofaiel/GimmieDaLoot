
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Limits")]
    public float minCameraX = 10f;
    public float maxCameraX = 15f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;

    [Header("Jump Settings")]
    public float jumpForce = 8f;       // how strong the jump is
    public float jumpCooldown = 1f;    // one jump every second
    private float nextJumpTime = 0f;   // cooldown timer

    [Header("Camera Look (Arrow Keys)")]
    public Transform cameraTransform;
    public float lookSpeed = 80f;
    private float xRotation = 0f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        RotateCameraWithMouse();
        MoveWithWASD();
        HandleJump();
        ApplyGravity();
    }

    void MoveWithWASD()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void RotateCameraWithMouse()
{
    float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

    // vertical rotation (camera)
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

    cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    // horizontal rotation (player body)
    transform.Rotate(Vector3.up * mouseX);
}


    void HandleJump()
    {
        // only jump if cooldown is done
        if (Time.time >= nextJumpTime && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;            // BASIC JUMP
            nextJumpTime = Time.time + jumpCooldown; 
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
