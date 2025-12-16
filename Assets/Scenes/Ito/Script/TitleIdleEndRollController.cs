using UnityEngine;

public class TitleIdleEndRollController : MonoBehaviour
{
    public float idleTimeToShowEndRoll = 5f;   // 無操作でエンドロールを出すまで
    public GameObject titleCanvasRoot;         // タイトル用 Canvas（任意）
    public GameObject endRollCanvasRoot;       // エンドロール用 Canvas

    float lastInputTime;
    bool endRollVisible = false;

    void Start()
    {
        lastInputTime = Time.time;

        if (endRollCanvasRoot != null)
            endRollCanvasRoot.SetActive(false);    // 最初は非表示
        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(true);       // タイトルは表示
    }

    void Update()
    {
        // いつでも入力は見る
        bool hasInput = HasAnyInput();

        if (!endRollVisible)
        {
            // タイトル表示中：無操作時間を測る
            if (hasInput)
            {
                lastInputTime = Time.time;         // 入力があったらリセット
            }

            if (Time.time - lastInputTime >= idleTimeToShowEndRoll)
            {
                ShowEndRoll();                     // 5秒放置でエンドロール表示
            }
        }
        else
        {
            // エンドロール表示中：入力があったらタイトルに戻す
            if (hasInput)
            {
                HideEndRollAndBackToTitle();
            }
        }
    }

    bool HasAnyInput()
    {
        if (Input.anyKeyDown) return true;
        if (Input.GetAxisRaw("Horizontal") != 0) return true;
        if (Input.GetAxisRaw("Vertical") != 0) return true;
        // 必要に応じてボタンを追加
        // if (Input.GetButtonDown("Submit")) return true;
        return false;
    }

    void ShowEndRoll()
    {
        endRollVisible = true;

        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(false);      // タイトル非表示

        if (endRollCanvasRoot != null)
        {
            // 必要ならここで位置リセット
            // var scroller = endRollCanvasRoot.GetComponentInChildren<EndRollScroller>();
            // if (scroller != null) scroller.ResetPosition();

            endRollCanvasRoot.SetActive(true);     // エンドロール表示開始
        }
    }

    void HideEndRollAndBackToTitle()
    {
        endRollVisible = false;
        lastInputTime = Time.time;                 // 戻った直後から再カウント

        if (endRollCanvasRoot != null)
            endRollCanvasRoot.SetActive(false);    // エンドロール非表示

        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(true);       // タイトル再表示
    }
}
