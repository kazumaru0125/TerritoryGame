using UnityEngine;
using Photon.Pun;

public class PlayerAttackingState : IPlayerState
    {
    private AttackHitboxStatus hitboxStatus;
    private bool effectSpawned = false; // エフェクト生成フラグ

    public void EnterState(PlayerController player)
        {
        effectSpawned = false;

        if (player.photonView.IsMine)
            {
            // 攻撃開始を全員に通知
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, true);
            }

        // ヒットボックス制御は自分のみ
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

        // 攻撃終了判定
        if (stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.98f)
            {
            EndAttack(player);
            return;
            }

        // 攻撃ヒットタイミングでエフェクト生成（50%のタイミング例）
        if (!effectSpawned && stateInfo.IsTag("Attack") && stateInfo.normalizedTime >= 0.5f)
            {
            SpawnAttackEffect(player);
            }
        }

    private void SpawnAttackEffect(PlayerController player)
        {
        if (!player.photonView.IsMine) return;

        if (player.attackEffectPrefab != null && hitboxStatus != null)
            {
            Vector3 effectPosition = hitboxStatus.transform.position;
            Quaternion effectRotation = Quaternion.identity;

            // 全員にエフェクト生成を通知
            //player.photonView.RPC("RPC_SpawnAttackEffect", RpcTarget.All, effectPosition, effectRotation);
            // player.photonView.RPC( "RPC_SpawnAttackEffect",RpcTarget.All,hitboxStatus.transform.position,Quaternion.Euler(90f, 0f, 0f));
            player.photonView.RPC("RPC_SpawnAttackEffect", RpcTarget.All);

            effectSpawned = true;
            }
        }

    private void EndAttack(PlayerController player)
        {
        if (player.photonView.IsMine)
            {
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
            }

        if (hitboxStatus != null)
            {
            hitboxStatus.isAttacking = false;
            }

        player.ChangeState(new PlayerMoveingState());
        }

    public void ExitState(PlayerController player)
        {
        if (hitboxStatus != null)
            hitboxStatus.isAttacking = false;

        if (player.photonView.IsMine)
            {
            //  player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
            //  player.photonView.RPC( "RPC_SpawnAttackEffect",RpcTarget.All,hitboxStatus.transform.position,Quaternion.Euler(90f, 0f, 0f));
            player.photonView.RPC("RPC_SpawnAttackEffect", RpcTarget.All);

            }
        }
    }
