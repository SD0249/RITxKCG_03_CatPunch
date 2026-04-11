using UnityEngine;

public class SpawnRuntime
{
    public EnemySpawnData Data;

    public float Timer;

    public float NextInterval;

    public int ActiveCount;

    public ISpawnRule Rule;
}
