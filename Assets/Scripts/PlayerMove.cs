using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed = 8f;
    public float acceleration = 20f;
    public float friction = 8f;
    
    private float mouseSensitivity;
    private const string MOUSE_SENS_KEY = "MouseSensitivity";
    
    private CharacterController controller;
    private float verticalVelocity;
    private float rotationX = 0f;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENS_KEY, 2f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveDirection = Vector3.zero;
    }

    void Update()
    {
        // Horizontal mouse look only
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX += mouseX;
        transform.rotation = Quaternion.Euler(0, rotationX, 0);

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        
        // Calculate wish direction based on input and player rotation
        Vector3 wishDirection = transform.right * moveX + transform.forward * moveZ;
        wishDirection = wishDirection.normalized;
        
        // Doom-style acceleration
        if (wishDirection.magnitude > 0)
        {
            // Add acceleration in wish direction
            moveDirection += wishDirection * acceleration * Time.deltaTime;
            
            // Clamp to max speed
            if (moveDirection.magnitude > maxSpeed)
                moveDirection = moveDirection.normalized * maxSpeed;
        }
        else
        {
            // Apply friction/deceleration
            moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, friction * Time.deltaTime);
            
            // Snap to zero when very slow
            if (moveDirection.magnitude < 0.1f)
                moveDirection = Vector3.zero;
        }
        
        // Apply horizontal movement
        Vector3 move = moveDirection;
        
        // Gravity
        if (controller.isGrounded)
            verticalVelocity = -0.1f;
        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }
}