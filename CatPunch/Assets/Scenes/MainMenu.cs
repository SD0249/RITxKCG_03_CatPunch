using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Reference to panels
    public Image controlPanel;
    public Image UIPanel;

    private SoundManager soundManager;

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

    private void OnEnable()
    {
        controlPanel.gameObject.SetActive(false);
        UIPanel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
    }

    public void ClickStartButton()
    {
        // Play click sound effect
        soundManager.PlaySE(Sound.SE.CLICK);

        SceneManager.LoadScene("JayEnvironment");
    }

    public void ClickControlButton()
    {
        // Play click sound effect
        soundManager.PlaySE(Sound.SE.CLICK);

        controlPanel.gameObject.SetActive(true);
    }

    public void ClickCreditButton()
    {
        // Play click sound effect
        soundManager.PlaySE(Sound.SE.CLICK);

        UIPanel.gameObject.SetActive(true);
    }

    public void Cancel()
    {
        Debug.Log("Panels Disabled!");
        controlPanel.gameObject.SetActive(false);
        UIPanel.gameObject.SetActive(false);
    }
}
