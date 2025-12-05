using UnityEngine;

public class DizzyController : MonoBehaviour
{
    Animator anim;
    bool isDizzyingPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
    }

    void Update()
    {

        if (!isDizzyingPlaying)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                anim.SetBool("is_dizzying", true);
                isDizzyingPlaying = true;
            }
        }
        else
        {
            // アニメーション終了判定
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // is_dizzyingがtrueの間かつアニメーションの再生時間が終わっていればフラグ解除
            if (!stateInfo.IsName("is_dizzying") || stateInfo.normalizedTime >= 1f)
            {
                anim.SetBool("is_dizzying", false);
                isDizzyingPlaying = false;
            }
        }
    }
}
