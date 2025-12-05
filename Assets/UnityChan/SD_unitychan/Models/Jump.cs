using Photon.Pun;
using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviourPun, IPunObservable
    {
    [SerializeField] private float jumpForce = 7f;
    private bool isJumping = false;
    private Animator anim;
    private Rigidbody rb;
    public bool isGrounded = false;

    private float groundCheckDistance = 0.3f;
    private Vector3 groundCheckOffset = Vector3.up * 0.1f;

    private Vector3 networkVelocity;

    void Start()
        {
        anim = GetComponent<Animator>();
        anim.applyRootMotion = false;

        rb = GetComponent<Rigidbody>();
        }

    void FixedUpdate()
        {
        if (!photonView.IsMine)
            {
            // 他プレイヤーは速度を補間するだけ
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, networkVelocity, 0.5f);
            return;
            }

        isGrounded = Physics.Raycast(transform.position + groundCheckOffset, Vector3.down, groundCheckDistance);
        if (isGrounded && isJumping)
            isJumping = false;
        }

    void Update()
        {
        if (!photonView.IsMine)
            return;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && isGrounded && !isJumping)
            {
            isJumping = true;
            photonView.RPC(nameof(PlayJumpAnimation), RpcTarget.All);
            StartCoroutine(ApplyJumpForce());
            }
        }

    [PunRPC]
    void PlayJumpAnimation()
        {
        anim.Play("Jump", 0, 0);
        }

    IEnumerator ApplyJumpForce()
        {
        yield return new WaitForFixedUpdate();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            stream.SendNext(rb.linearVelocity);
        else
            networkVelocity = (Vector3)stream.ReceiveNext();
        }
    }
