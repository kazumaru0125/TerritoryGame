using Photon.Pun;
using UnityEngine;

public class GetOfuda : MonoBehaviourPun
    {
    public int Ofuda = 1;       // 加算スコア
    public int minusOfuda = 1;  // 減算スコア

    private void OnCollisionEnter(Collision collision)
        {
        // 自分のキャラのみ処理
        if (!photonView.IsMine) return;

        // --- お札取得処理 ---
        if (collision.gameObject.CompareTag("Ofuda"))
            {
            TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            // 鬼（Oni）はスコア加算しない
            if (role.CurrentRole == "Oni") return;

            // スコア加算を全員に同期
            photonView.RPC(nameof(AddOfudaScoreRPC), RpcTarget.All, role.CurrentTeam, Ofuda);

            // アイテム削除を MasterClient に依頼
            PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
            if (targetView != null)
                {
                photonView.RPC(nameof(RequestDestroyOfudaRPC), RpcTarget.MasterClient, targetView.ViewID);
                }
            }
        }

    private void OnTriggerStay(Collider other)
        {
        // 自分のキャラのみ処理
        if (!photonView.IsMine) return;

        // --- 攻撃判定でスコア減算 ---
        if (other.gameObject.CompareTag("AttackHitbox"))
            {
            TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            photonView.RPC(nameof(AddOfudaScoreRPC), RpcTarget.All, role.CurrentTeam, -minusOfuda);
            }
        }

    // --- RPC: スコア加算処理 ---
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

    // --- RPC: アイテム削除依頼 ---
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
