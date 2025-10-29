using UnityEngine;
using TMPro;

public class BlinkTMPText : MonoBehaviour
{
    public float speed = 1.0f;
    private float time;
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.color = GetAlphaColor(text.color);
    }

    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 3.5f * speed;
        color.a = Mathf.Abs(Mathf.Sin(time)); // AlphaÇÕê≥ÇÃílÇ…
        return color;
    }
}
