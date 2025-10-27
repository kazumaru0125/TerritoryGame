using UnityEngine;
using Photon.Pun;

public class GetVitalityScriput : MonoBehaviourPun
    {
    public int Vitality = 1;

    private void OnCollisionEnter(Collision collision)
        {
        if (!photonView.IsMine) return; // 自分のキャラだけが処理

        if (collision.gameObject.CompareTag("vitality"))
            {
            //PlayerRole role = GetComponent<PlayerRole>();
            TestPlayerRoll role = GetComponent<TestPlayerRoll>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            // スコア加算を全員に同期
            photonView.RPC(nameof(AddScoreRPC), RpcTarget.All, role.CurrentTeam, Vitality);

            // アイテム削除を MasterClient に依頼
            PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
            if (targetView != null)
                {
                photonView.RPC(nameof(RequestDestroyRPC), RpcTarget.MasterClient, targetView.ViewID);
                }
            }
        }

    [PunRPC]
    private void AddScoreRPC(string team, int value)
        {
        DecreaseTMPNumber manager = FindObjectOfType<DecreaseTMPNumber>();
        if (manager == null) return;

        if (team == "A")
            manager.AddATeamVitality(value);
        else if (team == "B")
            manager.AddBTeamVitality(value);
        }

    [PunRPC]
    private void RequestDestroyRPC(int viewID)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null && pv.gameObject != null)
            {
            PhotonNetwork.Destroy(pv.gameObject);
            }
        }

    }
