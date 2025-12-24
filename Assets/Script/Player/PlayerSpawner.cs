using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        StartCoroutine(InitRoutine());
        }

    IEnumerator InitRoutine()
        {
        // Photon が部屋接続完了するまで待つ
        while (!PhotonNetwork.InRoom)
            {
            yield return null;
            }

        // SpawnArea 検索
        GameObject[] spawnObjs = GameObject.FindGameObjectsWithTag("SpawnArea");
        while (spawnObjs.Length == 0)
            {
            Debug.Log("SpawnArea がまだ見つからない。待機中…");
            yield return null;
            spawnObjs = GameObject.FindGameObjectsWithTag("SpawnArea");
            }

        spawnAreas = new Transform[spawnObjs.Length];
        for (int i = 0; i < spawnObjs.Length; i++)
            {
            spawnAreas[i] = spawnObjs[i].transform;
            }

        // PlayerList が揃うまで待つ
        while (PhotonNetwork.PlayerList.Length == 0)
            {
            yield return null;
            }

        UpdatePlayerNameUI();
        SpawnPlayer();
        }

    void UpdatePlayerNameUI()
        {
        if (!playerNicNameTextLD || !playerNicNameTextLU || !playerNicNameTextRD || !playerNicNameTextRU)
            {
            Debug.LogWarning("Player Name UI がセットされていません");
            return;
            }

        playerNicNameTextLD.text = "";
        playerNicNameTextLU.text = "";
        playerNicNameTextRD.text = "";
        playerNicNameTextRU.text = "";

        string myTeam = "None";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];

        int enemyIndex = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
            {
            string team = p.CustomProperties.ContainsKey("Team") ?
                          (string)p.CustomProperties["Team"] : "None";

            if (team == myTeam)
                {
                if (p == PhotonNetwork.LocalPlayer)
                    playerNicNameTextLU.text = p.NickName;
                else
                    playerNicNameTextLD.text = p.NickName;
                }
            else
                {
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

        if (spawnAreas == null || spawnAreas.Length == 0)
            {
            Debug.LogError("SpawnArea がありません！");
            return;
            }

        if (myIndex >= 0 && myIndex < spawnAreas.Length)
            {
            Transform spawnPoint = spawnAreas[myIndex];
            PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
            }
        else
            {
            Debug.LogWarning("スポーンポイントが足りません！");
            }
        }

    public override void OnPlayerEnteredRoom(Player newPlayer)
        => UpdatePlayerNameUI();

    public override void OnPlayerLeftRoom(Player otherPlayer)
        => UpdatePlayerNameUI();

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        if (changedProps.ContainsKey("Team"))
            UpdatePlayerNameUI();
        }
    }
