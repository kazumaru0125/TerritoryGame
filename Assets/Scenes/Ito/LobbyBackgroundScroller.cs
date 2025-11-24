using UnityEngine;
using UnityEngine.UI;

public class LobbyBackgroundScroller : MonoBehaviour
{
    public RawImage rawImage;
    public float scrollSpeed = 0.05f;

    private float offset = 0;

    private void Update()
    {
        if (rawImage != null)
        {
            // オフセットを連続で増やす
            offset += scrollSpeed * Time.deltaTime;

            // 1.0でラップ：無限に循環
            offset = offset % 1f; // offsetが0～1でループ

            // width=2で2回分常に表示
            rawImage.uvRect = new Rect(offset, 0, 2, 1);
        }
    }
}
