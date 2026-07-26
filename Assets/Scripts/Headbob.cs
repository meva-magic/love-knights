using UnityEngine;

public class DoomHeadBob : MonoBehaviour
{
    public float bobSpeed = 10f;
    public float horizontalBobAmount = 0.03f;
    public float verticalBobAmount = 0.05f;
    
    private Vector3 startPosition;
    private float bobTimer;
    private PlayerMovement playerMovement;
    private CharacterController controller;

    void Start()
    {
        startPosition = transform.localPosition;
        playerMovement = GetComponentInParent<PlayerMovement>();
        controller = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (controller == null || playerMovement == null) return;
        
        // Get actual movement speed from CharacterController
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0;
        float currentSpeed = horizontalVelocity.magnitude;
        
        if (controller.isGrounded && currentSpeed > 0.1f)
        {
            // Bob based on actual distance traveled (like classic Doom)
            bobTimer += currentSpeed * Time.deltaTime * bobSpeed;
            
            // Doom uses sine for vertical and cosine for horizontal sway
            float verticalBob = Mathf.Sin(bobTimer * 2f) * verticalBobAmount;
            float horizontalBob = Mathf.Cos(bobTimer) * horizontalBobAmount;
            
            // Combined bob - Doom style bobbing goes down-left then up-right
            transform.localPosition = new Vector3(
                startPosition.x + horizontalBob,
                startPosition.y + verticalBob,
                startPosition.z
            );
        }
        else
        {
            // Return to center when stopped or in air
            bobTimer = 0;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition, 
                startPosition, 
                8f * Time.deltaTime
            );
        }
    }
}