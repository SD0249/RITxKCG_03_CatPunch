using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Mouse : MonoBehaviour, IDespawnNotifier
{
    // Information needed to agent stored as Field
    // public NavMeshAgent mouse;
    public float maxSpeed;
    public float rotationSpeed = 5.0f;
    public Animator mouseAnimator;
    public GameObject currentTarget;                 // This will be the transform of the closest cookie object to the mouse. Continuously pursues until it reaches it
    public int CookieCount;                                          

    public Vector3 startingPos;                     // After obtaining the cookie, they will return to this location.
    public MouseState currentState;

    // Needed for jumping
    public float jumpThreshold = 1.0f;  // This is the distance to trigger jump
    public float jumpNudge = 0.3f;
    private bool isJumping = false;

    public enum MouseState
    {
        Idle,  // Acts like a loading screen. When calculating the closest distance, and after returning to its starting position
        Seek,   // When mouse is moving - Jump is a subaction in idle
               // Even though mouse tries to avoid other mice, it can run into them (distance factor).  
               // Then they attempt to jump - probably testing needed - and avoid the other mouse.
        Return
    }

    // Despawning Event
    public event System.Action OnDespawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

    }

    void OnEnable() 
    {
        currentState = MouseState.Idle;
        startingPos = gameObject.transform.position;    // This will only work when despawning is just disabling the mouse
        if(MouseManager.Instance != null)
        MouseManager.Instance.RegisterMouse(this); 
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case MouseState.Idle:
                // Play Idle animation
                mouseAnimator.SetFloat("Speed", 0f);

                // Change State
                if (currentTarget == null)
                {
                    currentTarget = MouseManager.Instance.GetRandomTestSceneCookie();

                    // For actual in game
                    // currentTarget = StageManager.Instance.GetNearestCookie(gameObject.transform.position).gameObject;
                    currentState = MouseState.Seek;
                }
                else
                {
                    currentState = MouseState.Seek;
                }
                break;

            case MouseState.Seek:
                mouseAnimator.SetFloat("Speed", 2.0f);  // Mouse will start running

                // If another mouse is too close to this mouse
                HandleSeparationAndJump();

                // Mouse moves towards the cookie every frame
                MoveTowards(currentTarget.transform.position);

                // Reaching cookie reaction is handled by IsTrigger
                // This is just here for fall back, just in case!
                if (Vector3.Distance(transform.position, currentTarget.transform.position) < 0.3f)
                {
                    Debug.Log("Reached Cookie");
                    mouseAnimator.SetFloat("Speed", 0f);
                    currentTarget = null;        // Clear target
                    currentTarget.SetActive(false);
                    currentState = MouseState.Return;
                }
                break;

            case MouseState.Return:
                mouseAnimator.SetFloat("Speed", 2.0f);

                // If another mouse is too close to this mouse
                HandleSeparationAndJump();

                // Mouse returns to where it spawned
                MoveTowards(startingPos);

                // Check whether mouse returned (A little room of threshold - 0.3f can change if this feels not too close
                if (Vector3.Distance(transform.position, startingPos) < 0.3f)
                {
                    mouseAnimator.SetFloat("Speed", 0f);    // Should start playing idle
                    DespawnMouse();
                }
                break;
        }
    }

    private void MoveTowards(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        direction.y = 0;    // Keep it horizontal

        // Rotate Smoothly
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Move
        float step = maxSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect cookie
        if (other.CompareTag("Cookie"))
        {
            Debug.Log("Reached Cookie");

            GameObject cookie = currentTarget.gameObject;
            mouseAnimator.SetFloat("Speed", 0f);
            currentTarget = null;        // Clear target
            cookie.SetActive(false);
            currentState = MouseState.Return;
        }

        // Need to be implemented
        // Detect other mice for jump & Trigger jump
        // If we go with this method, then 2 collider is needed,
        // and it needs to be in OnTriggerStay... I don't think this will ever be triggered
    }
    
    void HandleSeparationAndJump()
    {
        foreach(Mouse otherMouse in MouseManager.Instance.allMice)
        {
            if (otherMouse == null ||otherMouse == this) continue;   // Don't jump over yourself!

            // Jump Logic
            float distance = Vector3.Distance(transform.position, otherMouse.transform.position);
            if(distance < jumpThreshold && !isJumping)
            {
                Debug.Log($"{name} jumping over {otherMouse.name}");

                // Trigger Jump animation
                isJumping = true;
                mouseAnimator.SetTrigger("Jump");

                // Small Nudge to visually avoid overlapping
                Vector3 nudgeDir = (transform.position - otherMouse.transform.position).normalized;
                transform.position += nudgeDir * jumpNudge;

                // Reset after animation duration
                StartCoroutine(ResetJump(0.3f));
            }
        }
    }

    private IEnumerator ResetJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        isJumping = false;
    }

    // Despawn Mouse with this method
    private async void DespawnMouse()
    {
        MouseManager.Instance.UnregisterMouse(this);
        await Task.Delay(3000); // 3000 ms = 3 seconds
        OnDespawn?.Invoke();
    }
    
}
