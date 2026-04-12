using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour, IDespawnNotifier
{
    public event System.Action OnDespawn;
    [SerializeField]
    private float moveSpeed = 3.0f;

    private Transform TargetCookie;
    private Vector3 Startpos;
    private enum BirdState
    {
        Approaching, // 接近中（水平移動）
        Diving,      // 急降下中
        Ascending,   //　上昇中
        Returning    // 帰還中
    }
    private BirdState currentState = BirdState.Approaching;

    void Start()
    {
       Startpos = transform.position;
        var list = test.Instance.CookiesList;
        if (list != null && list.Count > 0)
        { 
            TargetCookie = list[Random.Range(0, list.Count)].transform;
        }
    }

    void Update()
    {
        if (TargetCookie == null) return;

        switch (currentState)
        {
            case BirdState.Approaching:
                MoveAbove(); // 水平移動のメソッド
                break;
            case BirdState.Diving:
                Dive();      // 急降下のメソッド
                break;
                case BirdState.Ascending:
                Ascend();
                break;
            case BirdState.Returning:
                ReturnHome(); // 帰還のメソッド
                break;
       
        }
    }
    void MoveAbove()
    {
        Vector3 TopOfCookie = new Vector3(TargetCookie.position.x, transform.position.y, TargetCookie.position.z);
        if (TargetCookie == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            TopOfCookie,
            moveSpeed * Time.deltaTime
        );
        transform.LookAt(TopOfCookie);
        if (Vector3.Distance(transform.position,TopOfCookie) < 0.5f)
        {
             currentState = BirdState.Diving;
        }
    }
    void Dive()
    {
        if (TargetCookie == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            TargetCookie.position,
            moveSpeed * Time.deltaTime
        );
        transform.LookAt(TargetCookie);
        if (Vector3.Distance(transform.position, TargetCookie.position) < 0.5f)
        {
            currentState = BirdState.Ascending;
        }

    }
    void Ascend()
    {
        Vector3 TopOfCookie = new Vector3(TargetCookie.position.x, Startpos.y, TargetCookie.position.z);
        if (TargetCookie == null) return;
        transform.position = Vector3.MoveTowards(
            transform.position,
            TopOfCookie,
            moveSpeed * Time.deltaTime
        );
        transform.LookAt(TopOfCookie);
        if (Vector3.Distance(transform.position, TopOfCookie) < 0.5f)
        {
            currentState = BirdState.Returning;
        }
    }
    void ReturnHome()
    {
        if (TargetCookie == null) return;
        transform.position = Vector3.MoveTowards(
            transform.position,
            Startpos,
            moveSpeed * Time.deltaTime
        );
        transform.LookAt(Startpos);
        if (Vector3.Distance(transform.position, Startpos) < 0.5f)
        {   OnDespawn?.Invoke();
            // クッキーを持ち帰った後の処理
            Destroy(gameObject); // 鳥を削除
        }
    }

}