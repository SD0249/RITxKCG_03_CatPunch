using UnityEngine;

public class TestEnemyAki : MonoBehaviour, IDespawnNotifier
{
    public event System.Action OnDespawn;

    private LimitTimer despawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        despawnTimer = new LimitTimer(1f);
        despawnTimer.OnFinished += () =>
        {
            OnDespawn?.Invoke();

            // setactive‚ðfalse
            gameObject.SetActive(false);
        };
        despawnTimer.Start();
    }

    private void FixedUpdate()
    {
        despawnTimer.FixedUpdate(Time.fixedDeltaTime);
    }

    void OnEnable()
    {
        if (despawnTimer != null)
        {
            despawnTimer.Start();
        }
    }
}
