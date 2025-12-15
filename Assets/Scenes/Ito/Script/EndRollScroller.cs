using UnityEngine;

public class EndRollScroller : MonoBehaviour
{
    public float speed = 50f;           // スクロール速度（px/sec）
    public float endY = 1500f;          // 終了位置の Y（Canvas の解像度に合わせて調整）
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 上方向に移動
        rect.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        // 一定位置まで行ったら処理（シーン遷移など）
        if (rect.anchoredPosition.y >= endY)
        {
            // 例：タイトルシーンへ
            // SceneManager.LoadScene("Title");
        }
    }
}
