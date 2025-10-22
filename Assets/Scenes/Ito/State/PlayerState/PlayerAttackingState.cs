using UnityEngine;

public class PlayerAttackingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.Animator.SetBool("is_attacking", true);
        player.Animator.SetBool("is_walking", false);
        player.Animator.SetBool("is_running", false);
        player.Rigidbody.linearVelocity = Vector3.zero;
    }

    public void UpdateState(PlayerController player)
    {
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        // "Attack" タグのステートのみで攻撃終了を判定（AnimatorでAttackステートにTag="Attack"を設定）
        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.98f)
        {
            player.ChangeState(new PlayerMoveingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        player.Animator.SetBool("is_attacking", false);
    }
}
