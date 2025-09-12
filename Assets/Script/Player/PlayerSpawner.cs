using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
    [Header("Name Text UI")]
    public TMP_Text playerNicNameTextLD;
    public TMP_Text playerNicNameTextLU;
    public TMP_Text playerNicNameTextRD;
    public TMP_Text playerNicNameTextRU;

    private Transform[] spawnAreas;

    void Start()
        {
        // SpawnAreaタグを持つオブジェクトを全部取得
        GameObject[] spawnObjs = GameObject.FindGameObjectsWithTag("SpawnArea");
        spawnAreas = new Transform[spawnObjs.Length];
        for (int i = 0; i < spawnObjs.Length; i++)
            {
            spawnAreas[i] = spawnObjs[i].transform;
            }

        if (PhotonNetwork.InRoom) // ルームにいる場合
            {
            UpdatePlayerNameUI();
            SpawnPlayer();
            }
        }

    void UpdatePlayerNameUI()
        {
        // 各テキストを一旦クリア
        playerNicNameTextLD.text = "";
        playerNicNameTextLU.text = "";
        playerNicNameTextRD.text = "";
        playerNicNameTextRU.text = "";

        // 自分のチーム取得
        string myTeam = "None";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
            myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            }

        // 敵チーム表示用インデックス
        int enemyIndex = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
            {
            string team = p.CustomProperties.ContainsKey("Team") ? (string)p.CustomProperties["Team"] : "None";

            if (team == myTeam)
                {
                // 自分チーム
                if (p == PhotonNetwork.LocalPlayer)
                    {
                    playerNicNameTextLU.text =  p.NickName;
                    }
                else
                    {
                    playerNicNameTextLD.text = p.NickName;
                    }
                }
            else
                {
                // 敵チーム → 右側に配置
                switch (enemyIndex)
                    {
                    case 0: playerNicNameTextRD.text = p.NickName; break;
                    case 1: playerNicNameTextRU.text = p.NickName; break;
                    }
                enemyIndex++;
                }
            }
        }

    void SpawnPlayer()
        {
        Player[] players = PhotonNetwork.PlayerList;
        int myIndex = System.Array.IndexOf(players, PhotonNetwork.LocalPlayer);

        if (myIndex >= 0 && myIndex < spawnAreas.Length)
            {
            Transform spawnPoint = spawnAreas[myIndex];
            PhotonNetwork.Instantiate("unitychan", spawnPoint.position, spawnPoint.rotation);
            }
        else
            {
            Debug.LogWarning("スポーンポイントが足りません！");
            }
        }

    // --- 他プレイヤーが入退室した時も更新する ---
    public override void OnPlayerEnteredRoom(Player newPlayer)
        {
        UpdatePlayerNameUI();
        }

    public override void OnPlayerLeftRoom(Player otherPlayer)
        {
        UpdatePlayerNameUI();
        }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        if (changedProps.ContainsKey("Team"))
            {
            UpdatePlayerNameUI();
            }
        }
    }
