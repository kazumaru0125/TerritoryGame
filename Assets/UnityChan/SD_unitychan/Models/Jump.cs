using UnityEngine;

public class Jump : MonoBehaviour
{
    float jumpForce = 7;
    bool isJumpWait;
    Animator anim; // Animator
    float jumpWaitTimer;
    bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("Animatorが見つかりません！");
        if (GetComponent<Rigidbody>() == null) Debug.LogError("Rigidbodyが見つかりません！");
    }

    void FixedUpdate()
    {
        // 地面判定のRaycastの距離を調整（距離をもう少し短くしたりレイの開始点を微調整すると効果的）
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
    }

    void Update()
    {
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
        }
    }
}
