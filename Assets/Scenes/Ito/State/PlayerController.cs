using Photon.Pun;  // Photon用に追加
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    private IPlayerState currentState;

    public PlayerIdlingState idelState = new PlayerIdlingState();
    public PlayerMoveingState moveingState = new PlayerMoveingState();
    public PlayerJumpingState jumpingState = new PlayerJumpingState();
    public PlayerAttackingState attackingState = new PlayerAttackingState();
    public PlayerCatchingState catchingState = new PlayerCatchingState();

    public float moveSpeed = 5.0f;

    void Start()
    {
        // 自分のプレイヤーのみ初期化
        if (!photonView.IsMine) return;

        if (idelState != null)
        {
            currentState = idelState;
            currentState.EnterState(this);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;  // 自分のプレイヤーのみ操作許可

        if (currentState != null)
        {
            currentState.UpdateState(this);
        }

        // ジャンプキーが押されたらジャンプ状態に遷移
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {
            ChangeState(jumpingState);
            //return;
        }

        // 攻撃キーが押されたら攻撃状態に遷移
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1"))
        {
            ChangeState(attackingState);
            return;
        }

        // キャッチキーが押されたらキャッチ状態に遷移
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState(catchingState);
            return;
        }

        // もし現在の状態がIdleで、移動入力があればMoveingStateに切り替える例
        if (currentState == idelState)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
            {
                ChangeState(moveingState);
                return;
            }
        }

        // 移動状態から入力なしでIdle状態に戻す例
        if (currentState == moveingState)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (Mathf.Abs(x) < 0.1f && Mathf.Abs(z) < 0.1f)
            {
                ChangeState(idelState);
                return;
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
