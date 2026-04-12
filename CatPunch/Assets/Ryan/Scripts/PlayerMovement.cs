using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Switch;
using UnityEngine.Rendering;
using UnityEngine.UI;
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
    private bool isSprinting = false;
    private bool isMoving = false;
    private bool tired = false;

    //movement
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private GameObject fpCam;
    [SerializeField] private GameObject head;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private Slider slider;
    [SerializeField] private Animator catAnimation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // locks and hides cursor
        Cursor.visible = false;
        catAnimation.SetBool("IsIdle", true);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = true;
        if (context.canceled)
        {
            isMoving = false;
        }
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
        float originalHeadHeight = head.transform.localPosition.y;
        if (context.performed && characterController.isGrounded)
        {
            
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, originalHeadHeight/0.5f, head.transform.localPosition.z);
            speed = 2.5f; // Reduce speed when crouching
        }
        else if (context.canceled)
        {
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, originalHeadHeight/2, head.transform.localPosition.z);
            speed = 5f; // Reset speed when standing up
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            catAnimation.SetTrigger("Punch");
            Debug.Log("Attack called!");
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log($"Hit {enemy.name}");
                // Implement damage logic here
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !tired)
        {
            Debug.Log("Sprinting!");
            isSprinting = true;
        }
        else if (context.canceled)
        {
            Debug.Log("Stopped sprinting!");
            isSprinting = false;
            speed = 5f;
            StartCoroutine(RegenerateStamina());
        }
    }

    public void SetStamina(float staminaValue)
    {
        slider.value = staminaValue; // Update the slider UI
    }

    private IEnumerator RegenerateStamina()
    {
        while (stamina < 100f)
        {
            stamina += Time.deltaTime * 20f; // Regenerate stamina over time
            SetStamina(stamina); // Update the slider UI
            yield return null;
        }
        stamina = 100f;
        SetStamina(stamina); // Ensure slider is full
        tired = false;
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

        if (isSprinting)
        {
            catAnimation.SetBool("IsSprinting", true);
            speed = 10f;
            Debug.Log($"Current stamina: {stamina}");
            stamina -= Time.deltaTime * 10f; // Decrease stamina while sprinting
            SetStamina(stamina);
            if (stamina <= 0)
            {
                catAnimation.SetBool("IsSprinting", false);
                tired = true;
                isSprinting = false;
                stamina = 0;
                speed = 5f;
                StartCoroutine(RegenerateStamina());
            }
        }

        Vector3 move = (forward * moveInput.y) + (right * moveInput.x);
        characterController.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        transform.rotation = fpCam.transform.rotation;

        //if (isMoving)
        //{
        //    catAnimation.SetBool("IsWalking", true);
        //}
        //else if (!isMoving)
        //{
        //    catAnimation.SetBool("IsIdle", true);
        //}
        //else if (isSprinting)
        //{
        //    catAnimation.SetBool("IsSprinting", true);
        //}
        
        
        
    }
}
