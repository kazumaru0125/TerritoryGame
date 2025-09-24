using Photon.Pun;
using UnityEngine;

public class GetOfuda : MonoBehaviourPun
    {
    public int Vitality = 1;

    private void OnCollisionEnter(Collision collision)
        {
        if (!photonView.IsMine) return; // 自分のキャラだけが処理

        if (collision.gameObject.CompareTag("Ofuda"))
            {
            PlayerRole role = GetComponent<PlayerRole>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            // スコア加算を全員に同期（RPC名をユニークに変更）
            photonView.RPC(nameof(AddOfudaScoreRPC), RpcTarget.All, role.CurrentTeam, Vitality);

            // アイテム削除を MasterClient に依頼
            PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
            if (targetView != null)
                {
                photonView.RPC(nameof(RequestDestroyOfudaRPC), RpcTarget.MasterClient, targetView.ViewID);
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
    }
