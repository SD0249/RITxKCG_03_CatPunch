using UnityEngine;
using UnityEngine.Events;

public class LimitTimer
{

    public float timeLimit { get; private set; }

    public float currentTime { get; private set; }

    public bool isRunning { get; private set; }

    public bool IsFinished => currentTime <= 0.0f;

    public event UnityAction OnFinished;

    // constructor
    public LimitTimer(float limit)
    {
        timeLimit = limit;

        currentTime = timeLimit;

        isRunning = false;
    }

    public void Start()
    {
        currentTime = timeLimit;

        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void FixedUpdate(float deltaTime)
    {
        if (!isRunning || IsFinished)
        {
            return;
        }

        currentTime -= deltaTime;

        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;

            isRunning = false;

            OnFinished?.Invoke();
        }
    }
}
