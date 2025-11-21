using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Limits")]
    public float minCameraX = -60f;
    public float maxCameraX = 60f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity   = -9.81f;

    [Header("Jump Settings")]
    public float jumpForce     = 8f;
    public float jumpCooldown  = 1f;
    private float nextJumpTime = 0f;

    [Header("Camera Look (Mouse)")]
    public Transform cameraTransform;
    public float lookSpeed = 80f;
    private float xRotation = 0f;

    [Header("Animation")]
    public Animator anim;          // drag the cat Animator here in Inspector

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
            xRotation = cameraTransform.localEulerAngles.x;


        // If you forget to drag the Animator, try to grab it from children
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        // Lock & hide mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
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

        float rawSpeed = move.magnitude;

        // 0 = idle, 1 = full speed
        float animSpeed = Mathf.Clamp01(rawSpeed);
        if (anim != null)
        {
            // This is the parameter your Kitty blend tree uses
            anim.SetFloat("Vert", animSpeed);
            anim.SetFloat("State", 0f);  // 0 = walk, change to 1f if you add run
        }

        if (rawSpeed > 1f)
            move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void RotateCameraWithMouse()
    {
        // float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        // float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

        // xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

        // cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // transform.Rotate(Vector3.up * mouseX);

        float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

    // Modify X rotation
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

    // Keep the existing Y and Z rotation
    Vector3 current = cameraTransform.localEulerAngles;
    cameraTransform.localRotation = Quaternion.Euler(xRotation, current.y, current.z);

    // Rotate player horizontally
    transform.Rotate(Vector3.up * mouseX);

    }

    void HandleJump()
    {
        if (Time.time >= nextJumpTime && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpForce;
            nextJumpTime = Time.time + jumpCooldown;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
