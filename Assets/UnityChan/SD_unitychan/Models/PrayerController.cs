using UnityEngine;

public class PrayerController : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
    }

    void Update()
    {
        // キー・ボタンの長押し判定
        bool isKeyHeld = Input.GetKey(KeyCode.F) || Input.GetKey("joystick button 1");

        anim.SetBool("is_prayering", isKeyHeld);
    }
}
