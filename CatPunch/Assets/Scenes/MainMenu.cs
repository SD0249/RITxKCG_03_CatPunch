using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Reference to panels
    public Image controlPanel;
    public Image UIPanel;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        SceneManager.LoadScene("JayEnvironment");
    }

    public void ClickControlButton()
    {
        controlPanel.gameObject.SetActive(true);
    }

    public void ClickCreditButton()
    {
        UIPanel.gameObject.SetActive(true);
    }

    public void Cancel()
    {
        Debug.Log("Panels Disabled!");
        controlPanel.gameObject.SetActive(false);
        UIPanel.gameObject.SetActive(false);
    }
}
