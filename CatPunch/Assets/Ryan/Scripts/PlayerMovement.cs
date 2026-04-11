using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private bool grounded = true;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;

    //movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    //ground check
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkRadius = 0.2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move Input: {moveInput}");
        Debug.Log("Received input");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jumping: {context.performed} - Is Grounded: {characterController.isGrounded}");
        if (context.performed && characterController.isGrounded)
        {
            Debug.Log("Jump initiated");
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
