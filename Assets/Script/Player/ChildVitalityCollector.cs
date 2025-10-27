using UnityEngine;
using Photon.Pun;

public class ChildVitalityCollector : MonoBehaviour
    {
    public int Vitality = 1; // この子が取ったときのスコア

    private void OnCollisionEnter(Collision collision)
        {
        if (collision.gameObject.CompareTag("vitality"))
            {
            TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            // Oni は加算しない
            if (role.CurrentRole == "Oni") return;

            PhotonView parentView = role.photonView;
            if (parentView == null || !parentView.IsMine) return;

            // スコア加算 RPC
            if (parentView != null)
                parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, Vitality);

            // アイテム削除 RPC
            PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
            if (targetView != null)
                {
                if (parentView != null)
                    parentView.RPC("RequestDestroyRPC", RpcTarget.MasterClient, targetView.ViewID);
                }
            }
        }
    }
