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
                playerNicNameTextLU.text = "Me:" + p.NickName;
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
        Player[] players = PhotonNetwork.PlayerList;
        int myIndex = System.Array.IndexOf(players, PhotonNetwork.LocalPlayer);

        if (myIndex >= 0 && myIndex < spawnAreas.Length)
        {
            Transform spawnPoint = spawnAreas[myIndex];
          //  PhotonNetwork.Instantiate("akai", spawnPoint.position, spawnPoint.rotation);
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
}
