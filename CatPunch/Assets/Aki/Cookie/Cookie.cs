using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField]
    private int initCookieCount;

    public int CookieRemaining { get; private set; }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CookieRemaining = initCookieCount;

        // StageManagerÇ…é©êgÇìoò^
        StageManager.Instance.AddCookie(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
