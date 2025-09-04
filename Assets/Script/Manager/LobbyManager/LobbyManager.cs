using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
    {
    [Header("Connection Status")]
    public TMP_Text connectionStatusText;

    [Header("Login UI Panel")]
    public InputField playerNameInput;
    public GameObject Login_UI_Panel;


    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    public InputField roomNameInputField;

    //public InputField maxPlayerInputField;

    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;
    public TMP_Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerListContent;
    public GameObject startGameButton;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject roomListEntryPrefab;
    public GameObject roomListParentGameObject;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;

    #region Unity Methods

    // Start is called before the first frame update
    private void Start()
        {
        // Login_UI_Panel.SetActive(true);
        // GameOptions_UI_Panel.SetActive(false);
        ActivatePanel(Login_UI_Panel.name);

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;
        }

    // Update is called once per frame
    private void Update()
        {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;
        }

    #endregion

    #region UI Callbacks
    public void OnLoginButtonClicked()
        {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
            {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();

            }
        else
            {
            Debug.Log("PlayerName is invalid!");
            }

        }

    public void OnCreateRoomButtonClicked()
        {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
            {
            roomName = "Room " + Random.Range(1000, 10000);
            }

        RoomOptions roomOptions = new RoomOptions();
        // roomOptions.MaxPlayers = (byte)int.Parse(maxPlayerInputField.text);
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

    public void OnCancelButtonClicked()
        {
        ActivatePanel(GameOptions_UI_Panel.name);
        }

    public void OnShowRoomListButtonClicked()
        {
        if (!PhotonNetwork.InLobby)
            {
            PhotonNetwork.JoinLobby();
            }

        ActivatePanel(RoomList_UI_Panel.name);
        }

    public void OnBackButtonClicked()
        {
        if (PhotonNetwork.InLobby)
            {
            PhotonNetwork.LeaveLobby();
            }
        ActivatePanel(GameOptions_UI_Panel.name);
        }

    public void OnLeaveGameButtonClicked()
        {
        PhotonNetwork.LeaveRoom();
        }

    public void OnJoinRandomRoomButtonClicked()
        {
        ActivatePanel(JoinRandomRoom_UI_Panel.name);
        PhotonNetwork.JoinRandomRoom();
        }

    public void OnStartGameButtonClicked()
        {
        if (PhotonNetwork.IsMasterClient)
            {
             PhotonNetwork.LoadLevel("TGameScene");
            //PhotonNetwork.LoadLevel("SampleScene");
            }
        }


    #endregion

    #region Photon Callbacks
    public override void OnConnected()
        {
        Debug.Log("Connected to Internet");
        }
    public override void OnConnectedToMaster()
        {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is connected to Photon");
        ActivatePanel(GameOptions_UI_Panel.name);

        }

    public override void OnCreatedRoom()
        {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
        }

    //public override void OnJoinedRoom()
    //    {
    //    Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    //    ActivatePanel(InsideRoom_UI_Panel.name);

    //    if (PhotonNetwork.LocalPlayer.IsMasterClient)
    //        {
    //        startGameButton.SetActive(true);
    //        }
    //    else
    //        {
    //        startGameButton.SetActive(false);
    //        }

    //    roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " + "Players/Max.players:" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

    //    if (playerListGameObjects == null)
    //        {
    //        playerListGameObjects = new Dictionary<int, GameObject>();
    //        }

    //    foreach (Player player in PhotonNetwork.PlayerList)
    //        {
    //        GameObject playerListGameObject = Instantiate(playerListPrefab);
    //        playerListGameObject.transform.SetParent(playerListContent.transform);
    //        playerListGameObject.transform.localScale = Vector3.one;

    //        playerListGameObject.transform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;
    //        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
    //            {
    //            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(true);
    //            }
    //        else
    //            {
    //            playerListGameObject.transform.Find("PlayerIndicator").gameObject.SetActive(false);

    //            }
    //        playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
    //        }

    //    }

    public override void OnJoinedRoom()
        {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoom_UI_Panel.name);

        // --- StartGameButton ---
        if (startGameButton != null)
            {
            startGameButton.SetActive(PhotonNetwork.LocalPlayer.IsMasterClient);
            }
        else
            {
            Debug.LogWarning("startGameButton が Inspector に設定されていません！");
            }

        // --- RoomInfoText ---
        if (roomInfoText != null)
            {
            roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                                "Players/Max.players:" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
            }
        else
            {
            Debug.LogWarning("roomInfoText が Inspector に設定されていません！");
            }

        // --- PlayerList 初期化 ---
        if (playerListGameObjects == null)
            {
            playerListGameObjects = new Dictionary<int, GameObject>();
            }

        // --- PlayerListPrefab / PlayerListContent チェック ---
        if (playerListPrefab == null || playerListContent == null)
            {
            Debug.LogError("playerListPrefab または playerListContent が Inspector に設定されていません！");
            return; // ここで処理を止める
            }

        // --- Player List 作成 ---
        foreach (Player player in PhotonNetwork.PlayerList)
            {
            GameObject playerListGameObject = Instantiate(playerListPrefab);
            playerListGameObject.transform.SetParent(playerListContent.transform, false);
            playerListGameObject.transform.localScale = Vector3.one;

            // --- PlayerNameText の取得 (Text / TMP_Text 両対応) ---
            var nameTextObj = playerListGameObject.transform.Find("PlayerNameText");
            if (nameTextObj != null)
                {
                var uiText = nameTextObj.GetComponent<Text>();
                var tmpText = nameTextObj.GetComponent<TMP_Text>();

                if (uiText != null)
                    {
                    uiText.text = player.NickName;
                    }
                else if (tmpText != null)
                    {
                    tmpText.text = player.NickName;
                    }
                else
                    {
                    Debug.LogWarning("PlayerNameText に Text または TMP_Text コンポーネントがありません！");
                    }
                }
            else
                {
                Debug.LogWarning("PlayerNameText がプレハブに見つかりません！");
                }

            // --- PlayerIndicator の処理 ---
            var indicator = playerListGameObject.transform.Find("PlayerIndicator");
            if (indicator != null)
                {
                indicator.gameObject.SetActive(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
                }

            // --- Dictionary 追加 ---
            if (!playerListGameObjects.ContainsKey(player.ActorNumber))
                {
                playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
                }
            }
        }


    public override void OnPlayerEnteredRoom(Player newPlayer)
        {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                            "Players/Max.players:" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = Instantiate(playerListPrefab);
        playerListGameObject.transform.SetParent(playerListContent.transform, false);
        playerListGameObject.transform.localScale = Vector3.one;

        // --- PlayerNameText の取得 (Text / TMP_Text 両対応) ---
        var nameTextObj = playerListGameObject.transform.Find("PlayerNameText");
        if (nameTextObj != null)
            {
            var uiText = nameTextObj.GetComponent<Text>();
            var tmpText = nameTextObj.GetComponent<TMP_Text>();

            if (uiText != null)
                {
                uiText.text = newPlayer.NickName;
                }
            else if (tmpText != null)
                {
                tmpText.text = newPlayer.NickName;
                }
            else
                {
                Debug.LogWarning("PlayerNameText に Text または TMP_Text コンポーネントがありません！");
                }
            }

        // --- PlayerIndicator ---
        var indicator = playerListGameObject.transform.Find("PlayerIndicator");
        if (indicator != null)
            {
            indicator.gameObject.SetActive(newPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
            }

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
        }

    public override void OnPlayerLeftRoom(Player otherPlayer)
        {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " + "Players/Max.players:" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
            startGameButton.SetActive(true);
            }
        }

    public override void OnLeftRoom()
        {
        ActivatePanel(GameOptions_UI_Panel.name);
        foreach (GameObject playerListGameObject in playerListGameObjects.Values)
            {
            Destroy(playerListGameObject);
            }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
        }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //    {
    //    ClearRoomListView();

    //    foreach (RoomInfo room in roomList)
    //        {
    //        Debug.Log(room.Name);
    //        if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
    //            {
    //            if (cachedRoomList.ContainsKey(room.Name))
    //                {
    //                cachedRoomList.Remove(room.Name);
    //                }
    //            }
    //        else
    //            {
    //            if (cachedRoomList.ContainsKey(room.Name))
    //                {
    //                cachedRoomList[room.Name] = room;
    //                }
    //            else
    //                {
    //                cachedRoomList.Add(room.Name, room);
    //                }
    //            }
    //        }

    //    foreach (RoomInfo room in cachedRoomList.Values)
    //        {
    //        GameObject roomListEntryGameObject = Instantiate(roomListEntryPrefab);
    //        roomListEntryGameObject.transform.SetParent(roomListParentGameObject.transform);
    //        roomListEntryGameObject.transform.localScale = Vector3.one;

    //        roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
    //        roomListEntryGameObject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
    //        roomListEntryGameObject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));

    //        roomListGameObjects.Add(room.Name, roomListEntryGameObject);

    //        }
    //    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        // 1. 既存の表示をクリア
        if (roomListGameObjects == null)
            roomListGameObjects = new Dictionary<string, GameObject>();

        foreach (var roomGO in roomListGameObjects.Values)
            {
            Destroy(roomGO);
            }
        roomListGameObjects.Clear();

        if (roomListParentGameObject == null)
            {
            Debug.LogError("roomListParentGameObject が設定されていません！");
            return;
            }

        // RoomListParent を必ず表示状態にする
        if (roomListParentGameObject != null && !roomListParentGameObject.activeSelf) { roomListParentGameObject.SetActive(true); }

        // 2. RoomList を反映
        foreach (RoomInfo room in roomList)
            {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
                continue;

            // Prefab を Content の子として生成
            GameObject roomGO = Instantiate(roomListEntryPrefab, roomListParentGameObject.transform, false);

            RectTransform rt = roomGO.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            // --- 部屋名の反映 (Text / TMP_Text 両対応) ---
            var nameObj = roomGO.transform.Find("RoomNameText");
            if (nameObj != null)
                {
                var uiText = nameObj.GetComponent<Text>();
                var tmpText = nameObj.GetComponent<TMP_Text>();

                if (uiText != null) uiText.text = room.Name;
                else if (tmpText != null) tmpText.text = room.Name;
                else Debug.LogWarning("RoomNameText に Text または TMP_Text がありません！");
                }
            else
                {
                Debug.LogWarning("RoomNameText がプレハブに見つかりません！");
                }

            // --- プレイヤー数の反映 (Text / TMP_Text 両対応) ---
            var playersObj = roomGO.transform.Find("RoomPlayersText");
            if (playersObj != null)
                {
                var uiText = playersObj.GetComponent<Text>();
                var tmpText = playersObj.GetComponent<TMP_Text>();

                string playersInfo = room.PlayerCount + " / " + room.MaxPlayers;

                if (uiText != null) uiText.text = playersInfo;
                else if (tmpText != null) tmpText.text = playersInfo;
                else Debug.LogWarning("RoomPlayersText に Text または TMP_Text がありません！");
                }

            // --- Join ボタン ---
            var joinButton = roomGO.transform.Find("JoinRoomButton")?.GetComponent<Button>();
            if (joinButton != null)
                {
                joinButton.onClick.AddListener(() =>
                {
                    if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
                    PhotonNetwork.JoinRoom(room.Name);
                });
                }

            // Dictionary に登録
            roomListGameObjects.Add(room.Name, roomGO);
            }
        }



    public override void OnLeftLobby()
        {
        ClearRoomListView();
        cachedRoomList.Clear();
        }

    public override void OnJoinRandomFailed(short returnCode, string message)
        {
        Debug.Log(message);

        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

    #endregion

    #region Private Methods
    void OnJoinRoomButtonClicked(string _roomName)
        {
        if (PhotonNetwork.InLobby)
            {
            PhotonNetwork.LeaveLobby();
            }
        PhotonNetwork.JoinRoom(_roomName);

        }
    void ClearRoomListView()
        {
        foreach (var roomListGameObject in roomListGameObjects.Values)
            {
            Destroy(roomListGameObject);
            }

        roomListGameObjects.Clear();
        }
    #endregion

    public void ShowCreateRoomPanelClick()
        {
        ActivatePanel(CreateRoom_UI_Panel.name);
        }


    #region  Public Methods
    public void ActivatePanel(string panelToBeActivated)
        {
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
        JoinRandomRoom_UI_Panel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Panel.name));
        }
    #endregion


    }
