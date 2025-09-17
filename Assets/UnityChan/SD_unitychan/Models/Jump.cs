using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviour
    {
    [SerializeField] private float jumpForce = 7f;    // �W�����v��
    private bool isJumping = false;                   // �W�����v���t���O
    private Animator anim;
    private Rigidbody rb;
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } }

    private float groundCheckDistance = 0.3f;         // �n�ʔ���̋���
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    void Start()
        {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (anim == null) Debug.LogError("Animator��������܂���I");
        if (rb == null) Debug.LogError("Rigidbody��������܂���I");
        }

    void FixedUpdate()
        {
        // �n�ʔ���i���S��菭���ォ��^���փ��C���΂��j
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);

        // ���n���莞�ɃW�����v��ԏI��
        if (isGrounded && isJumping)
            {
            isJumping = false;
            }
        }

    void Update()
        {
        // �W�����v����
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 5")) && isGrounded && !isJumping)
            {
            if (anim != null) anim.Play("Jump", 0, 0);
            isJumping = true;

            // Rigidbody��Y���x�����Z�b�g
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;

            // �W�����v�͂𕨗��X�V�ɍ��킹�ăR���[�`���ŕt�^
            StartCoroutine(ApplyJumpForce());
            }
        }

    IEnumerator ApplyJumpForce()
        {
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        }
    }
