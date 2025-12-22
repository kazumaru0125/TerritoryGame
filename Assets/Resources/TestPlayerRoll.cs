using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections;


public class TestPlayerRoll : MonoBehaviourPunCallbacks
    {
    [Header("モデル参照")]
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject oniModel;

    [SerializeField] private PlayerRespawnScript Prespawn;
    [SerializeField] private OniPlayerRespawn Orespawn;

    [Header("UI設定")]
    [SerializeField] private Vector3 uiOffset = new Vector3(0, 2.0f, 0);

    public string CurrentTeam { get; private set; } // "A" or "B"
    public string CurrentRole { get; private set; } // "Human" or "Oni"

    private TextMeshPro teamText;
    private Transform uiTransform;



    private void Start()
        {
        if (PhotonNetwork.IsMasterClient)
            {
            PhotonNetwork.CurrentRoom.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable {
            { "TeamA_Damage", 0 },
            { "TeamB_Damage", 0 }
                }
            );
            }

        AssignTeamAndRoleIfEmpty();
        UpdateTeam();
        UpdateRole();
        CreateUI();
        }

    private void LateUpdate()
        {
        // Qキーでチーム全員のロール切り替え
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
            {
            if (PhotonNetwork.IsMasterClient)
                {
                // Master が全員にフェード付きロール切替を指示
                photonView.RPC(
                    "RPC_RoleSwapWithFade",
                    RpcTarget.All,
                    CurrentTeam   // 仮で自分のチームを渡す
                );
                }
            }

        // UIをカメラの方向に
        //if (uiTransform != null && Camera.main != null)
        //    {
        //    uiTransform.position = transform.position + uiOffset;
        //    Vector3 direction = uiTransform.position - Camera.main.transform.position;
        //    if (direction.sqrMagnitude > 0.001f)
        //        uiTransform.rotation = Quaternion.LookRotation(direction);
        //    }
        }

    // -----------------------------
    // --- Photon 同期系 ----------
    // -----------------------------
    private void AssignTeamAndRoleIfEmpty()
        {
        var playerList = PhotonNetwork.PlayerList;

        int countA = 0;
        int countB = 0;
        foreach (var p in playerList)
            {
            if (p.CustomProperties.TryGetValue("Team", out object t))
                {
                if ((string)t == "A") countA++;
                else if ((string)t == "B") countB++;
                }
            }

        if (!photonView.Owner.CustomProperties.ContainsKey("Team"))
            {
            string assignedTeam = (countA <= countB) ? "A" : "B";
            photonView.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Team", assignedTeam } });
            }

        //if (!photonView.Owner.CustomProperties.ContainsKey("Role"))
        //    {
        //    string assignedRole = (Random.value < 0.5f) ? "Human" : "Oni";
        //    photonView.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", assignedRole } });
        //    }

        if (!photonView.Owner.CustomProperties.ContainsKey("Role"))
            {
            string assignedRole = "Human"; // 常にHumanを初期設定
            photonView.Owner.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", assignedRole } });
            }

        }

    private void ToggleRoleForTeam(string team)
        {
        foreach (var player in PhotonNetwork.PlayerList)
            {
            if (player.CustomProperties.TryGetValue("Team", out object t) && (string)t == team)
                {
                string currentRole = player.CustomProperties.TryGetValue("Role", out object r) ? (string)r : "Human";
                string newRole = (currentRole == "Human") ? "Oni" : "Human";
                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Role", newRole } });
                }
            }
        }

    public override void OnPlayerPropertiesUpdate(
      Player target,
      ExitGames.Client.Photon.Hashtable changedProps)
        {
        // この TestPlayerRoll が表しているプレイヤーか？
        if (target != photonView.Owner) return;

        if (changedProps.ContainsKey("Team"))
            {
            UpdateTeam();
            }

        if (changedProps.ContainsKey("Role"))
            {
            UpdateRole(); // ← UIとモデルを必ず更新
            }
        }

    // -----------------------------
    // --- Team & Role反映 --------
    // -----------------------------
    private void UpdateTeam()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Team", out object team))
            {
            CurrentTeam = (string)team;
            ApplyTeamVisual();
            UpdateTeamUI();
            }
        }

    //private void UpdateRole()
    //    {
    //    //if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
    //    //    {
    //    //    CurrentRole = (string)role;
    //    //    // 子モデルに直接通知して切り替え
    //    //    UpdateModelByRole();
    //    //    UpdateTeamUI();
    //    //    }

    //    }

    private void UpdateRole()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            {
            CurrentRole = (string)role;

            UpdateModelByRole();
            UpdateTeamUI();

         if (photonView.IsMine)
                {
                if (CurrentRole == "Human")
                    {
                    if (Prespawn != null)
                        Prespawn.RespawnAtRandomSpawnArea();
                    }
                else if (CurrentRole == "Oni")
                    {
                    if (Orespawn != null)
                        Orespawn.RespawnAtRandomSpawnArea();
                    }
                }
            }
        }


    // -----------------------------
    // --- モデル切替 -------------
    // -----------------------------
    private void UpdateModelByRole()
        {
        if (humanModel == null || oniModel == null)
            {
            Debug.LogWarning("HumanModel または OniModel が設定されていません");
            return;
            }

        bool isHuman = CurrentRole == "Human";
        humanModel.SetActive(isHuman);
        oniModel.SetActive(!isHuman);

        // Animatorがあればリセット
        GameObject activeObj = isHuman ? humanModel : oniModel;
        Animator anim = activeObj.GetComponent<Animator>();
        if (anim != null)
            {
            anim.Rebind();
            anim.Update(0);
            }
        }

    // -----------------------------
    // --- 表示/UI処理 ------------
    // -----------------------------
    private void CreateUI()
        {
        GameObject uiObj = new GameObject("TeamUI");
        uiObj.transform.SetParent(transform);
        uiTransform = uiObj.transform;
        uiTransform.localScale = Vector3.one * 0.1f;

        teamText = uiObj.AddComponent<TextMeshPro>();
        teamText.alignment = TextAlignmentOptions.Center;
        teamText.fontSize = 2.5f;
        teamText.enableAutoSizing = true;
        teamText.color = Color.white;
        UpdateTeamUI();
        }

    private void ApplyTeamVisual()
        {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            {
            renderer.material.color = (CurrentTeam == "A") ? Color.blue : Color.red;
            }
        }

    private void UpdateTeamUI()
        {
        if (teamText != null)
            {
            teamText.text = $"Team {CurrentTeam}\nRole {CurrentRole}";
            teamText.color = (CurrentTeam == "A") ? Color.blue : Color.red;
            }
        }


    // ==============================
    // DualShoukiGauge から呼ばれる
    // ==============================
    public void RequestRoleChange(string newRole)
        {
        if (CurrentRole == newRole) return;

        // Masterが管理している場合はRPCなどに変更してもOK
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", newRole } }
        );

        Debug.Log($"[TestPlayerRoll] Role changed to {newRole}");
        }



    [PunRPC]
    public void AddScoreRPC(string team, int value)
        {
        DecreaseTMPNumber manager = FindObjectOfType<DecreaseTMPNumber>();
        if (manager == null) return;

        if (team == "A")
            manager.AddATeamVitality(value);
        else
            manager.AddBTeamVitality(value);
        }

    [PunRPC]
    public void RequestDestroyRPC(int viewID)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
            PhotonNetwork.Destroy(pv.gameObject);
        }

    // ==============================
    // チームダメージの加算（Masterのみ）
    // ==============================
    [PunRPC]
    //public void AddTeamDamageRPC(string team)
    //    {
    //    if (!PhotonNetwork.IsMasterClient) return;

    //    // ルームプロパティ辞書
    //    var room = PhotonNetwork.CurrentRoom;
    //    if (room == null) return;

    //    // キー決定
    //    string key = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";

    //    // 現在値を取得
    //    int current = 0;
    //    if (room.CustomProperties.ContainsKey(key))
    //        current = (int)room.CustomProperties[key];

    //    current += 1;

    //    // 更新
    //    room.SetCustomProperties(
    //        new ExitGames.Client.Photon.Hashtable { { key, current } }
    //    );

    //    Debug.Log($"[Master] Team {team} Damage = {current}");

    //    // 3回以上でロール反転
    //    if (current >= 3)
    //        {
    //        // ① 今ダメージを食らったチームを反転
    //        ToggleRoleForTeam(team);

    //        // ② 相手チームも反転
    //        string otherTeam = (team == "A") ? "B" : "A";
    //        ToggleRoleForTeam(otherTeam);

    //        // リセット
    //        room.SetCustomProperties(
    //            new ExitGames.Client.Photon.Hashtable { { key, 0 } }
    //        );

    //        Debug.Log($"[Master] Team A and B Roles Swapped!");
    //        }
    //    }

    //  [PunRPC]
    //public void AddTeamDamageRPC(string team)
    //    {
    //    if (!PhotonNetwork.IsMasterClient) return;

    //    var room = PhotonNetwork.CurrentRoom;
    //    if (room == null) return;

    //    string key = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";

    //    int current = 0;
    //    if (room.CustomProperties.ContainsKey(key))
    //        current = (int)room.CustomProperties[key];

    //    current += 1;

    //    room.SetCustomProperties(
    //        new ExitGames.Client.Photon.Hashtable { { key, current } }
    //    );

    //    // ★ 残り死亡回数をログに表示（3回まで）
    //    int remaining = Mathf.Max(0, 3 - current);
    //    Debug.Log($"[Master] Team {team} Damage = {current} / 3  残り {remaining}回");

    //    // 3回以上でロール反転
    //    if (current >= 3)
    //        {
    //        Debug.Log($"[Master] Team {team} が3回死亡 → 全チームのロール反転開始！");

    //        ToggleRoleForTeam(team);

    //        string otherTeam = (team == "A") ? "B" : "A";
    //        ToggleRoleForTeam(otherTeam);

    //        // リセット
    //        room.SetCustomProperties(
    //            new ExitGames.Client.Photon.Hashtable { { key, 0 } }
    //        );

    //        Debug.Log($"[Master] Team A と Team B のロールを反転しました！");
    //        }
    //    }

    public void AddTeamDamageRPC(string team)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        var room = PhotonNetwork.CurrentRoom;
        if (room == null) return;

        // ダメージを受けたチームのキー
        string damagedKey = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";

        // ダメージを与えた側のキー
        string attackerKey = (team == "A") ? "TeamB_Damage" : "TeamA_Damage";

        // --- ① ダメージ受けた側を +1 ---
        int damagedValue = room.CustomProperties.ContainsKey(damagedKey)
            ? (int)room.CustomProperties[damagedKey] : 0;

        damagedValue += 1;


        // --- ② ダメージ与えた側を -1（0未満にはしない） ---
        int attackerValue = room.CustomProperties.ContainsKey(attackerKey)
            ? (int)room.CustomProperties[attackerKey] : 0;

        attackerValue = Mathf.Max(0, attackerValue - 1);


        // --- ③ ルームプロパティ更新 ---
        room.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable {
            { damagedKey, damagedValue },
            { attackerKey, attackerValue }
            }
        );

        Debug.Log($"[Master] {team} チームが被弾 → {damagedKey}={damagedValue}, 反対側 {attackerKey}={attackerValue}");

        //// --- ④ ロール反転判定 ---
        //if (damagedValue >= 3)
        //    {
        //    Debug.Log($"[Master] Team {team} が3回死亡 → 全チームのロール反転！");

        //    ToggleRoleForTeam(team);

        //    string otherTeam = (team == "A") ? "B" : "A";
        //    ToggleRoleForTeam(otherTeam);

        //    // リセット
        //    room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { damagedKey, 0 } });

        //    Debug.Log("[Master] ロール反転完了！");
        //    }

        if (damagedValue >= 3)
            {
            Debug.Log($"[Master] Team {team} が3回死亡 → フェード付きロール反転");

            //photonView.RPC(
            //    "RPC_RoleSwapWithFade",
            //    RpcTarget.All,
            //    team
            //);

            if (PhotonNetwork.IsMasterClient)
                {
                // Master が全員にフェード付きロール切替を指示
                photonView.RPC(
                    "RPC_RoleSwapWithFade",
                    RpcTarget.All,
                    CurrentTeam   // 仮で自分のチームを渡す
                );
                }
            // ダメージリセット
            room.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { damagedKey, 0 } }
            );
            }

        }

    public int GetRemainingLifeForTeam(string team)
        {
        var room = PhotonNetwork.CurrentRoom;
        if (room == null) return 3;

        string key = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";

        if (!room.CustomProperties.ContainsKey(key)) return 3;

        int current = (int)room.CustomProperties[key];
        return Mathf.Max(0, 3 - current);
        }



    public int GetRemainingLife()
        {
        var room = PhotonNetwork.CurrentRoom;
        if (room == null) return 3;

        string key = (CurrentTeam == "A") ? "TeamA_Damage" : "TeamB_Damage";

        if (!room.CustomProperties.ContainsKey(key)) return 3;

        int current = (int)room.CustomProperties[key];
        return Mathf.Max(0, 3 - current);
        }


    [PunRPC]
    private void RPC_RoleSwapWithFade(string team)
        {
        StartCoroutine(RoleSwapFadeSequence(team));
        }

    private IEnumerator RoleSwapFadeSequence(string team)
        {
        // ① フェードイン（暗転）
        if (ChangeFade.Instance != null)
            {
            ChangeFade.Instance.FadeIn(0.5f);
            yield return new WaitForSeconds(0.5f);
            }

        // ② ロール反転（Masterのみ）
        if (PhotonNetwork.IsMasterClient)
            {
            ToggleRoleForTeam(team);

            string otherTeam = (team == "A") ? "B" : "A";
            ToggleRoleForTeam(otherTeam);
            }

        // ★ Photon の CustomProperties 同期待ち
        yield return new WaitForSeconds(0.1f);

        // ★ ここが方法①の核心
        // Role / Team を「必ず再取得してUI更新」
        UpdateTeam();
        UpdateRole();
        UpdateTeamUI();

        // ③ フェードアウト（明転）
        if (ChangeFade.Instance != null)
            {
            ChangeFade.Instance.FadeOut(0.5f);
            }
        }




    }
