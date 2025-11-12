using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TestPlayerRoll : MonoBehaviourPunCallbacks
    {
    [Header("モデル参照")]
    [SerializeField] private GameObject humanModel;
    [SerializeField] private GameObject oniModel;

    [Header("UI設定")]
    [SerializeField] private Vector3 uiOffset = new Vector3(0, 2.0f, 0);

    public string CurrentTeam { get; private set; } // "A" or "B"
    public string CurrentRole { get; private set; } // "Human" or "Oni"

    private TextMeshPro teamText;
    private Transform uiTransform;

    private void Start()
        {
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
            ToggleRoleForTeam(CurrentTeam);
            }

        // UIをカメラの方向に
        if (uiTransform != null && Camera.main != null)
            {
            uiTransform.position = transform.position + uiOffset;
            Vector3 direction = uiTransform.position - Camera.main.transform.position;
            if (direction.sqrMagnitude > 0.001f)
                uiTransform.rotation = Quaternion.LookRotation(direction);
            }
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

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
        if (target == photonView.Owner)
            {
            if (changedProps.ContainsKey("Team")) UpdateTeam();
            if (changedProps.ContainsKey("Role")) UpdateRole();
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

    private void UpdateRole()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            {
            CurrentRole = (string)role;
            // 子モデルに直接通知して切り替え
            UpdateModelByRole();
            UpdateTeamUI();
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


    }
