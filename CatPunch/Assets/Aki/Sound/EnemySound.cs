using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair<Sound.SE, AudioClip>> seList;

    [SerializeField]
    private AudioSource source;

    private Dictionary<Sound.SE, AudioClip> seDic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seDic = new();

        foreach (var se in seList)
        {
            seDic[se.Key] = se.Value;
        }
    }

    public void PlaySE(Sound.SE seKey)
    {
        source.PlayOneShot(seDic[seKey]);
    }
}
