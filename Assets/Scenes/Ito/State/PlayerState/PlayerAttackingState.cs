using UnityEngine;

public class PlayerAttackingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.Animator.SetBool("is_attacking", true);
        player.Animator.SetBool("is_walking", false);
        player.Animator.SetBool("is_running", false);
    }

    public void UpdateState(PlayerController player)
    {
        // 攻撃アニメーションの終了判定
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("Attack") || stateInfo.normalizedTime >= 1f)
        {
            // 攻撃が終わったら歩き状態に戻る
            player.ChangeState(new PlayerMoveingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        player.Animator.SetBool("is_attacking", false);
    }
}
