using UnityEngine;

public class PlayerMoveingState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // 現在の移動・走り状態を即時反映
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = forward * z + right * x;
        bool isWalking = move.magnitude > 0.05f;
        bool isRunning = player.IsRun && (isWalking || player.Animator.GetBool("is_running"));

        if (isRunning)
        {
            player.Animator.SetBool("is_running", true);
            player.Animator.SetBool("is_walking", false);
        }
        else if (isWalking)
        {
            player.Animator.SetBool("is_walking", true);
            player.Animator.SetBool("is_running", false);
        }
        else
        {
            player.Animator.SetBool("is_running", false);
            player.Animator.SetBool("is_walking", false);
        }
    }


    public void UpdateState(PlayerController player)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0; right.y = 0;
        forward.Normalize(); right.Normalize();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = forward * z + right * x;

        bool isWalking = move.magnitude > 0.05f;
        bool isRunning = player.IsRun && isWalking;

        // 地面判定
        bool isGrounded = player.IsGrounded();

        // 一時的なダッシュ（ボタン押しっぱなし）
        bool isDash = Input.GetKey("joystick button 2") && isGrounded;
        // 走り「トグル」：PlayerControllerのIsRun（LeftShiftトグル）
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

        // Animator用パラメータを確実にセット
        if(!isWalking)
        {
            player.Animator.SetBool("is_walking", false);
            player.Animator.SetBool("is_running", isRunning);
        }
        else if (isWalking && !isRunning)
        {
            player.Animator.SetBool("is_walking", true);
            player.Animator.SetBool("is_running", isRunning);
        }
        else
        player.Animator.SetBool("is_running", isRunning);
        player.Animator.SetBool("is_attacking", false);
    }

    public void ExitState(PlayerController player)
    {
        player.Animator.SetBool("is_walking", false);
        player.Animator.SetBool("is_running", false);
    }
}
