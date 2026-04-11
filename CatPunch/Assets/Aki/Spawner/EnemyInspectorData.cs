using UnityEngine;

/// <summary>
/// “G‚Ì¶¬í—Ş
/// </summary>
public enum SpawnerType
{
    RAT,
    BIRD
}

/// <summary>
/// “G¶¬‚Ìƒf[ƒ^
/// </summary>
[System.Serializable]
public class EnemySpawnData
{
    public GameObject Prefab;

    public float BaseSpawnInterval;

    public int MaxSpawnCount;

    public SpawnerType Type;
}