using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<EnemySpawnData> spawnDataList;

    [SerializeField]
    private float spawnAreaX;
    
    [SerializeField]
    private float spawnAreaZ;

    private SpawnPointProvider spawnPointProvider;

    private EnemyPool enemyPool;

    private List<SpawnRuntime> runtimes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runtimes = new List<SpawnRuntime>();

        enemyPool = new();

        spawnPointProvider = new SpawnPointProvider(spawnAreaX, spawnAreaZ);

        foreach (var data in spawnDataList)
        {
            var runtime = new SpawnRuntime();

            runtime.Data = data;
            runtime.Rule = CreateRule(data);
            runtime.NextInterval = runtime.Rule.GetNextSpawnInterval();

            runtimes.Add(runtime);
        }
    }

    private void FixedUpdate()
    {
        foreach (var runtime in runtimes)
        {
            runtime.Timer += Time.fixedDeltaTime;

            if (runtime.Timer < runtime.NextInterval)
            {
                continue;
            }

            TrySpawn(runtime);
            runtime.Timer = 0f;
            runtime.NextInterval = runtime.Rule.GetNextSpawnInterval();

        }
    }

    /// <summary>
    /// 各種スポーンルールの生成
    /// </summary>
    /// <param name="data">スポーンデータ</param>
    /// <returns>生成されたスポーンルール</returns>
    /// <exception cref="System.Exception">敵の種類が不明な場合にスローされます</exception>
    private ISpawnRule CreateRule(EnemySpawnData data)
    {
        switch (data.Type)
        {
            case SpawnerType.RAT:
                return new RatSpawnRule(data.BaseSpawnInterval);

            case SpawnerType.BIRD:
                return new BirdSpawnRule(data.BaseSpawnInterval);

            default:
                throw new System.Exception("Unknown enemy type");
        }
    }

    private void TrySpawn(SpawnRuntime runtime)
    {
        // 最大数になってるならスポーンさせない
        if (runtime.ActiveCount >= runtime.Data.MaxSpawnCount)
        {
            return;
        }

        // プールから取得
        var obj = enemyPool.Get(runtime.Data.Prefab);

        // 生成位置を決定
        obj.transform.position = spawnPointProvider
            .GetRandomSpawnPoint(runtime.Data.Type == SpawnerType.BIRD);

        // デスポーン通知インターフェースの取得
        var notifier = obj.GetComponent<IDespawnNotifier>();

        // デスポーン通知インターフェースがあれば、スポーン数の管理とプールへの返却を登録
        if (notifier != null)
        {
            void OnDespawnHndler()
            {
                // アクティブ数を減らす
                runtime.ActiveCount--;

                enemyPool.Return(runtime.Data.Prefab, obj);
            
                notifier.OnDespawn -= OnDespawnHndler;
            };

            notifier.OnDespawn += OnDespawnHndler;
        }

        // アクティブ数を増やす
        runtime.ActiveCount++;
    }
}
