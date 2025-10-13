using System.Collections;
using UnityEngine;

public class PlayerAttackingState : IPlayerState
{
    //private Animator anim;
    private bool isCatchingPlaying = false;
    private PlayerController playerController;

    public void EnterState(PlayerController player)
    {
        playerController = player;
        //anim = player.GetComponent<Animator>();
        //if (anim == null)
        //{
        //    Debug.LogError("Animatorが見つかりません！");
        //}
        isCatchingPlaying = false;
    }

    public void UpdateState(PlayerController player)
    {
        float rt = Input.GetAxis("RT");

        if (!isCatchingPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
            {
                //anim.SetBool("is_attacking", true);
                isCatchingPlaying = true;
            }
        }
        else
        {
            // アニメーション終了判定
            //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // 実際の攻撃アニメーション名(例: "Attack")で判定するのが安全
            //if (!stateInfo.IsName("Attack") || stateInfo.normalizedTime >= 1f)
            //{
            //    //anim.SetBool("is_attacking", false);
            //    isCatchingPlaying = false;
            //    // 攻撃終了後にアイドル状態に戻す
            //    player.ChangeState(player.idelState);
            //}
        }
    }

    public void ExitState(PlayerController player)
    {
        //anim.SetBool("is_attacking", false);
        isCatchingPlaying = false;
    }
}
