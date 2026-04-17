using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickPlayAgain()
    {
        SceneManager.LoadScene("JayEnvironment");
    }

    public void ClickBackToMM()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
