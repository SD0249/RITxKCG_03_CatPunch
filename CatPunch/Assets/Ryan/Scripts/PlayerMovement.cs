using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody rb;
    public int forceMagnitude = 1;
    private float speed;
    private bool grounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, forceMagnitude, 0), ForceMode.VelocityChange);
        while (grounded)
        {
            rb.linearDamping = 5;
        }
        while (!grounded)
        {
            rb.linearDamping = 0.5f;
        }
    }
}
