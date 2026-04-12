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
    }

    [SerializeField]
    private float timeLimit;

    public LimitTimer Timer { get; private set; }

    [Tooltip("クッキーのリスト CookiesList")]
    private List<Cookie> cookies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // timer
        Timer = new LimitTimer(timeLimit);
        Timer.OnFinished += OnTimeLimitReached;
        Timer.Start();
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
        // GameClear 
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

    public Cookie GetNearestCookie(Vector3 pos)
    {
        if(cookies.Count <= 0)
        {
            return null;
        }

        Cookie nearest = null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < cookies.Count; i++)
        {
            float distance = (cookies[i].transform.position - pos).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = cookies[i];
            }
        }

        return nearest;
    }

    public Cookie GetRandomCookie()
    {
        int rand = Random.Range(0, cookies.Count);

        return cookies[rand];
    }
}
