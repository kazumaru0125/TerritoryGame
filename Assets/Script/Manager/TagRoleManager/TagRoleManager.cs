using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TagRoleManager : MonoBehaviourPunCallbacks
    {
    [SerializeField] private int maxOni = 2;
    [SerializeField] private int maxRunner = 2;

    private List<Player> playerList = new List<Player>();

    void Start()
        {
        if (PhotonNetwork.IsMasterClient)
            {
            AssignInitialRoles();
            }
        }

    /// <summary>
    /// 鬼と人をランダムに割り当て、全員に通知
    /// </summary>
    void AssignInitialRoles()
        {
        playerList = new List<Player>(PhotonNetwork.PlayerList);

        // シャッフル
        for (int i = 0; i < playerList.Count; i++)
            {
            int rand = Random.Range(i, playerList.Count);
            var tmp = playerList[i];
            playerList[i] = playerList[rand];
            playerList[rand] = tmp;
            }

        int oniCount = 0;
        int runnerCount = 0;

        foreach (var p in playerList)
            {
            string role = "";
            if (oniCount < maxOni)
                {
                role = "Oni";
                oniCount++;
                }
            else if (runnerCount < maxRunner)
                {
                role = "Runner";
                runnerCount++;
                }

            // カスタムプロパティで各プレイヤーに役割を記録
            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
            ht["Role"] = role;
            p.SetCustomProperties(ht);
            }
        }

    /// <summary>
    /// 鬼が人を捕まえた時に呼ばれる（MasterClientのみが処理）
    /// </summary>
    [PunRPC]
    public void SwapRoles(int oniId, int runnerId)
        {
        Player oni = GetPlayerById(oniId);
        Player runner = GetPlayerById(runnerId);

        if (oni == null || runner == null) return;

        // 入れ替え
        SetRole(oni, "Runner");
        SetRole(runner, "Oni");
        }

    private void SetRole(Player p, string role)
        {
        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable();
        ht["Role"] = role;
        p.SetCustomProperties(ht);
        }

    private Player GetPlayerById(int id)
        {
        foreach (var p in PhotonNetwork.PlayerList)
            {
            if (p.ActorNumber == id) return p;
            }
        return null;
        }
    }
