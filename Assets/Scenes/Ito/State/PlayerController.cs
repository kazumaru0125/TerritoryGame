using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;

    public PlayerIdlingState idelState = new PlayerIdlingState();
    public MoveingState moveingState = new MoveingState();

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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
            if (moveingState != null)
            {
                ChangeState(moveingState);
            }
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
