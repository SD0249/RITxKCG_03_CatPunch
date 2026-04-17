using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Stageを管理するクラス
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // クッキーのリストを初期化
        cookies = new List<Cookie>();

        // timer
        Timer = new LimitTimer(timeLimit);
        Timer.OnFinished += OnTimeLimitReached;
        Timer.Start();
    }

    [SerializeField]
    private float timeLimit;

    public LimitTimer Timer { get; private set; }

    [Tooltip("クッキーのリスト CookiesList")]
    private List<Cookie> cookies;

    /// <summary>
    /// 鳥がクッキーを盗んだ数
    /// </summary>
    private int birdStoleNum;

    /// <summary>
    /// 鳥が盗んだクッキーを加算(Add BirdStoleNum)
    /// </summary>
    public void BirdStole() => birdStoleNum++;

    /// <summary>
    /// 鳥が盗んだクッキーの数を取得
    /// </summary>
    /// <returns>クッキーの数</returns>
    public int GetBirdStoleNum() => birdStoleNum;

    public SoundManager soundManager {  get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        birdStoleNum = 0;

        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Timer.FixedUpdate(Time.fixedDeltaTime);
    }

    private void OnTimeLimitReached()
    {
        GameClear();
    }

    private void GameClear()
    {
        SceneManager.LoadScene("GameClear");
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    /// <summary>
    /// クッキーの追加
    /// </summary>
    /// <param name="cookie">追加するクッキー</param>
    public void AddCookie(Cookie cookie)
    {
        if (!cookies.Contains(cookie))
        {
            cookies.Add(cookie);
        }
    }

    /// <summary>
    /// 最も近いクッキー皿を取得
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Cookie GetNearestCookie(Vector3 pos)
    {
        if (cookies.Count <= 0)
        {
            return null;
        }

        Cookie nearest = null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < cookies.Count; i++)
        {
            if (cookies[i].StolenCount >= cookies[i].TotalCount)
            {
                continue;
            }

            float distance = (cookies[i].transform.position - pos).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = cookies[i];
            }
        }

        return nearest;
    }

    /// <summary>
    /// ランダムなクッキー皿を取得(現在未使用)
    /// </summary>
    /// <returns></returns>
    public Cookie GetRandomCookie()
    {
        int rand = Random.Range(0, cookies.Count);

        return cookies[rand];
    }

    public void StolenCookie()
    {
        // クッキーが1つでも空じゃなければ終了
        for (int i = 0; i < cookies.Count; i++)
        {
            if (!cookies[i].IsEmpty)
            {
                return;
            }
        }

        // 全て空ならゲームオーバー
        GameOver();
    }
}
