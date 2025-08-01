using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
    {
    private Animator animator;

    void Start()
        {
        // Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();
        }

    void Update()
        {
        // �ړ����͔���iWASD�܂��͖��L�[�j
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;

        // �����́iQ�L�[�j
        bool isAvoidance = Input.GetKey(KeyCode.Q);

        bool isDashing = Input.GetKey(KeyCode.Z) && isMoving;

        // Animator�Ƀp�����[�^��n��
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAvoidance", isAvoidance);
        animator.SetBool("IsDashing", isDashing);
        }
    }
