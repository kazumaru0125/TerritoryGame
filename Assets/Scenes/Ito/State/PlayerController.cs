using Photon.Pun;  // Photon—p‚É’Ç‰Á
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public float JumpForce = 9f;
    public float RunSpeed = 14.0f;
    public float WalkSpeed = 7.5f;

    public bool IsRun { get; private set; }

    private IPlayerState currentState;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        ChangeState(new PlayerMoveingState());
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 8"))
            IsRun = !IsRun;

        currentState?.UpdateState(this);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && IsGrounded())
            ChangeState(new PlayerJumpingState());
        else if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1"))
            ChangeState(new PlayerAttackingState());
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

}
