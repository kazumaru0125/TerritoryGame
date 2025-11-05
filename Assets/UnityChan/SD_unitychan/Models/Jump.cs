using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Jump : MonoBehaviourPun
    {
    [SerializeField] private float jumpForce = 7f;
    private bool isJumping = false;
    private Animator anim;
    private Rigidbody rb;
    public bool isGrounded = false;
    private float groundCheckDistance = 0.3f;
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    void Start()
        {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        }

    void FixedUpdate()
        {
        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);
        if (isGrounded && isJumping)
            isJumping = false;
        }

    void Update()
        {
        if (!photonView.IsMine)
            return;

        // 入力とジャンプ条件
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded && !isJumping)
            {
            anim.applyRootMotion = false;

            isJumping = true;
            photonView.RPC(nameof(PlayJumpAnimation), RpcTarget.All); // ✅ 全クライアントへ送信

            // 物理ジャンプ
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0;
            rb.linearVelocity = velocity;
            StartCoroutine(ApplyJumpForce());
            }
        }

    [PunRPC]
    void PlayJumpAnimation()
        {
        if (anim != null)
            anim.Play("Jump", 0, 0);
        }

    IEnumerator ApplyJumpForce()
        {
        yield return new WaitForFixedUpdate();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }
