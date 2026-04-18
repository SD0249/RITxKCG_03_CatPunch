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
    private float attackRange = 10f;
    private bool isSprinting = false;
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
    [SerializeField] private GameObject cat;

    // Punch Impact UI
    public Image punch_Eng;
    public Image punch_Jpn;
    private int displayCount;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // locks and hides cursor
        Cursor.visible = false;
        catAnimation.SetBool("Idle", true);

        // Set impact active to false
        punch_Eng.gameObject.SetActive(false);
        punch_Jpn.gameObject.SetActive(false);
        displayCount = 0;
    }

    //receiving input to move
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        
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
        //saves the initial position of the head
        float originalHeadHeight = head.transform.localPosition.y;
        if (context.performed && characterController.isGrounded)
        {
            //halving the height of the head to new crouch position
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, originalHeadHeight/0.5f, head.transform.localPosition.z);
            speed = 2.5f; // Reduce speed when crouching
        }
        else if (context.canceled)
        {
            //multiplying head height by 2 to return to original position
            head.transform.localPosition = new Vector3(head.transform.localPosition.x, originalHeadHeight/2, head.transform.localPosition.z);
            speed = 5f; // Reset speed when standing up
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //plays punch animation
            catAnimation.SetBool("Idle", false);
            Debug.Log("Idle is set to " + catAnimation.GetBool("Idle"));
            catAnimation.SetTrigger("Punch");

            //checks each enemy hit in attack range
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log($"Hit {enemy.name}");
                
                if(enemy.gameObject.TryGetComponent<Mouse>(out Mouse mouse))
                {
                    mouse.OnHit(this.gameObject.transform.position);
                }
                else if(enemy.gameObject.TryGetComponent<Bird>(out Bird bird))
                {
                    bird.HitPunch();
                }
            }

            catAnimation.SetBool("Idle", true);
            Debug.Log("Idle is set to " + catAnimation.GetBool("Idle"));

            // Display Punch Impact UI - Start Coroutine
            displayCount++;
            StartCoroutine(DisplayPunchImpact());
            
        }
    }

    private IEnumerator DisplayPunchImpact()
    {
        // when display count is even
        if(displayCount % 2 == 0)
        {
            punch_Jpn.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            punch_Jpn.gameObject.SetActive(false);
        }
        // when display count is odd
        else
        {
            punch_Eng.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            punch_Eng.gameObject.SetActive(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !tired)
        {
            isSprinting = true;
        }
        else if (context.canceled)
        {
            isSprinting = false;
            speed = 5f; //resetting speed
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
        //getting camera position
        forward = fpCam.transform.forward;
        right = fpCam.transform.right;
        up = fpCam.transform.up;

        forward.Normalize();
        right.Normalize();
        up.Normalize();

        //checking if sprinting and updating speed and stamina accordingly
        if (isSprinting)
        {
            speed = 10f; //doubling base speed
            stamina -= Time.deltaTime * 10f; // Decrease stamina while sprinting
            SetStamina(stamina);
            //when running out of energy
            if (stamina <= 0)
            {
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

        //making cat always face camera
        transform.rotation = fpCam.transform.rotation;        
        //transform.rotation = Quaternion.Slerp(transform.rotation, fpCam.transform.rotation, Time.deltaTime);        
        //cat.transform.rotation = Quaternion.Slerp(transform.rotation, fpCam.transform.rotation, Time.deltaTime);        
    }
}
