using System.Collections.Generic;
using UnityEngine;

// Needed for Mice to avoid each other 
// TRACKS ONLY ACTIVE MICE!!!
public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    public List<Mouse> allMice = new List<Mouse>();

    // Not needed at actual scene
    // public GameObject[] testSceneCookie;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterMouse(Mouse mouse)
    {
        if (!allMice.Contains(mouse))
            allMice.Add(mouse);
    }

    public void UnregisterMouse(Mouse mouse)
    {
        if (allMice.Contains(mouse))
            allMice.Remove(mouse);
    }

    // Test method
    //public GameObject GetRandomTestSceneCookie()
    //{
    //    int randomindex = Random.Range(0, 4);
    //    return testSceneCookie[randomindex];
    //}
}
