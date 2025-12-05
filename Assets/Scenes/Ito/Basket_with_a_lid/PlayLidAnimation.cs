using UnityEngine;

public class PlayLidAnimation : MonoBehaviour
{
    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        // Oキーを押した瞬間
        if (Input.GetKeyDown(KeyCode.O))
        {
            anim.Play();              // デフォルトのクリップを再生
            // 特定の名前のクリップなら anim.Play("Basket_box_with_a_lid.001");
        }
    }
}
