using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneButton : MonoBehaviour
{
    SoundManager soundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickPlayAgain()
    {
        // Play click sound effect
        soundManager.PlaySE(Sound.SE.CLICK);

        SceneManager.LoadScene("JayEnvironment");
    }

    public void ClickBackToMM()
    {
        // Play click sound effect
        soundManager.PlaySE(Sound.SE.CLICK);

        SceneManager.LoadScene("MainMenu");
    }
}
