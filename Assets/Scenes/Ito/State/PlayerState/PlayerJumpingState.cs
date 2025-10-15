using UnityEngine;

public class PlayerJumpingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // ジャンプアニメーション開始
        player.Animator.Play("Jump", 0, 0);
        // RigidbodyのY速度リセット
        Vector3 v = player.Rigidbody.linearVelocity;
        v.y = 0;
        player.Rigidbody.linearVelocity = v;
        // ジャンプ力適用（ベロシティチェンジ）
        player.Rigidbody.AddForce(Vector3.up * player.JumpForce, ForceMode.VelocityChange);
    }

    public void UpdateState(PlayerController player)
    {
        // 空中にいる間の追加処理があれば記述（例：エアコントロール等）

        // 地面に着地したら歩きに遷移
        if (player.IsGrounded())
        {
            player.ChangeState(new PlayerMoveingState());
        }
    }

    public void ExitState(PlayerController player)
    {
        // 着地時のアニメーションや状態リセットがあれば記述
    }
}

