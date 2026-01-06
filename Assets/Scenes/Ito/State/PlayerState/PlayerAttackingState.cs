using UnityEngine;
using Photon.Pun;

public class PlayerAttackingState : IPlayerState
    {
    private AttackHitboxStatus hitboxStatus;
    private bool effectSpawned;

    private float attackTimer;
    private const float MAX_ATTACK_TIME = 0.6f; // 攻撃最大時間（空中保険）

    public void EnterState(PlayerController player)
        {
        effectSpawned = false;
        attackTimer = 0f;

        if (player.photonView.IsMine)
            {
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, true);
            }

        hitboxStatus = player.GetComponentInChildren<AttackHitboxStatus>();
        if (hitboxStatus != null)
            {
            hitboxStatus.isAttacking = true;
            }
        }

    public void UpdateState(PlayerController player)
        {
        attackTimer += Time.deltaTime;

        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        // 地上攻撃：アニメ終了で終わる
        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.98f)
            {
            EndAttack(player);
            return;
            }

        // 空中攻撃：時間で強制終了
        if (!stateInfo.IsTag("Attack") && attackTimer >= MAX_ATTACK_TIME)
            {
            EndAttack(player);
            return;
            }

        // エフェクト
        if (!effectSpawned && attackTimer >= 0.25f)
            {
            SpawnAttackEffect(player);
            }
        }

    private void SpawnAttackEffect(PlayerController player)
        {
        if (!player.photonView.IsMine) return;
        if (player.IsStunned) return;

        player.photonView.RPC("RPC_SpawnAttackEffect", RpcTarget.All);
        effectSpawned = true;
        }

    private void EndAttack(PlayerController player)
        {
        if (player.photonView.IsMine)
            {
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
            }

        if (hitboxStatus != null)
            hitboxStatus.isAttacking = false;

        player.ChangeState(player.moveState);
        }

    public void ExitState(PlayerController player)
        {
        if (hitboxStatus != null)
            hitboxStatus.isAttacking = false;
        }
    }

