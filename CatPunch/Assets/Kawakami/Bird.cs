using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour, IDespawnNotifier
{
    public event System.Action OnDespawn;

    [SerializeField]
    private float moveSpeed = 3.0f; // 移動速度
    [SerializeField]
   　private float fadeDuration = 1.5f;// フェードアウトの時間
    [SerializeField]
    private float DeleateTime = 3.0f;// 薄くなるまでの時間

    private Transform TargetCookie;        // ターゲットの座標
    private Vector3 Startpos;              // 初期位置
    private Cookie TargetCookieComponent;  // ターゲットのCookieスクリプト参照
    private Rigidbody BirdRb;// Rigidbodyコンポーネントの参照
    private Renderer BirdRenderer; // 鳥のレンダラー参照


    private enum BirdState
    {
        Approaching, // 接近
        Diving,      // 急降下
        Ascending,   // 上昇
        Returning,    // 帰還
        Dead        // 死亡
    }
    private BirdState currentState = BirdState.Approaching;

    void Awake()
    {
        BirdRb = GetComponent<Rigidbody>();
        BirdRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        Debug.Log("Bird Enabled!");

        Startpos = transform.position;
        TargetCookieComponent = StageManager.Instance.GetRandomCookie();
        TargetCookie = TargetCookieComponent.transform;

        if (BirdRb != null)
        {
            BirdRb.isKinematic = true; // 鳥のRigidbodyをキネマティックに設定

            // Got warning - Kinematic body's linear or angular velocity shouldn't be manually set
            // BirdRb.linearVelocity = Vector3.zero; // 速度をリセット
            // BirdRb.angularVelocity = Vector3.zero;
        }
        // 2. 見た目をはっきりさせる（透明や非表示から戻します）
        if (BirdRenderer != null)
        {
            // 色と透明度を元通り（不透明）にいたします
            BirdRenderer.enabled = true;
            Color c = BirdRenderer.material.color;
            BirdRenderer.material.color = new Color(c.r, c.g, c.b, 1.0f);
        }
        currentState = BirdState.Approaching;
        Debug.Log("Bird state: Approaching");
    }
    void Update()
    {
        if (TargetCookie == null) return;

        switch (currentState)
        {
            case BirdState.Approaching:
                MoveAbove();
                break;
            case BirdState.Diving:
                Dive();
                break;
            case BirdState.Ascending:
                Ascend();
                break;
            case BirdState.Returning:
                ReturnHome();
                break;
            case BirdState.Dead:
                break;
        }
    }

    // 1. クッキーの真上まで水平移動/Move horizontally to directly above the cookie.
    void MoveAbove()
    {
        Vector3 TopOfCookie = new Vector3(TargetCookie.position.x, transform.position.y, TargetCookie.position.z);

        transform.position = Vector3.MoveTowards(transform.position, TopOfCookie, moveSpeed * Time.deltaTime);
        transform.LookAt(TopOfCookie);

        if (Vector3.Distance(transform.position, TopOfCookie) < 0.5f)
        {
            currentState = BirdState.Diving;
        }
    }

    // 2. クッキーに向かって急降下/Plunging towards the cookie
    void Dive()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetCookie.position, moveSpeed * Time.deltaTime);
        transform.LookAt(TargetCookie);

        if (Vector3.Distance(transform.position, TargetCookie.position) < 0.1f)
        {
            currentState = BirdState.Ascending;
        }
    }

    // 3. 元の高さまで上昇/Return to original height
    void Ascend()
    {
        Vector3 TopOfCookie = new Vector3(TargetCookie.position.x, Startpos.y, TargetCookie.position.z);

        transform.position = Vector3.MoveTowards(transform.position, TopOfCookie, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, TopOfCookie) < 0.1f)
        {
            // クッキーを仮取得
            //Get a temporary cookie
            TargetCookieComponent.TryReserve();
            currentState = BirdState.Returning;
        }
    }

    // 4. 初期位置へ帰還
    void ReturnHome()
    {
        transform.position = Vector3.MoveTowards(transform.position, Startpos, moveSpeed * Time.deltaTime);
        transform.LookAt(Startpos);

        if (Vector3.Distance(transform.position, Startpos) < 0.1f)
        {
            // クッキーの取得
            //Acquisition of cookies
            StageManager.Instance.BirdStole();
            TargetCookieComponent.Confirm();
            OnDespawn?.Invoke();
     
        }
    }

    // プレイヤーに攻撃された際の処理
    public void HitPunch()
    {
        Debug.Log("Bird Hit!");

        // クッキー戻す
        if (TargetCookieComponent != null)
        {
            TargetCookieComponent.Cancel();
        }
        currentState = BirdState.Dead;
        BirdRb.isKinematic = false;
        StartCoroutine(DespawnRoutine());
       
    }
    IEnumerator DespawnRoutine()
    {
        // 1. 3秒間、点滅を繰り返す
        // Loop for 3 seconds
        float timer = 0;
        while (timer < DeleateTime)
        {
            //消滅するまでに点滅させると思って間違えて書いてました点滅させても
           // BirdRenderer.enabled = !BirdRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
          timer += 0.1f;
        }

          BirdRenderer.enabled = true;
       
        // 元の色情報の保存
        Color c = BirdRenderer.material.color;
        //ゆっくり透明になっていく
        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
            BirdRenderer.material.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }


        OnDespawn?.Invoke();
        

    }
}