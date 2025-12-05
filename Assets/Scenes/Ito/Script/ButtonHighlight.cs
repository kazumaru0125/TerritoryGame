using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image borderImage;         // 枠線Image
    public float flashSpeed = 4f;     // 点滅スピード

    bool isSelected;

    void Reset()
    {
        // 自動で子のImageを探す（"Border"など名前で）
        if (borderImage == null)
        {
            var t = transform.Find("Border");
            if (t != null) borderImage = t.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (borderImage == null) return;

        if (isSelected)
        {
            // sin波で0～1を往復させてAlphaを点滅
            float a = (Mathf.Sin(Time.unscaledTime * flashSpeed) + 1f) * 0.5f;
            var c = borderImage.color;
            c.a = a;
            borderImage.color = c;
        }
        else
        {
            // 非選択時は枠線消す
            var c = borderImage.color;
            c.a = 0f;
            borderImage.color = c;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }
}
