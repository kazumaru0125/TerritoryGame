using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon; 
using Photon.Realtime;
using TMPro;

public class LobbyController : MonoBehaviourPunCallbacks
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

    [Header("Team UI")]
    public GameObject Team_UI_Panel;
    public TMP_Text teamAListText;
    public TMP_Text teamBListText;
    //public Button resetButton; // Resetボタン

    private const int MaxPerTeam = 2;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;
    public GameObject StartGameButton;

    #region Unity Methods

    public GameObject SD_unitychan_humanoid; // モデルのGameObject
    private Animator animator;

    public TMP_Text roomHostText;
    public TMP_Text membersListText;
    public int maxPlayers = 4;         // ルーム最大人数

    private string[] dotsAnimArray = { "・", "・・", "・・・" };
    private int dotsAnimIndex = 0;
    private float dotsAnimInterval = 0.5f; // 点の更新間隔
    private float dotsAnimTimer = 0f;

    public GameObject emptyRoomImage;
    public TMP_Text emptyRoomText;


    private List<string> randomNames = new List<string> 
    {
        "Unityちゃん", "トライデント", "ぼんじり", "前田",
        "匿名希望", "名無しさん", "わしじゃよ"
        // ここに好きな名前を追加
    };

    public static string GetHostName()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.IsMasterClient) return p.NickName;
        }
        return "Unknown";
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Login_UI_Panel.SetActive(true);
        // GameOptions_UI_Panel.SetActive(false);
        ActivatePanel(Login_UI_Panel.name);

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        PhotonNetwork.AutomaticallySyncScene = true;

        if (SD_unitychan_humanoid != null)
        {
            SD_unitychan_humanoid.SetActive(true);
            animator = SD_unitychan_humanoid.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        connectionStatusText.text = "Connection status: " + PhotonNetwork.NetworkClientState;

        // アニメーションタイマー処理
        dotsAnimTimer += Time.deltaTime;
        if (dotsAnimTimer >= dotsAnimInterval)
        {
            dotsAnimIndex = (dotsAnimIndex + 1) % dotsAnimArray.Length;
            dotsAnimTimer = 0f;
            // アニメーション表示をアップデート
            if (InsideRoom_UI_Panel.activeSelf)
            {
                UpdateRoomInfoUI();
            }
        }
    }


    #endregion

    #region UI Callbacks
    public void OnLoginButtonClicked()
    {
        // 既存処理：Photon接続
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

        // Saluteアニメ開始
        if (animator != null)
        {
            animator.SetBool("is_saluteing", true);
            // コルーチン起動でアニメ終了まで待つ
            StartCoroutine(WaitAndHideModel());
            Debug.Log("is_saluteingをtrueにしました");
        }
        else
        {
            Debug.LogWarning("Animatorが取得できていません！");
        }
    }

    void UpdateStartGameButtonVisibility()
    {
        if (StartGameButton != null)
        {
            StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    private IEnumerator WaitAndHideModel()
    {
        // Saluteステートの長さ取得
        float saluteLength = 5.5f;
        if (animator != null)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == "Salute")
                {
                    saluteLength = clip.length;
                    break;
                }
            }
        }
        // アニメ再生分だけ待つ
        yield return new WaitForSeconds(saluteLength);

        // モデル非表示
        SD_unitychan_humanoid.SetActive(false);

        // UI次画面へ
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnRandomButtonClicked()
    {
        if (randomNames.Count == 0) return;

        int index = Random.Range(0, randomNames.Count);
        string selected = randomNames[index];

        if (playerNameInput != null)
        {
            playerNameInput.text = selected;
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

    public void OnTeamButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props["CanSelectTeam"] = true;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("CanSelectTeam"))
        {
            bool canSelect = (bool)propertiesThatChanged["CanSelectTeam"];
            if (canSelect)
            {
                ActivatePanel(Team_UI_Panel.name);
                UpdateTeamUI(); // 初期表示更新
            }
        }
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
        //ActivatePanel(GameOptions_UI_Panel.name);

    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

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
            // ① 親を指定せず生成
            GameObject playerListGameObject = Instantiate(playerListPrefab);

            // ② 生成後に親を設定（falseにするとローカル座標維持）
            playerListGameObject.transform.SetParent(playerListContent.transform, false);
            playerListGameObject.transform.localScale = Vector3.one;

            // --- PlayerNameText の設定 ---
            var nameTextObj = playerListGameObject.transform.Find("PlayerNameText");
            if (nameTextObj != null)
                {
                var uiText = nameTextObj.GetComponent<Text>();
                var tmpText = nameTextObj.GetComponent<TMP_Text>();

                if (uiText != null)
                    uiText.text = player.NickName;
                else if (tmpText != null)
                    tmpText.text = player.NickName;
                else
                    Debug.LogWarning("PlayerNameText に Text または TMP_Text コンポーネントがありません！");
                }
            else
                {
                Debug.LogWarning("PlayerNameText がプレハブに見つかりません！");
                }

            // --- PlayerIndicator の処理 ---
            var indicator = playerListGameObject.transform.Find("PlayerIndicator");
            if (indicator != null)
                indicator.gameObject.SetActive(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

            // --- Dictionary 追加 ---
            if (!playerListGameObjects.ContainsKey(player.ActorNumber))
                playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
            }

        UpdateRoomInfoUI();

        }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                            "Players/Max.players:" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        // GameObject playerListGameObject = Instantiate(playerListPrefab);
        GameObject playerListGameObject = Instantiate(playerListPrefab, playerListContent.transform, false);

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

        UpdateRoomInfoUI();
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

        UpdateRoomInfoUI();
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

        // 部屋が存在するかを判定
        bool hasRoom = false;
        foreach (RoomInfo room in roomList)
        {
            if (room.IsOpen && room.IsVisible && !room.RemovedFromList) hasRoom = true;
        }

        // 部屋がなければ画像とテキストを表示、あれば隠す
        emptyRoomImage.SetActive(!hasRoom);
        emptyRoomText.gameObject.SetActive(!hasRoom);
        if (!hasRoom)
        {
            emptyRoomText.text = "部屋がありません";
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

    void ShowTeamPanelAndSetStartButton()
    {
        Team_UI_Panel.SetActive(true);
        if (StartGameButton != null)
        {
            StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // チームパネルが表示中なら、再度ボタン表示を同期
        if (Team_UI_Panel.activeSelf && StartGameButton != null)
        {
            StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }


    #region  Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
        Team_UI_Panel.SetActive(panelToBeActivated.Equals(Team_UI_Panel.name));

        // チームパネル表示のときだけ、ホスト判定でボタン表示
        if (panelToBeActivated.Equals(Team_UI_Panel.name) && StartGameButton != null)
        {
            StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }
    }
    #endregion

    public void OnTeamAButtonClicked()
    {
        if (CountPlayersInTeam("A") >= MaxPerTeam)
        {
            Debug.Log("Team A is full!");
            return;
        }

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["Team"] = "A";
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void OnTeamBButtonClicked()
    {
        if (CountPlayersInTeam("B") >= MaxPerTeam)
        {
            Debug.Log("Team B is full!");
            return;
        }

        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["Team"] = "B";
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    private int CountPlayersInTeam(string team)
    {
        int count = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.ContainsKey("Team") && (string)p.CustomProperties["Team"] == team)
            {
                count++;
            }
        }
        return count;
    }

    void UpdateRoomInfoUI()
    {
        string hostName = GetHostName();
        roomHostText.text = $"RoomHost : <b>{hostName}</b>";

        List<string> memberNames = new List<string>();
        foreach (Player p in PhotonNetwork.PlayerList)
            memberNames.Add(p.NickName);

        string result = "";
        for (int i = 1; i < maxPlayers; i++)
        {
            int memberIndex = i;
            if (i < memberNames.Count)
            {
                result += $"Member {memberIndex} : {memberNames[i]}\n";
            }
            else
            {
                string dots = dotsAnimArray[dotsAnimIndex];
                result += $"Member {memberIndex} : 探しています{dots}\n";
            }
        }
        membersListText.text = result;
    }

    private void UpdateTeamUI()
    {
        List<string> teamA = new List<string>();
        List<string> teamB = new List<string>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.CustomProperties.ContainsKey("Team"))
            {
                string team = (string)p.CustomProperties["Team"];
                if (team == "A") teamA.Add(p.NickName);
                else if (team == "B") teamB.Add(p.NickName);
            }
        }

        teamAListText.text = "Team A:\n" + string.Join("\n", teamA);
        teamBListText.text = "Team B:\n" + string.Join("\n", teamB);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Team"))
        {
            UpdateTeamUI();
        }
    }


    public void OnBackFromGameOptions_UI_PanelClicked()
    {
        // パネル切り替え
        ActivatePanel(Login_UI_Panel.name);

        // Unityちゃんを表示
        if (SD_unitychan_humanoid != null)
        {
            SD_unitychan_humanoid.SetActive(true);
        }
    }
    // ルーム入室中はGameOptionsなど
    public void OnBackCreateRoom_UI_PanelClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }
}
