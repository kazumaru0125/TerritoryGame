using UnityEngine;
using UnityEngine.UI;

public class LobbyBackgroundScroller : MonoBehaviour
{
    public RawImage rawImage;
    public float scrollSpeed = 0.05f;

    float offset = 0;
    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        offset = offset % 1f;
        rawImage.uvRect = new Rect(offset, 0, 2, 1);
    }

}
