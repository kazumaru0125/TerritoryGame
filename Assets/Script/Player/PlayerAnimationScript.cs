using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
    {
    private Animator animator;

    void Start()
        {
        // Animatorコンポーネントを取得
        animator = GetComponent<Animator>();
        }

    void Update()
        {
        // 移動入力判定（WASDまたは矢印キー）
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;

        // 回避入力（Qキー）
        bool isAvoidance = Input.GetKey(KeyCode.Q);

        bool isDashing = Input.GetKey(KeyCode.Z) && isMoving;

        // Animatorにパラメータを渡す
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAvoidance", isAvoidance);
        animator.SetBool("IsDashing", isDashing);
        }
    }
