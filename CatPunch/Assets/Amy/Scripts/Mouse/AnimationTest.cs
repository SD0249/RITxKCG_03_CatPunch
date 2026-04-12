using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationTest : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;
    public PlayerInput inputAction;

    private void OnEnable()
    {
        // inputAction.enabled = true;
    }

    void Update()
    {
        // Move forward automatically
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Update animator speed
        animator.SetFloat("Speed", speed);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Jump input fired!");
            animator.SetTrigger("Jump");
        }
    }
}

