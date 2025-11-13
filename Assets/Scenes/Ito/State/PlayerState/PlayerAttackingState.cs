using UnityEngine;
using Photon.Pun;

public class PlayerAttackingState : IPlayerState
    {
    private AttackHitboxStatus hitboxStatus;

    public void EnterState(PlayerController player)
        {
        if (player.photonView.IsMine)
            {
            // 自分が攻撃したら、全員に「攻撃開始」を通知
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, true);
            }

        // ヒットボックス制御は自分のみでOK
        hitboxStatus = player.GetComponentInChildren<AttackHitboxStatus>();
        if (hitboxStatus != null)
            {
            Debug.Log("isAttacking true!");
            hitboxStatus.isAttacking = true;
            }
        }

    public void UpdateState(PlayerController player)
        {
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.98f)
            {
            if (player.photonView.IsMine)
                {
                // 攻撃終了を全員に通知
                player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
                }

            if (hitboxStatus != null)
                {
                Debug.Log("isAttacking false!");
                hitboxStatus.isAttacking = false;
                }

            player.ChangeState(new PlayerMoveingState());
            }
        }

    public void ExitState(PlayerController player)
        {
        if (hitboxStatus != null)
            hitboxStatus.isAttacking = false;

        if (player.photonView.IsMine)
            {
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
            }
        }
    }
