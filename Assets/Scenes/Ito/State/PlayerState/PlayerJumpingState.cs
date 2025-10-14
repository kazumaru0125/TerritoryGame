using System.Collections;
using UnityEngine;

public class PlayerJumpingState : IPlayerState
{
    // ジャンプする力の大きさ
    private float jumpForce = 9f;
    // 現在ジャンプ中かどうかのフラグ
    private bool isJumping = false;
   // private Animator anim;
    private Rigidbody rb;
    // プレイヤーが地面にいるかどうか
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }
    // 地面判定に使う距離
    private float groundCheckDistance = 0.3f;
    // 地面判定を少し上から出すオフセット
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    // PlayerControllerの参照を保持
    private PlayerController playerController;

    public void EnterState(PlayerController player)
    {
        Debug.Log("ジャンピングステート開始");
        playerController = player;
        //anim = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();

        // AnimatorまたはRigidbodyがセットされていない場合エラーメッセージを表示
        //if (anim == null) Debug.LogError("Animatorコンポーネントが見つかりません！");
        if (rb == null) Debug.LogError("Rigidbodyコンポーネントが見つかりません！");
    }

    //更新処理
    public void UpdateState(PlayerController player)
    {
        // 地面チェック
        CheckGrounded(player);

        // 着地したらジャンプ状態を解除
        if (isGrounded && isJumping)
        {
            isJumping = false;
            // アイドル状態に戻る
            player.ChangeState(player.idelState);
          
            return;
        }

        // スペースキーまたはコントローラのRTボタンが押され、地面にいてジャンプ中でなければジャンプ実行
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded && !isJumping)
        {
            // ジャンプアニメーション再生
            //if (anim != null) anim.Play("Jump", 0, 0);
            isJumping = true;

            // ジャンプ前にRigidbodyのY方向速度をリセット
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;

            // ジャンプの力を加える
            player.StartCoroutine(ApplyJumpForce(player));
        }
    }

    // 地面チェック処理
    private void CheckGrounded(PlayerController player)
    {
        // Raycastで足元に地面があるかチェックし、isGroundedに結果を入れる
        isGrounded = Physics.Raycast(player.transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);
    }

    //終了
    public void ExitState(PlayerController player)
    {
        Debug.Log("ジャンピングステート終了");
    }

    IEnumerator ApplyJumpForce(PlayerController player)
    {
        yield return new WaitForFixedUpdate();
        rb.AddForce(player.transform.up * jumpForce, ForceMode.VelocityChange);
    }
}
