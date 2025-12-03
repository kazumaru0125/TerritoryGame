using UnityEngine;
using Photon.Pun;

public class PlayerTrapDameageState : IPlayerState
{
    private float stunStartTime;   // スタン開始時間
    private float stunDuration;    // このステートで使うスタン秒数

    public void EnterState(PlayerController player)
    {
        // Player側の設定値をコピー（毎回同じでもOKだし、状態ごとに変えてもOK）
        stunDuration = player.StunDuration;
        stunStartTime = Time.time; // 今の時間を記録[web:77][web:78]

        if (player.photonView.IsMine)
        {
            player.SetStun(true);
            player.photonView.RPC("RPC_SetDizzyingState", RpcTarget.All, true);
        }
    }

    public void UpdateState(PlayerController player)
    {
        // 一定時間経過したらスタン解除
        float elapsed = Time.time - stunStartTime; // 経過秒[web:77][web:78]
        if (elapsed >= stunDuration)
        {
            if (player.photonView.IsMine)
            {
                player.photonView.RPC("RPC_SetDizzyingState", RpcTarget.All, false);
            }
            EndDizzying(player);
        }
    }

    private void EndDizzying(PlayerController player)
    {
        if (player.photonView.IsMine)
        {
            player.SetStun(false);
        }

        player.ChangeState(new PlayerMoveingState());
    }

    public void ExitState(PlayerController player)
    {
        if (player.photonView.IsMine)
        {
            player.photonView.RPC("RPC_SetDizzyingState", RpcTarget.All, false);
            player.SetStun(false);
        }
    }
}
