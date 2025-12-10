using UnityEngine;

public class DisappointmentController : MonoBehaviour
{
    Animator anim;
    bool isDisappointmentingPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
    }

    void Update()
    {

        if (!isDisappointmentingPlaying)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                anim.SetBool("is_Disappointmenting", true);
                isDisappointmentingPlaying = true;
            }
        }
        else
        {
            // アニメーション終了判定
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // is_Disappointmentingがtrueの間かつアニメーションの再生時間が終わっていればフラグ解除
            if (!stateInfo.IsName("is_Disappointmenting") || stateInfo.normalizedTime >= 1f)
            {
                anim.SetBool("is_Disappointmenting", false);
                isDisappointmentingPlaying = false;
            }
        }
    }
}
