using UnityEngine;

[CreateAssetMenu(fileName = "BirdData", menuName = "Scriptable Objects/BirdData")]
public class BirdData : ScriptableObject
{
    /// <summary>
    /// Œ³‚É‚È‚é¶¬ŠÔŠu
    /// </summary>
    public float BaseSpawnInterval;

    /// <summary>
    /// ¶¬ŠÔŠu‚Ìƒ‰ƒ“ƒ_ƒ€U‚ê•
    /// </summary>
    public float SpawnIntervalRange;

    /// <summary>
    /// Å¬¶¬ŠÔŠu
    /// </summary>
    public float MinSpawnInterval;
}
