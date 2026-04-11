using UnityEngine;

/// <summary>
/// 生成ルールのインターフェース
/// </summary>
public interface ISpawnRule
{
    /// <summary>
    /// 生成間隔を取得する
    /// </summary>
    /// <returns>生成間隔</returns>
    float GetNextSpawnInterval();
}

/// <summary>
/// ネズミの生成ルール(未完成)
/// </summary>
public class RatSpawnRule : ISpawnRule
{
    private float spawnInterval;

    public RatSpawnRule(float spawnInterval)
    {
        this.spawnInterval = spawnInterval;
    }

    public float GetNextSpawnInterval()
    {
        return spawnInterval;
    }
}

/// <summary>
/// 鳥の生成ルール(未完成)
/// </summary>
public class BirdSpawnRule : ISpawnRule
{
    private float spawnInterval;
    
    public BirdSpawnRule(float spawnInterval)
    {
        this.spawnInterval = spawnInterval;
    }

    public float GetNextSpawnInterval()
    {
        return spawnInterval;
    }
}