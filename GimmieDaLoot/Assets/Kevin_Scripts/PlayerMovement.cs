
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {

//     [Header("Movement Limits")]
//     public float minCameraX = 10f;   // lowest downward rotation allowed
//     public float maxCameraX = 15f;  // highest upward rotation allowed (RENAMED so clamp works)



//     [Header("Movement")]
//     public float moveSpeed = 6f;
//     public float gravity = -9.81f;

//     [Header("Camera Look (Arrow Keys)")]
//     public Transform cameraTransform;
//     public float lookSpeed = 80f;      // how fast camera turns with arrow keys
//     private float xRotation = 0f;

//     private CharacterController controller;
//     private Vector3 velocity;

//     void Start()
//     {
//         controller = GetComponent<CharacterController>();

//         // We’re not using the mouse anymore, so no need to lock it
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }

//     void Update()
//     {
//         RotateCameraWithArrows();
//         MoveWithWASD();
//         ApplyGravity();
//     }

//     void MoveWithWASD()
//     {
//         Vector3 move = Vector3.zero;

//         // Forward / Back
//         if (Input.GetKey(KeyCode.W)) move += transform.forward;
//         if (Input.GetKey(KeyCode.S)) move -= transform.forward;

//         // Left / Right
//         if (Input.GetKey(KeyCode.A)) move -= transform.right;
//         if (Input.GetKey(KeyCode.D)) move += transform.right;

//         if (move.sqrMagnitude > 1f)
//             move.Normalize();

//         controller.Move(move * moveSpeed * Time.deltaTime);
//     }

//     void RotateCameraWithArrows()
// {
//     float lookHorizontal = 0f;
//     float lookVertical = 0f;

//     if (Input.GetKey(KeyCode.RightArrow)) lookHorizontal = 1f;
//     if (Input.GetKey(KeyCode.LeftArrow))  lookHorizontal = -1f;

//     if (Input.GetKey(KeyCode.UpArrow))    lookVertical = 1f;
//     if (Input.GetKey(KeyCode.DownArrow))  lookVertical = -1f;

//     // vertical camera tilt (up/down)
//     xRotation -= lookVertical * lookSpeed * Time.deltaTime;

//     // clamp based on Inspector values  
//     xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

//     cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

//     // horizontal rotation (turn player)
//     transform.Rotate(Vector3.up * lookHorizontal * lookSpeed * Time.deltaTime);
// }


//     void ApplyGravity()
//     {
//         if (controller.isGrounded && velocity.y < 0)
//             velocity.y = -2f; // small downward force to keep grounded

//         velocity.y += gravity * Time.deltaTime;
//         controller.Move(velocity * Time.deltaTime);
//     }
// }

// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement Limits")]
//     public float minCameraX = 10f;   // lowest downward rotation allowed
//     public float maxCameraX = 15f;   // highest upward rotation allowed

//     [Header("Movement")]
//     public float moveSpeed = 6f;
//     public float gravity = -9.81f;
//     public float jumpHeight = 2f;    // NEW: how high the jump is

//     [Header("Camera Look (Arrow Keys)")]
//     public Transform cameraTransform;
//     public float lookSpeed = 80f;      // how fast camera turns with arrow keys
//     private float xRotation = 0f;

//     private CharacterController controller;
//     private Vector3 velocity;

//     void Start()
//     {
//         controller = GetComponent<CharacterController>();

//         // We’re not using the mouse anymore, so no need to lock it
//         Cursor.lockState = CursorLockMode.None;
//         Cursor.visible = true;
//     }

//     void Update()
//     {
//         RotateCameraWithArrows();
//         MoveWithWASD();
//         ApplyGravity();
//     }

//     void MoveWithWASD()
//     {
//         Vector3 move = Vector3.zero;

//         // Forward / Back
//         if (Input.GetKey(KeyCode.W)) move += transform.forward;
//         if (Input.GetKey(KeyCode.S)) move -= transform.forward;

//         // Left / Right
//         if (Input.GetKey(KeyCode.A)) move -= transform.right;
//         if (Input.GetKey(KeyCode.D)) move += transform.right;

//         if (move.sqrMagnitude > 1f)
//             move.Normalize();

//         controller.Move(move * moveSpeed * Time.deltaTime);
//     }

//     void RotateCameraWithArrows()
//     {
//         float lookHorizontal = 0f;
//         float lookVertical = 0f;

//         if (Input.GetKey(KeyCode.RightArrow)) lookHorizontal = 1f;
//         if (Input.GetKey(KeyCode.LeftArrow))  lookHorizontal = -1f;

//         if (Input.GetKey(KeyCode.UpArrow))    lookVertical = 1f;
//         if (Input.GetKey(KeyCode.DownArrow))  lookVertical = -1f;

//         // vertical camera tilt (up/down)
//         xRotation -= lookVertical * lookSpeed * Time.deltaTime;

//         // clamp based on Inspector values  
//         xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

//         cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

//         // horizontal rotation (turn player)
//         transform.Rotate(Vector3.up * lookHorizontal * lookSpeed * Time.deltaTime);
//     }

//     void ApplyGravity()
//     {
//         if (controller.isGrounded)
//         {
//             // keep a tiny downward force so we stay grounded
//             if (velocity.y < 0f)
//                 velocity.y = -2f;

//             // NEW: Jump when grounded and Space is pressed
//             if (Input.GetKeyDown(KeyCode.Space))
//             {
//                 // physics formula for initial jump velocity
//                 velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//             }
//         }

//         // gravity over time
//         velocity.y += gravity * Time.deltaTime;
//         controller.Move(velocity * Time.deltaTime);
//     }
// }



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
    }

    void Update()
    {
        RotateCameraWithArrows();
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

    void RotateCameraWithArrows()
    {
        float lookHorizontal = 0f;
        float lookVertical = 0f;

        if (Input.GetKey(KeyCode.RightArrow)) lookHorizontal = 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  lookHorizontal = -1f;

        if (Input.GetKey(KeyCode.UpArrow))    lookVertical = 1f;
        if (Input.GetKey(KeyCode.DownArrow))  lookVertical = -1f;

        xRotation -= lookVertical * lookSpeed * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, minCameraX, maxCameraX);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookHorizontal * lookSpeed * Time.deltaTime);
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
