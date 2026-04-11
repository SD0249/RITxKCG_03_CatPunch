using UnityEngine;
using UnityEngine.UI;

public class TimeLimitUI : MonoBehaviour
{
    private StageManager stageManager;

    private Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageManager = StageManager.Instance;

        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = Mathf.CeilToInt(stageManager.Timer.currentTime);

        text.text = seconds.ToString();
    }
}
