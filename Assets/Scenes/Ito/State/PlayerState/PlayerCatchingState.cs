using UnityEngine;

public class PlayerCatchingState : IPlayerState
{
    //private Animator anim;
    private bool isCatchingPlaying = false;
    private PlayerController playerController;

    public void EnterState(PlayerController player)
    {
        playerController = player;
        //anim = player.GetComponent<Animator>();
        //if (anim == null)
        //{
        //    Debug.LogError("Animatorが見つかりません！");
        //}
        isCatchingPlaying = false;
    }

    public void UpdateState(PlayerController player)
    {
        float rt = Input.GetAxis("RT");

        if (!isCatchingPlaying)
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
            {
                //anim.SetBool("is_caughting", true);
                isCatchingPlaying = true;
            }

            // ここで移動処理を呼び出すことで、キャッチング中以外移動できる
            // player.moveingState.UpdateState(player); ←移動用ステートを利用する場合
        }
        else
        {
            //// アニメーション終了判定
            //AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            //// 実際のキャッチアニメーション名(例: "Catch")で判定するのが安全
            //if (!stateInfo.IsName("Catch") || stateInfo.normalizedTime >= 1f)
            //{
            //    anim.SetBool("is_caughting", false);
            //    isCatchingPlaying = false;
            //    // キャッチ終了後にアイドル状態に戻す
            //    player.ChangeState(player.idelState);
            //}
        }
    }

    public void ExitState(PlayerController player)
    {
        //anim.SetBool("is_caughting", false);
        isCatchingPlaying = false;
    }
}
