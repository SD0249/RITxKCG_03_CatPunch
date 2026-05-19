using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "MouseData", menuName = "Scriptable Objects/RatData")]
public class RatData : ScriptableObject
{
    public RatSpawnData[] SpawnDataArray;
}

[System.Serializable]
public struct RatSpawnData
{
    /// <summary>
    /// Å¬¶¬ŠÔŠu(Minimum spawn interval)
    /// </summary>
    public float MinInterval;

    /// <summary>
    /// Å‘å¶¬ŠÔŠu(Maximum spawn interval)
    /// </summary>
    public float MaxInterval;

    /// <summary>
    /// Å‘å“¯¶¬”(Maximum simultaneous spawn number)
    /// </summary>
    public int MaxSpawnNum;

    /// <summary>
    /// Ÿ‚ÌƒŒƒxƒ‹‚ÉˆÚs‚·‚éŠÔ(Time to transition to the next level)
    /// </summary>
    public float NextLevelTime;
}