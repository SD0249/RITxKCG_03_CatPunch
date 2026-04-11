using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector3 forward;
    private Vector3 right;
    private Vector3 up;

    //movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private GameObject fpCam;

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

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        transform.Rotate(0, lookInput.x * Time.deltaTime * 100f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forward = fpCam.transform.forward;
        right = fpCam.transform.right;
        up = fpCam.transform.up;

        forward.Normalize();
        right.Normalize();
        up.Normalize();

        //Vector3 move = new Vector3(moveInput.x * forward.x, 0, moveInput.y);
        Vector3 move = (forward * moveInput.y) + (right * moveInput.x);
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
