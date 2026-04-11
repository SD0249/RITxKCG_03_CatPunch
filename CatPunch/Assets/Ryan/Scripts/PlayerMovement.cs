using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private Vector3 forward;
    private Vector3 right;
    private Vector3 up;
    private float attackRange = 1f;

    //movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private GameObject fpCam;
    [SerializeField] private GameObject head;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;

    //to visualize attack animation before animations are implemented
    [SerializeField] private GameObject paw;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // locks and hides cursor
        Cursor.visible = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");
        //Debug.Log("Received input");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log($"Jumping: {context.performed} - Is Grounded: {characterController.isGrounded}");
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

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, 0.5f, head.transform.localPosition.z);
            speed = 2.5f; // Reduce speed when crouching
        }
        else if (context.canceled)
        {
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, 1f, head.transform.localPosition.z);
            speed = 5f; // Reset speed when standing up
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Attack called!");
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log($"Hit {enemy.name}");
                // Implement damage logic here
            }

            StartCoroutine(PawAttack());
        }
    }

    private IEnumerator PawAttack()
    {
        forward = fpCam.transform.forward;
        right = fpCam.transform.right;
        up = fpCam.transform.up;

        forward.Normalize();
        right.Normalize();
        up.Normalize();

        paw.transform.localPosition += (forward * attackRange) + (right * attackRange); // move forward
        yield return new WaitForSeconds(0.2f);                   // wait
        paw.transform.localPosition = new Vector3(0.48f, 0.28f, 0.71f); // reset
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

        Vector3 move = (forward * moveInput.y) + (right * moveInput.x);
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, fpCam.transform.rotation, 0.1f * Time.deltaTime);
        transform.rotation = fpCam.transform.rotation;
    }
}
