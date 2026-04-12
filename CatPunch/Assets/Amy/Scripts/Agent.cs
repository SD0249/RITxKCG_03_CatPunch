using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    [SerializeField]
    Rigidbody objectRb;

    Vector3 velocity, acceleration, steeringForce;

    Quaternion previousRotation;
    Quaternion nextRotation;

    [SerializeField]
    protected float maxSpeed;
    
    public Vector3 Velocity { get { return velocity; } }

    private void Awake()
    {
        objectRb = GetComponent<Rigidbody>();        
        if(objectRb == null)
        {
            Debug.LogError($"No Rigidbody!");
        }
        nextRotation = objectRb.rotation;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // Save previous rotation for smooth rotational movement
        previousRotation = nextRotation;

        // Every Frame recalculate acceleration
        acceleration = Vector3.zero;
        steeringForce = CalculateSteering();
        acceleration += steeringForce;

        // Update Velocity according to acceleration
        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Look towards the direction the agent is moving
        if (velocity.magnitude > 0)
        {
            nextRotation = Quaternion.LookRotation(velocity, Vector3.up);
            nextRotation = Quaternion.Slerp(previousRotation, nextRotation, Time.fixedDeltaTime);
        }

        // Update Position and Rotation accordingly
        if(objectRb != null)
        {
            objectRb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
            objectRb.MoveRotation(nextRotation);
        }
        else
        {
            Debug.Log("Rigidbody not assigned!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract Vector3 CalculateSteering();

    public Vector3 GetFuturePosition(float TimeInSeconds)
    {
        return transform.position + velocity * TimeInSeconds;
    }

    protected Vector3 Seek(Vector3 targetPosition)
    {
        // 1. Calculate desired velocity direction
        Vector3 desiredVelocity = targetPosition - transform.position;

        // 2. Scale the desiredVelocity by max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // 3. Calculate the resulting steering force to make current velocity into desired velocity
        Vector3 seekForce = desiredVelocity - velocity;

        // 4. Return the steering force
        return seekForce;
    }

    // Wrapper to call seek outside
    protected Vector3 Seek(GameObject targetObject)
    {
        return Seek(targetObject.transform.position);
    }
}
