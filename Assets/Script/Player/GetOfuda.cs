using Photon.Pun;
using UnityEngine;
using System.Collections;

public class GetOfuda : MonoBehaviourPun
    {
    public int Vitality = 1; // Šƒ|ƒCƒ“ƒg
    public float invincibleTime = 2f; // ’D‚í‚êŒã‚Ì–³“GŠÔ
    private bool isInvincible = false;

    private Collider col;

    private void Start()
        {
        col = GetComponent<Collider>();
        }

    private void OnCollisionEnter(Collision collision)
        {
        if (!photonView.IsMine) return; // ©•ª‚ÌƒLƒƒƒ‰‚¾‚¯ˆ—

        // --- Ofudaæ“¾ ---
        if (collision.gameObject.CompareTag("Ofuda"))
            {
            PlayerRole role = GetComponent<PlayerRole>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            photonView.RPC(nameof(AddOfudaScoreRPC), RpcTarget.All, role.CurrentTeam, Vitality);

            PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
            if (targetView != null)
                {
                photonView.RPC(nameof(RequestDestroyOfudaRPC), RpcTarget.MasterClient, targetView.ViewID);
                }
            }

        // --- ƒvƒŒƒCƒ„[“¯m‚Ì’Dæ ---
        if (collision.gameObject.CompareTag("Player"))
            {
            if (isInvincible) return; // ©•ª‚ª–³“G‚È‚ç’D‚í‚ê‚È‚¢

            GetOfuda other = collision.gameObject.GetComponent<GetOfuda>();
            PlayerRole myRole = GetComponent<PlayerRole>();

            if (other != null && myRole != null && !string.IsNullOrEmpty(myRole.CurrentTeam))
                {
                if (other.Vitality > 0 && !other.isInvincible)
                    {
                    // 1ƒ|ƒCƒ“ƒg’D‚¤RPC‚ğŒÄ‚Ô
                    photonView.RPC(nameof(StealOfudaRPC), RpcTarget.All, myRole.CurrentTeam, other.photonView.ViewID);
                    }
                }
            }
        }

    [PunRPC]
    private void AddOfudaScoreRPC(string team, int value)
        {
        OfudaCount manager = FindObjectOfType<OfudaCount>();
        if (manager == null) return;

        if (team == "A")
            manager.AddATeamVitality(value);
        else if (team == "B")
            manager.AddBTeamVitality(value);
        }

    [PunRPC]
    private void RequestDestroyOfudaRPC(int viewID)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
            {
            PhotonNetwork.Destroy(pv.gameObject);
            }
        }

    // --- ’Dæˆ— ---
    [PunRPC]
    private void StealOfudaRPC(string thiefTeam, int victimViewID)
        {
        PhotonView victimPV = PhotonView.Find(victimViewID);
        if (victimPV == null) return;

        GetOfuda victim = victimPV.GetComponent<GetOfuda>();
        if (victim != null && victim.Vitality > 0)
            {
            // ”íŠQÒ -1
            victim.Vitality -= 1;

            // –³“GŠÔ•t—^
         //   victim.StartCoroutine(victim.SetInvincible());

            // ƒ`[ƒ€ƒXƒRƒA‰ÁZ
            OfudaCount manager = FindObjectOfType<OfudaCount>();
            if (manager != null)
                {
                if (thiefTeam == "A")
                    manager.AddATeamVitality(1);
                else if (thiefTeam == "B")
                    manager.AddBTeamVitality(1);
                }
            }
        }

    // --- –³“GŠÔˆ— ---
    //private IEnumerator SetInvincible()
    //    {
    //    isInvincible = true;
    //    //if (col != null) col.enabled = false; // “–‚½‚è”»’èOFF

    //    //yield return new WaitForSeconds(invincibleTime);

    //    //if (col != null) col.enabled = true; // “–‚½‚è”»’èON
    //    //isInvincible = false;
    //    }
    }
