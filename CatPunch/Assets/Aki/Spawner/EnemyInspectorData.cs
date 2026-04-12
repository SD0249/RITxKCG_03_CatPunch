using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵の生成種類
/// </summary>
public enum SpawnerType
{
    RAT,
    BIRD
}

/// <summary>
/// 敵生成のデータ
/// </summary>
[System.Serializable]
public class EnemySpawnData
{
    public GameObject Prefab;

    public List<RatPrefab> RatPrefabs;

    public int MaxSpawnCount;

    public SpawnerType Type;
}

/// <summary>
/// ネズミのプレハブ管理
/// </summary>
[System.Serializable]
public class RatPrefab
{
    public GameObject Prefab;

    public float Probability;
}