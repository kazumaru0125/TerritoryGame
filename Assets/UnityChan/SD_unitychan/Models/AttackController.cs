using System.Collections;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 1f;      // 少しジャンプする力
    private Rigidbody rb;
    Animator anim;
    bool isAttackPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();   // << 必ず初期化！
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");
        if (rb == null)
            Debug.LogError("Rigidbodyが見つかりません！");
    }

    void Update()
    {
        float rt = Input.GetAxis("RT");

        if (!isAttackPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
            {
                // ジャンプ前にY方向速度リセット
                Vector3 velocity = rb.linearVelocity;
                velocity.y = 0;
                rb.linearVelocity = velocity;
                StartCoroutine(ApplyJumpForce());
                anim.SetBool("is_attacking", true);
                isAttackPlaying = true;
            }
        }
        else
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            // ステート名はAnimator内で設定したもの（例:"Attack"）に揃える
            if (!stateInfo.IsName("Attack") || stateInfo.normalizedTime >= 1f)
            {
                anim.SetBool("is_attacking", false);
                isAttackPlaying = false;
            }
        }
    }

    IEnumerator ApplyJumpForce()
    {
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
    }
}
