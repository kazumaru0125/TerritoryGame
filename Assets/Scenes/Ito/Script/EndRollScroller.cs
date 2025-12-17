using UnityEngine;

public class EndRollScroller : MonoBehaviour
{
    public float speed = 50f;   // スクロール速度（px/sec）
    public float endY = 1500f;  // 終了位置の Y

    RectTransform rect;
    Vector2 startPos;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;   // 初期位置を保存
    }

    void OnEnable()
    {
        // エンドロールが表示されるたびに位置を初期化
        rect.anchoredPosition = startPos;
    }

    void Update()
    {
        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        if (rect.anchoredPosition.y >= endY)
        {
            // 必要なら自動でタイトルに戻す処理を呼ぶ
            // 例: FindObjectOfType<TitleIdleEndRollController>().HideEndRollAndBackToTitle();
        }
    }
}
