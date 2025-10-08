using UnityEngine;

public class ClapController : MonoBehaviour
{
    Animator anim;
    bool isClapPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
    }

    void Update()
    {
        //float rt = Input.GetAxis("RT");

        if (!isClapPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown("joystick button 2"))
            {
                anim.SetBool("is_claping", true);
                isClapPlaying = true;
            }
        }
        else
        {
            // アニメーション終了判定
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // is_clapingがtrueの間かつアニメーションの再生時間が終わっていればフラグ解除
            if (!stateInfo.IsName("is_claping") || stateInfo.normalizedTime >= 1f)
            {
                anim.SetBool("is_claping", false);
                isClapPlaying = false;
            }
        }
    }
}
