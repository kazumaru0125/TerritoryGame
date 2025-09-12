using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;    // ジャンプ力
    private bool isJumping = false;                   // ジャンプ中フラグ
    private Animator anim;
    private Rigidbody rb;
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }

    private float groundCheckDistance = 0.3f;         // 地面判定の距離
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (anim == null) Debug.LogError("Animatorが見つかりません！");
        if (rb == null) Debug.LogError("Rigidbodyが見つかりません！");
    }

    void FixedUpdate()
    {
        // 地面判定（中心より少し上から真下へレイを飛ばす）
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);

        // 着地判定時にジャンプ状態終了
        if (isGrounded && isJumping)
        {
            isJumping = false;
        }
    }

    void Update()
    {
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
        }
    }

    IEnumerator ApplyJumpForce()
    {
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }
}
