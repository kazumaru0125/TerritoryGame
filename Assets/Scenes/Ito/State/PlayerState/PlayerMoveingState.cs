using UnityEngine;
using Photon.Pun;

public class PlayerMoveingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        if (!player.photonView.IsMine) return;

        UpdateAnimation(player);
    }

    public void UpdateState(PlayerController player)
    {
        // 自分のキャラ以外は入力処理をしない
        if (!player.photonView.IsMine) return;
        if (player.IsStunned) return;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = forward * z + right * x;

        bool isWalking = false;
        bool isRunning = false;

        bool isGrounded = player.IsGrounded();
        bool isDash = Input.GetKey("joystick button 2") && isGrounded;
        bool runToggle = player.IsRun;

        if (move.magnitude > 0.05f)
        {
            isWalking = true;
            if (isDash || runToggle)
                isRunning = true;

            float speed = isRunning ? player.RunSpeed : player.WalkSpeed;
            Vector3 moveDir = move.normalized * speed * Time.deltaTime;
            player.transform.position += moveDir;

            Quaternion targetRot = Quaternion.LookRotation(move);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        // アニメーション更新（全員に反映）
        player.photonView.RPC("RPC_UpdateMoveAnimation", RpcTarget.All, isWalking, isRunning);
    }

    public void ExitState(PlayerController player)
    {
        if (player.photonView.IsMine)
        {
            player.photonView.RPC("RPC_UpdateMoveAnimation", RpcTarget.All, false, false);
        }
    }

    private void UpdateAnimation(PlayerController player)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = forward * z + right * x;

        bool isWalking = move.magnitude > 0.05f;
        bool isRunning = Input.GetKey("joystick button 2") && player.IsGrounded() || player.IsRun;

        player.photonView.RPC("RPC_UpdateMoveAnimation", RpcTarget.All, isWalking, isRunning);
    }
}
