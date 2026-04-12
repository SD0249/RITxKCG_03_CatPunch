using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField]
    private int totalCount;

    public int TotalCount => totalCount;

    public int AvailableCount { get; private set; }

    private int reservedCount;

    /// <summary>
    /// 完全に盗まれたカウントの取得
    /// </summary>
    public int StolenCount => totalCount - AvailableCount - reservedCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AvailableCount = totalCount;

        reservedCount = 0;

        // StageManagerに自身を登録
        StageManager.Instance.AddCookie(this);
    }

    public bool TryReserve()
    {
        if(AvailableCount <= 0)
        {
            return false;
        }

        AvailableCount--;
        reservedCount++;

        return true;
    }

    public void Confirm()
    {
        if(reservedCount <= 0)
        {
            return;
        }

        reservedCount--;

        StageManager.Instance.StolenCookie();
    }

    public void Cancel()
    {
        if (reservedCount <= 0)
        {
            return;
        }

        reservedCount--;
        AvailableCount++;
    }
}
