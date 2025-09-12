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
            PlayerRole role = GetComponent<PlayerRole>();
            if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

            // スコア加算を全員に同期
            photonView.RPC(nameof(AddScoreRPC), RpcTarget.All, role.CurrentTeam, Vitality);

            // アイテムを全員から削除
            PhotonNetwork.Destroy(collision.gameObject);
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
    }
