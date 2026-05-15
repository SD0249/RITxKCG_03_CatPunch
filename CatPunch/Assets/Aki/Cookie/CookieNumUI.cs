using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クッキーの数UI
/// </summary>
public class CookieNumUI : MonoBehaviour
{
    [Tooltip("ステージマネージャー")]
    private StageManager stageManager;

    [Tooltip("クッキーの数テキスト")]
    private Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ステージマネージャーの取得
        stageManager = StageManager.Instance;

        // クッキーの数テキストの取得
        text = GetComponent<Text>();

        // クッキーの数表示の更新イベントに登録
        stageManager.OnUpdateCookieUI += UpdateCookieCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// クッキーの数表示の更新
    /// </summary>
    /// <param name="allCount">全クッキーの数</param>
    /// <param name="currentCount">現在のクッキーの数</param>
    public void UpdateCookieCount(int allCount,int currentCount)
    {
        text.text = $"{currentCount} / {allCount}";
    }
}
