using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// マウスオーバーしている間、ボタンの枠を点滅させるコンポーネント
// ボタン本体のオブジェクトにアタッチして使う
public class ButtonHoverHighlight : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    // 点滅させたい枠用の Image（ボタンの子に置いた Border など）
    public Image borderImage;

    // 点滅スピード（値を大きくすると速く点滅）
    public float flashSpeed = 4f;

    // いまマウスがこのボタンの上に乗っているかどうか
    bool isHover;

    // コンポーネント追加時などに自動で呼ばれる
    // borderImage が未設定なら、子オブジェクト "Border" から自動取得する
    void Reset()
    {
        if (borderImage == null)
        {
            var t = transform.Find("Border");
            if (t != null) borderImage = t.GetComponent<Image>();
        }
    }

    // 毎フレーム、枠のアルファ値を更新して点滅させる
    void Update()
    {
        if (borderImage == null) return;

        if (isHover)
        {
            // sin波を使って 0～1 の値を作り、それをアルファ値に使う
            float a = (Mathf.Sin(Time.unscaledTime * flashSpeed) + 1f) * 0.5f;

            var c = borderImage.color;
            c.a = a;
            borderImage.color = c;
        }
        else
        {
            // ホバーしていないときはアルファ0で完全に非表示
            var c = borderImage.color;
            c.a = 0f;
            borderImage.color = c;
        }
    }

    // マウスカーソルがボタン上に入ったときに呼ばれる
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
    }

    // マウスカーソルがボタン上から出たときに呼ばれる
    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }

    // ボタンを含むパネルが非表示(SetActive(false))になったときなどに呼ばれる
    // 戻ったときに枠が点滅しっぱなしにならないよう、状態をリセットする
    void OnDisable()
    {
        isHover = false;

        if (borderImage != null)
        {
            var c = borderImage.color;
            c.a = 0f;
            borderImage.color = c;
        }
    }
}
