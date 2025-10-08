using UnityEngine;

public class AttackController : MonoBehaviour
{
    Animator anim;
    bool isAttackPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
    }

    void Update()
    {
        float rt = Input.GetAxis("RT");

        if (!isAttackPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
            {
                anim.SetBool("is_attacking", true);
                isAttackPlaying = true;
            }
        }
        else
        {
            // アニメーション終了判定
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // is_attackingがtrueの間かつアニメーションの再生時間が終わっていればフラグ解除
            if (!stateInfo.IsName("is_attacking") || stateInfo.normalizedTime >= 1f)
            {
                anim.SetBool("is_attacking", false);
                isAttackPlaying = false;
            }
        }
    }
}
