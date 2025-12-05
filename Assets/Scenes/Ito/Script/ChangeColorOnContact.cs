using UnityEngine;

public class ChangeColorOnContact : MonoBehaviour
{
    private Renderer objRenderer;
    private Color originalColor;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null)
        {
            originalColor = objRenderer.material.color;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // プレイヤー側に"Player"タグを付けている想定
        {
            if (objRenderer != null)
            {
                objRenderer.material.color = Color.red; // 触れたら赤に変える
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (objRenderer != null)
            {
                objRenderer.material.color = originalColor; // 離れたら元の色に戻す
            }
        }
    }
}
