using UnityEngine;

/// <summary>
/// 生成位置を提供するクラス
/// </summary>
public class SpawnPointProvider
{
    private float spawnHalfX;

    private float spawnHalfZ;

    private float birdSpawnHeight = 5f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="spawnAreaX">生成位置X</param>
    /// <param name="spawnAreaZ">生成位置Z</param>
    public SpawnPointProvider(float spawnAreaX, float spawnAreaZ)
    {
        spawnHalfX = spawnAreaX * 0.5f;
        spawnHalfZ = spawnAreaZ * 0.5f;
    }

    /// <summary>
    /// ランダムな生成位置を取得
    /// </summary>
    /// <param name="isBird">true = bird</param>
    /// <returns>生成位置</returns>
    public Vector3 GetRandomSpawnPoint(bool isBird = false)
    {
        Vector3 spawnPoint = Vector3.zero;

        var spawnSide = Random.Range(0, 4);

        switch (spawnSide)
        {
            case 0:

                spawnPoint.x = Random.Range(-spawnHalfX, spawnHalfX);
                spawnPoint.z = spawnHalfZ;

                break;

            case 1:

                spawnPoint.x = Random.Range(-spawnHalfX, spawnHalfX);
                spawnPoint.z = -spawnHalfZ;

                break;

            case 2:

                spawnPoint.x = spawnHalfX;
                spawnPoint.z = Random.Range(-spawnHalfZ, spawnHalfZ);

                break;

            case 3:

                spawnPoint.x = -spawnHalfX;
                spawnPoint.z = Random.Range(-spawnHalfZ, spawnHalfZ);

                break;

            default:

                Debug.LogError("Invalid spawn side: " + spawnSide);

                break;
        }

        if (isBird)
        {
            spawnPoint.y = birdSpawnHeight;
        }

        return spawnPoint;
    }
}
