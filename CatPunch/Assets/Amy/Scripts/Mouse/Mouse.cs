using UnityEngine;
using UnityEngine.AI;

public class Mouse : MonoBehaviour, IDespawnNotifier
{
    // Information needed to agent stored as Field
    public NavMeshAgent mouse;
    public Animator mouseAnimator;
    public Transform currentTarget;                 // This should later be cookie object

    // Despawning Event
    public event System.Action OnDespawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (currentTarget != null)
        {
            // NavMesh handles pathfinding
            mouse.SetDestination(currentTarget.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ** Testing Purpose
        // Update Animator speed
        float speed = mouse.velocity.magnitude;
        mouseAnimator.SetFloat("Speed", speed);
    }



    // Despawn Mouse with this method
    public void DespawnMouse()
    {
        OnDespawn?.Invoke();
    }
    
}
