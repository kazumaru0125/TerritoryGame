using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
    {
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public float JumpForce = 9f;
    public float RunSpeed = 14.0f;
    public float WalkSpeed = 7.5f;
    public int Stamina = 100;

    public GameObject HetBox;

    public bool IsRun { get; private set; }

    private IPlayerState currentState;
    private bool isAttackTriggered = false; // 攻撃トリガーフラグ

    void Start()
        {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        ChangeState(new PlayerMoveingState());
        HetBox.SetActive(false);
        }

    void Update()
        {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 8"))
            IsRun = !IsRun;

        // 攻撃トリガー（攻撃中は二重に遷移しない）
        if (!isAttackTriggered && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1")))
            {
            isAttackTriggered = true;
            ChangeState(new PlayerAttackingState());
            HetBox.SetActive(true);
            }
        else
            {
            HetBox.SetActive(false);
            }

        // 状態更新
        currentState?.UpdateState(this);

        // 攻撃終了でトリガーフラグを解除
        if (currentState is PlayerMoveingState)
            isAttackTriggered = false;

        // ジャンプトリガーは攻撃フラグが立ってないときのみ許可
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && IsGrounded() && !(currentState is PlayerAttackingState))
            ChangeState(new PlayerJumpingState());
        }

    public void ChangeState(IPlayerState newState)
        {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
        }

    public bool IsGrounded()
        {
        float checkDistance = 0.3f;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, checkDistance);
        }

    [PunRPC]
    public void RPC_SetAttackState(bool isAttacking)
        {
        Animator.SetBool("is_attacking", isAttacking);
        }

    [PunRPC]
    public void RPC_UpdateMoveAnimation(bool isWalking, bool isRunning)
        {
        Animator.SetBool("is_walking", isWalking && !isRunning);
        Animator.SetBool("is_running", isRunning);
        }

    [PunRPC]
    public void RPC_PlayJumpAnimation()
        {
        Animator.Play("Jump", 0, 0);
        }



    }
