using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
    public TMP_Text playerNicNameText;
    void Start()
        {
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("プレイヤー名: " + playerName);
        // --- RoomInfoText ---
        if (playerNicNameText != null)
            {
            playerNicNameText.text = playerName;

            }
        else
            {
            Debug.LogWarning("playerNicNameText が Inspector に設定されていません！");
            }
        if (PhotonNetwork.InRoom) // すでにルームにいる場合
            {
            SpawnPlayer();
            }
        }

    void SpawnPlayer()
        {
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
        PhotonNetwork.Instantiate("unitychan", pos, Quaternion.identity);
        }
    }
