using UnityEngine;

public class PlayerAttackingState : IPlayerState
{
    private AttackHitboxStatus hitboxStatus;

    public void EnterState(PlayerController player)
    {
        player.Animator.SetBool("is_attacking", true);
        // 攻撃用のヒットボックスを検索
        hitboxStatus = player.GetComponentInChildren<AttackHitboxStatus>();
        if (hitboxStatus != null)
        {
            Debug.Log("isAttacking true!");
            hitboxStatus.isAttacking = true; // ★攻撃開始時にtrue
        }
    }

    public void UpdateState(PlayerController player)
    {
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.98f)
        {
            player.Animator.SetBool("is_attacking", false);
            if (hitboxStatus != null)
            {
                Debug.Log("isAttacking false!");
                hitboxStatus.isAttacking = false; // ★攻撃終了時にfalse
            }
            player.ChangeState(new PlayerMoveingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        if (hitboxStatus != null) hitboxStatus.isAttacking = false;
        player.Animator.SetBool("is_attacking", false);
    }
}
