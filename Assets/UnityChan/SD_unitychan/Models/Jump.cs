<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> UnitytyanMovement
using UnityEngine;

public class Jump : MonoBehaviour
{
<<<<<<< HEAD
    float jumpForce = 6;//ヒトなら/6アイテム使用時OR鬼なら7
    bool isJumpWait;
    Animator anim; // Animator
    float jumpWaitTimer;
    bool isGrounded;
=======
    [SerializeField] private float jumpForce = 7f;    // ジャンプ力
    private bool isJumping = false;                   // ジャンプ中フラグ
    private Animator anim;
    private Rigidbody rb;
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }

    private float groundCheckDistance = 0.3f;         // 地面判定の距離
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;
>>>>>>> UnitytyanMovement

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (anim == null) Debug.LogError("Animatorが見つかりません！");
        if (rb == null) Debug.LogError("Rigidbodyが見つかりません！");
    }

    void FixedUpdate()
    {
<<<<<<< HEAD
        // 地面判定のRaycastの距離を調整（距離をもう少し短くしたりレイの開始点を微調整すると効果的）
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
=======
        // 地面判定（中心より少し上から真下へレイを飛ばす）
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);

        // 着地判定時にジャンプ状態終了
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }
>>>>>>> UnitytyanMovement
    }

    void Update()
    {
<<<<<<< HEAD
        // ジャンプ入力かつ地面にいてジャンプ待機中でなければジャンプ開始
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 5")) && isGrounded && !isJumpWait)
        {
            if (anim != null) anim.Play("Jump", 0, 0);
            isJumpWait = true;
            jumpWaitTimer = 0.3f;
        }

        if (isJumpWait)
        {
            jumpWaitTimer -= Time.deltaTime;
            if (jumpWaitTimer < 0)
            {
                // ジャンプ開始時にRigidbodyの速度をリセットしてから力を加える（これが重要）
                var rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;  // 速度リセット
                    rb.linearVelocity = transform.up * jumpForce;  // ジャンプの初速をセット
                }

                isJumpWait = false;
            }
=======
        // ジャンプ入力
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 5")) && isGrounded && !isJumping)
        {
            if (anim != null) anim.Play("Jump", 0, 0);
            isJumping = true;

            // RigidbodyのY速度をリセット（警告回避は下参照）
            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.velocity = velocity;

            // ジャンプ力は物理更新に合わせてコルーチンで付与
            StartCoroutine(ApplyJumpForce());
>>>>>>> UnitytyanMovement
        }
    }

    IEnumerator ApplyJumpForce()
    {
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }
}
