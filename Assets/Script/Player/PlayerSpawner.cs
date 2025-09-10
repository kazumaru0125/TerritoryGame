using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
    [Header("Name Text UI")]
    public TMP_Text playerNicNameTextLD;
    public TMP_Text playerNicNameTextLU;
    public TMP_Text playerNicNameTextRD;
    public TMP_Text playerNicNameTextRU;

    void Start()
        {
        if (PhotonNetwork.InRoom) // ルームにいる場合
            {
            UpdatePlayerNameUI();
            SpawnPlayer();
            }
        }

    void UpdatePlayerNameUI()
        {
        // ルーム内のプレイヤー一覧を取得
        Player[] players = PhotonNetwork.PlayerList;

        // 各テキストを一旦クリア
        playerNicNameTextLD.text = "";
        playerNicNameTextLU.text = "";
        playerNicNameTextRD.text = "";
        playerNicNameTextRU.text = "";

        // インデックス管理用
        int otherIndex = 0;

        foreach (Player p in players)
            {
            if (p == PhotonNetwork.LocalPlayer)
                {
                // 自分はLU固定
                playerNicNameTextLU.text ="Me:"+p.NickName;
                }
            else
                {
                // 他プレイヤーを順番に割り当て
                switch (otherIndex)
                    {
                    case 0: playerNicNameTextLD.text = p.NickName; break;
                    case 1: playerNicNameTextRD.text = p.NickName; break;
                    case 2: playerNicNameTextRU.text = p.NickName; break;
                    }
                otherIndex++;
                }
            }
        }

    void SpawnPlayer()
        {
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
        PhotonNetwork.Instantiate("unitychan", pos, Quaternion.identity);
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
    }
