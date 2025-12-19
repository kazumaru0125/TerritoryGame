using UnityEngine;
using UnityEngine.UI;

public class LobbyBackgroundScroller : MonoBehaviour
{
    public RawImage rawImage;
    public float scrollSpeed = 0.05f;

    // 全インスタンスで共有するオフセット
    private static float sharedOffset = 0f;

    void Update()
    {
        sharedOffset += scrollSpeed * Time.unscaledDeltaTime;
        sharedOffset = sharedOffset % 1f;
        rawImage.uvRect = new Rect(sharedOffset, 0, 2, 1);
    }
}
