using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
    {
    // ジャンプする力の大きさ
    [SerializeField] private float jumpForce = 7f;
    // 現在ジャンプ中かどうかのフラグ
    private bool isJumping = false;
    private Animator anim;
    private Rigidbody rb;
    // プレイヤーが地面にいるかどうか
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }
    // 地面判定に使う距離
    private float groundCheckDistance = 0.3f;
    // 地面判定を少し上から出すオフセット
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    void Start()
        {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        // AnimatorまたはRigidbodyがセットされていない場合エラーメッセージを表示
        if (anim == null) Debug.LogError("Animatorコンポーネントが見つかりません！");
        if (rb == null) Debug.LogError("Rigidbodyコンポーネントが見つかりません！");
        }

    void FixedUpdate()
        {
        // Raycastで足元に地面があるかチェックし、isGroundedに結果を入れる
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);

        // 着地したらジャンプ状態を解除
        if (isGrounded && isJumping)
            {
            isJumping = false;
            }
        }

    void Update()
        {
        // スペースキーまたはコントローラのRTボタンが押され、地面にいてジャンプ中でなければジャンプ実行
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded && !isJumping)
            {
            // ジャンプアニメーション再生
            if (anim != null) anim.Play("Jump", 0, 0);
            isJumping = true;

            // ジャンプ前にRigidbodyのY方向速度をリセット
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;

            // FixedUpdateの後で力を加えるためCoroutineで遅延
            StartCoroutine(ApplyJumpForce());
            }
        }

    // ジャンプの力を物理的に加える（FixedUpdate後に実行するためコルーチン）
    IEnumerator ApplyJumpForce()
        {
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }
