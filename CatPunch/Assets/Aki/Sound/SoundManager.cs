using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class KeyValuePair<TKey, TValue>
{
    public TKey Key;

    public TValue Value;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair<Sound.SE,AudioClip>> seList;

    private AudioSource source;

    private Dictionary<Sound.SE,AudioClip> seDic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source = GetComponent<AudioSource>();

        seDic = new();

        foreach(var se in seList)
        {
            seDic[se.Key] = se.Value;
        }
    }

    public void PlaySE(Sound.SE seKey)
    {
        source.PlayOneShot(seDic[seKey]);
    }
}
