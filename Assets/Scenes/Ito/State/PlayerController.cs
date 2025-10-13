using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;

    public PlayerIdlingState idelState = new PlayerIdlingState();
    public PlayerMoveingState moveingState = new PlayerMoveingState();
    public PlayerJumpingState jumpingState = new PlayerJumpingState();
    public PlayerAttackingState attackingState = new PlayerAttackingState();
    public PlayerCatchingState catchingState = new PlayerCatchingState();

    public float moveSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (idelState != null)
        {
            currentState = idelState;
            currentState.EnterState(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }

 
        if (moveingState != null)
        {
            ChangeState(moveingState);
        }
        if (jumpingState != null)
        {
            ChangeState(jumpingState);
        }
        if (attackingState != null)
        {
            ChangeState(jumpingState);
        }
        if(catchingState != null)
        {
            ChangeState(catchingState);
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (newState != null && currentState != newState)
        {
            currentState?.ExitState(this);
            currentState = newState;
            currentState?.EnterState(this);
        }
    }
}
