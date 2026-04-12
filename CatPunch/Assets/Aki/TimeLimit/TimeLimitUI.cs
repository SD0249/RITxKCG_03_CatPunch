using UnityEngine;
using UnityEngine.UI;

public class TimeLimitUI : MonoBehaviour
{
    private StageManager stageManager;

    private Text text;

    private bool isTenCount;

    [SerializeField]
    private int tenCountFontSize;

    [SerializeField]
    private Color tenCountColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageManager = StageManager.Instance;

        text = GetComponent<Text>();

        isTenCount = false;
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = Mathf.CeilToInt(stageManager.Timer.currentTime);

        if (seconds <= 10 && !isTenCount)
        {
            isTenCount = true;

            text.fontSize = tenCountFontSize;

            text.color = tenCountColor;
        }

        text.text = seconds.ToString();
    }
}
