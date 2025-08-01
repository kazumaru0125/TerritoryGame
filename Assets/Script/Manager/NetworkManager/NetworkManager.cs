using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
    {
    void Start()
        {
        PhotonNetwork.ConnectUsingSettings();
        }

    public override void OnConnectedToMaster()
        {
        PhotonNetwork.JoinRandomRoom();
        }

    public override void OnJoinRandomFailed(short returnCode, string message)
        {
        var option = new RoomOptions();
        option.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, option);
        }

    public override void OnJoinedRoom()
        {
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
        GameObject player = PhotonNetwork.Instantiate("akai", pos, Quaternion.identity);
        }
    }
