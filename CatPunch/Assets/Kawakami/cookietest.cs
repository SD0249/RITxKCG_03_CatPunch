using UnityEngine;

public class cookietest : MonoBehaviour
{
    [SerializeField]
    private int initCookieCount;

    public int CookieRemaining { get; private set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CookieRemaining = initCookieCount;

        // StageManagerに自身を登録
        test.Instance.AddCookie(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
