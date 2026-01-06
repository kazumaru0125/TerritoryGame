using UnityEngine;
using Photon.Pun;

public class PlayerJumpingState : IPlayerState
    {
    public void EnterState(PlayerController player)
        {
        // 自分のキャラだけ物理処理を行う
        if (player.photonView.IsMine)
            {
            // 全員にジャンプアニメーション開始を通知
            player.photonView.RPC("RPC_PlayJumpAnimation", RpcTarget.All);

            // RigidbodyのY速度リセット
            Vector3 v = player.Rigidbody.linearVelocity;
            v.y = 0;
            player.Rigidbody.linearVelocity = v;

            // ジャンプ力適用
            player.Rigidbody.AddForce(Vector3.up * player.JumpForce, ForceMode.VelocityChange);
            }

        if (player.IsStunned) return;
    }

    public void UpdateState(PlayerController player)
        {
        // ローカルプレイヤーだけが地面判定してステート遷移
        if (player.photonView.IsMine && player.IsGrounded())
            {
         //   player.ChangeState(new PlayerMoveingState());
            player.ChangeState(player.moveState);
            }
        }

    public void ExitState(PlayerController player)
        {
        // 着地時の処理を入れる場合はここに（例：着地アニメ）
        }
    }
