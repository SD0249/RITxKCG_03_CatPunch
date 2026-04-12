using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
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

    /// <summary>
    /// 生成対数を取得する
    /// </summary>
    /// <returns>生成対数</returns>
    int GetNextSpawnNum();
}

/// <summary>
/// ネズミの生成ルール(未完成)
/// </summary>
public class RatSpawnRule : ISpawnRule
{
    RatSpawnValue spawnValue;

    public RatSpawnRule()
    {
        spawnValue = new RatSpawnValue();
    }

    public float GetNextSpawnInterval()
    {
        float nextInterval;

        var timer = StageManager.Instance.Timer;

        float limit = timer.timeLimit - timer.currentTime;

        if (limit < spawnValue.SecondLevelTime)
        {
            nextInterval = Random.Range(spawnValue.FirstLevelMinInterval,spawnValue.FirstLevelMaxInterval);
        }
        else if (limit < spawnValue.ThirdLevelTime)
        {
            nextInterval = Random.Range(spawnValue.SecondLevelMinInterval,spawnValue.SecondLevelMaxInterval);
        }
        else
        {
            nextInterval = Random.Range(spawnValue.ThirdLevelMinInterval,spawnValue.ThirdLevelMaxInterval);
        }

        return nextInterval;
    }

    public int GetNextSpawnNum()
    {
        int num;

        var timer = StageManager.Instance.Timer;

        float limit = timer.timeLimit - timer.currentTime;

        if (limit < spawnValue.SecondLevelTime)
        {
            num = Random.Range(1,spawnValue.FirstLevelMaxSpawnNum + 1);
        }
        else if (limit < spawnValue.ThirdLevelTime)
        {
            num = Random.Range(1, spawnValue.SecondLevelMaxSpawnNum + 1);
        }
        else
        {
            num = Random.Range(1, spawnValue.ThirdLevelMaxSpawnNum + 1);
        }

        return num;
    }
}

/// <summary>
/// 鳥の生成ルール(未完成)
/// </summary>
public class BirdSpawnRule : ISpawnRule
{
    private float baseSpawnInterval = 8.0f;

    private float spawnIntervalRange = 1.0f;

    private float minSpawnInterval = 2.0f;

    public float GetNextSpawnInterval()
    {
        // 鳥が取ったクッキー数*0.5だけ間隔を減らす
        var seconds = baseSpawnInterval - (StageManager.Instance.GetBirdStoleNum() * 0.5f);

        // 最低値保障
        if (seconds < minSpawnInterval)
        {
            seconds = minSpawnInterval;
        }

        // 時間計算
        return Random.Range(seconds, seconds + spawnIntervalRange);
    }

    public int GetNextSpawnNum()
    {
        return 1;
    }
}

/// <summary>
/// ネズミの生成変数(めっちゃキモいいつか直す)
/// </summary>
public class RatSpawnValue
{
    public float FirstLevelMinInterval { get; private set; }

    public float FirstLevelMaxInterval { get; private set; }

    public int FirstLevelMaxSpawnNum {  get; private set; }

    public float SecondLevelTime { get; private set; }

    public float SecondLevelMinInterval { get; private set; }

    public float SecondLevelMaxInterval { get; private set; }

    public int SecondLevelMaxSpawnNum { get; private set; }

    public float ThirdLevelTime { get; private set; }

    public float ThirdLevelMinInterval { get; private set; }

    public float ThirdLevelMaxInterval { get; private set; }

    public int ThirdLevelMaxSpawnNum { get; private set; }

    public RatSpawnValue()
    {
        FirstLevelMinInterval = 3.0f;
        FirstLevelMaxInterval = 4.0f;
        FirstLevelMaxSpawnNum = 3;

        SecondLevelTime = 15.0f;
        SecondLevelMinInterval = 2.0f;
        SecondLevelMaxInterval = 3.0f;
        SecondLevelMaxSpawnNum = 4;

        ThirdLevelTime = 40.0f;
        ThirdLevelMinInterval = 1.0f;
        ThirdLevelMaxInterval = 2.0f;
        ThirdLevelMaxSpawnNum = 5;
    }
}