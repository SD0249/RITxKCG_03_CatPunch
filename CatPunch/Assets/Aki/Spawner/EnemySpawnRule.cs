using System.Threading;
using UnityEngine;

/// <summary>
/// 生成ル?ルのイン??フェ?ス
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
/// ネズ?の生成ル?ル(未完成)
/// </summary>
public class RatSpawnRule : ISpawnRule
{
    RatData ratData;

    int levelNum;

    LimitTimer timer;

    public RatSpawnRule(RatData data)
    {
        timer = StageManager.Instance.Timer;

        ratData = data;

        levelNum = 0;
    }

    public float GetNextSpawnInterval()
    {
        UpdateLevel();

        // 現在のレベルに応じた生成間隔をランダムで取得(get next level time and compare with elapsed time)
        return Random.Range(ratData.SpawnDataArray[levelNum].MinInterval, ratData.SpawnDataArray[levelNum].MaxInterval);
    }

    public int GetNextSpawnNum()
    {
        UpdateLevel();

        // 現在のレベルに応じた生成対数をランダムで取得(get next level time and compare with elapsed time)
        return Random.Range(1, ratData.SpawnDataArray[levelNum].MaxSpawnNum + 1);
    }

    /// <summary>
    /// レベルの更新
    /// </summary>
    private void UpdateLevel()
    {
        // 経過時間を取得(get elapsed time)
        float elapsed = timer.timeLimit - timer.currentTime;

        // 経過時間が次のレベルに移行する時間を超えている場合、レベルを上げる
        // (if elapsed time exceeds the time to transition to the next level, increase the level)
        while (levelNum < ratData.SpawnDataArray.Length - 1
            && elapsed >= ratData.SpawnDataArray[levelNum].NextLevelTime)
        {
            levelNum++;
        }
    }
}

/// <summary>
/// 鳥の生成ルール(未完成)
/// </summary>
public class BirdSpawnRule : ISpawnRule
{
    private BirdData birdData;

    public BirdSpawnRule(BirdData _birdData)
    {
        birdData = _birdData;
    }

    public float GetNextSpawnInterval()
    {
        // 鳥が取ったクッキ?数*0.5だけ間隔を減らす
        var seconds = birdData.BaseSpawnInterval - (StageManager.Instance.GetBirdStoleNum() * 0.5f);

        // 最低値保障
        if (seconds < birdData.MinSpawnInterval)
        {
            seconds = birdData.MinSpawnInterval;
        }

        // 時間計算
        return Random.Range(seconds, seconds + birdData.SpawnIntervalRange);
    }

    public int GetNextSpawnNum()
    {
        return 1;
    }
}

/// <summary>
/// ネズ?の生成変数(めっちゃキモいいつか直す)
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
        FirstLevelMaxSpawnNum = 1;

        SecondLevelTime = 15.0f;
        SecondLevelMinInterval = 2.0f;
        SecondLevelMaxInterval = 3.0f;
        SecondLevelMaxSpawnNum = 2;

        ThirdLevelTime = 40.0f;
        ThirdLevelMinInterval = 1.0f;
        ThirdLevelMaxInterval = 2.0f;
        ThirdLevelMaxSpawnNum = 3;
    }
}