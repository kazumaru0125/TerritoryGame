using UnityEngine;
using Photon.Pun;
public class PlayerTrapDameageState : IPlayerState
    {
    private bool NoMoveflag = false; // エフェクト生成フラグ

    public void EnterState(PlayerController player)
        {
        NoMoveflag = false;
        }

    public void UpdateState(PlayerController player)
        {
        AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);

        }

    private void SpawnAttackEffect(PlayerController player)
        {
        if (!player.photonView.IsMine) return;

       
        }

    private void EndAttack(PlayerController player)
        {
        if (player.photonView.IsMine)
            {
            player.photonView.RPC("RPC_SetAttackState", RpcTarget.All, false);
            }


        player.ChangeState(new PlayerMoveingState());
        }

    public void ExitState(PlayerController player)
        {

        }
    }

