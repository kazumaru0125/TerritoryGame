using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverHighlight : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public Image borderImage;
    public float flashSpeed = 4f;

    bool isHover;

    void Reset()
    {
        if (borderImage == null)
        {
            var t = transform.Find("Border");
            if (t != null) borderImage = t.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (borderImage == null) return;

        if (isHover)
        {
            float a = (Mathf.Sin(Time.unscaledTime * flashSpeed) + 1f) * 0.5f;
            var c = borderImage.color;
            c.a = a;
            borderImage.color = c;
        }
        else
        {
            var c = borderImage.color;
            c.a = 0f;
            borderImage.color = c;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
