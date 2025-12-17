using UnityEngine;
using Photon.Pun;

public class DisappointmentController : MonoBehaviourPun
    {
    //private bool isPlaying = false;
    //private TestPlayerRoll playerRoll;

    //void Awake()
    //    {
    //    playerRoll = GetComponentInParent<TestPlayerRoll>();

    //    if (playerRoll == null)
    //        Debug.LogError("TestPlayerRoll ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
    //    }

    //// ==============================
    //// ŠO•”ŒöŠJAPI
    //// ==============================
    //public void Play()
    //    {
    //    if (isPlaying) return;
    //    if (!photonView.IsMine) return;

    //    isPlaying = true;
    //    photonView.RPC(nameof(RPC_PlayAnimation), RpcTarget.All);
    //    }

    //[PunRPC]
    //private void RPC_PlayAnimation()
    //    {
    //    // š RPC‘¤‚Å•K‚¸ Animator ‚ðŽæ“¾‚·‚é
    //    Animator anim = playerRoll.GetCurrentAnimator();
    //    if (anim == null)
    //        {
    //        Debug.LogError("RPC_PlayAnimation: Animator ‚ªŽæ“¾‚Å‚«‚Ü‚¹‚ñ");
    //        return;
    //        }

    //    anim.SetBool("is_Disappointmenting", true);
    //    }

    //// ==============================
    //// Animation Event
    //// ==============================
    //public void OnDisappointmentFinished()
    //    {
    //    Animator anim = playerRoll.GetCurrentAnimator();
    //    if (anim != null)
    //        anim.SetBool("is_Disappointmenting", false);

    //    if (photonView.IsMine)
    //        {
    //        playerRoll.ExecuteFadeRoleChange(() =>
    //        {
    //            isPlaying = false;
    //        });
    //        }
    //    else
    //        {
    //        isPlaying = false;
    //        }
    //    }
    }
