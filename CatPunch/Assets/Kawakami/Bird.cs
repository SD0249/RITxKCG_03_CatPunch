using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour, IDespawnNotifier
{
    public event System.Action OnDespawn;

    [SerializeField]
    private float moveSpeed = 3.0f; // 移動速度

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

    void Start()
    {
        BirdRenderer=GetComponent<Renderer>();
        BirdRb = GetComponent<Rigidbody>();
        Startpos = transform.position;
        // ステージマネージャーからターゲットとなるクッキーを取得
        //Obtain the target cookie from the stage manager.
        TargetCookieComponent = StageManager.Instance.GetRandomCookie();
        TargetCookie = TargetCookieComponent.transform;
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
            TargetCookieComponent.Confirm();
            OnDespawn?.Invoke();
            Destroy(gameObject);
        }
    }

    // プレイヤーに攻撃された際の処理
    void HitPunch()
    {
        // クッキー戻す
        TargetCookieComponent.Cancel();
        currentState = BirdState.Dead;
        BirdRb.isKinematic = false;
        StartCoroutine(DespawnRoutine());
       
    }
    IEnumerator DespawnRoutine()
    {

        // 1. 3秒間、点滅を繰り返します
        // Loop for 3 seconds
        float timer = 0;
        while (timer < 3.0f)
        {
            // 見た目のスイッチを反転させます
            // Toggle the renderer visibility
            BirdRenderer.enabled = !BirdRenderer.enabled;

            // 0.1秒だけ待機
            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            timer += 0.1f;
           
        }
        BirdRenderer.enabled = true;



        OnDespawn?.Invoke();
            Destroy(gameObject);
    }
}