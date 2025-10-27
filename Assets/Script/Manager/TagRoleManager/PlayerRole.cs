using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerRole : MonoBehaviourPunCallbacks
    {
    public string CurrentTeam { get; private set; } // "A" or "B"
    public string CurrentRole { get; private set; } // "Human" or "Oni"

    private TextMeshPro teamText;
    private Transform uiTransform;

    //private void Start()
    //    {
    //    UpdateTeam();
    //    UpdateRole();

    //    // --- UI生成 ---
    //    GameObject uiObj = new GameObject("TeamUI");
    //    uiObj.transform.SetParent(transform);
    //    uiTransform = uiObj.transform;
    //    uiTransform.localPosition = new Vector3(0, 2.0f, 0);
    //    uiTransform.localScale = Vector3.one * 0.1f;

    //    teamText = uiObj.AddComponent<TextMeshPro>();
    //    teamText.alignment = TextAlignmentOptions.Center;
    //    teamText.fontSize = 2.5f;
    //    teamText.enableAutoSizing = true;
    //    teamText.text = "";
    //    teamText.color = Color.white;

    //    UpdateTeamUI();
    //    }

    private void Start()
        {
        AssignTeamAndRoleIfEmpty();
        UpdateTeam();
        UpdateRole();

        // --- UI生成 ---
        GameObject uiObj = new GameObject("TeamUI");
        uiObj.transform.SetParent(transform);
        uiTransform = uiObj.transform;
        uiTransform.localPosition = new Vector3(0, 2.0f, 0);
        uiTransform.localScale = Vector3.one * 0.1f;

        teamText = uiObj.AddComponent<TextMeshPro>();
        teamText.alignment = TextAlignmentOptions.Center;
        teamText.fontSize = 2.5f;
        teamText.enableAutoSizing = true;
        teamText.text = "";
        teamText.color = Color.white;

        UpdateTeamUI();
        }

    // --- チームとロールの未設定時の振り分け ---
    private void AssignTeamAndRoleIfEmpty()
        {
        var playerList = PhotonNetwork.PlayerList;

        // 既にチーム設定されている人数をカウント
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

        // 自分がまだ未設定なら割り振り
        if (!photonView.Owner.CustomProperties.ContainsKey("Team"))
            {
            string assignedTeam = (countA < 2) ? "A" : "B";
            var props = new ExitGames.Client.Photon.Hashtable { { "Team", assignedTeam } };
            photonView.Owner.SetCustomProperties(props);
            }

        // ロールも未設定なら順番に割り振り（Human/Oni）
        if (!photonView.Owner.CustomProperties.ContainsKey("Role"))
            {
            string assignedRole = (Random.value < 0.5f) ? "Human" : "Oni";
            var props = new ExitGames.Client.Photon.Hashtable { { "Role", assignedRole } };
            photonView.Owner.SetCustomProperties(props);
            }
        }

    private void LateUpdate()
        {
        // ★ QキーでRoleを切り替え（自分のオブジェクトだけ）
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
            {
            ToggleRoleForTeam(CurrentTeam);
            }

        // UIを常にカメラの方に向ける
        if (uiTransform != null && Camera.main != null)
            {
            Vector3 direction = uiTransform.position - Camera.main.transform.position;
            if (direction.sqrMagnitude > 0.001f)
                uiTransform.rotation = Quaternion.LookRotation(direction);
            }
        }

    void OnCollisionEnter(Collision collision)
        {
        if (!photonView.IsMine) return; // 自分のオブジェクトだけが処理

        if (collision.gameObject.CompareTag("Player"))
            {
            PlayerRole otherRole = collision.gameObject.GetComponent<PlayerRole>();
            if (otherRole == null) return;

            // チーム単位でロール切り替え
            ToggleRoleForTeam(CurrentTeam);
            }
        }

    // ✅ チーム全員のロールを切り替えるメソッド
    private void ToggleRoleForTeam(string team)
        {
        foreach (var player in PhotonNetwork.PlayerList)
            {
            if (player.CustomProperties.TryGetValue("Team", out object t) && (string)t == team)
                {
                string currentRole = player.CustomProperties.TryGetValue("Role", out object r) ? (string)r : "Human";
                string newRole = (currentRole == "Human") ? "Oni" : "Human";

                var props = new ExitGames.Client.Photon.Hashtable
                {
                    { "Role", newRole }
                };
                player.SetCustomProperties(props);
                }
            }
        }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
        if (target == photonView.Owner)
            {
            if (changedProps.ContainsKey("Team"))
                UpdateTeam();
            if (changedProps.ContainsKey("Role"))
                UpdateRole();
            }
        }

    private void UpdateTeam()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Team", out object team))
            {
            CurrentTeam = (string)team;
            ApplyTeamVisual();
            UpdateTeamUI();

            Debug.Log($"{photonView.Owner.NickName} のチームは {CurrentTeam} になりました");
            }
        }

    private void UpdateRole()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            {
            CurrentRole = (string)role;
            UpdateTeamUI();

            Debug.Log($"{photonView.Owner.NickName} の役割は {CurrentRole} になりました");
            }
        }

    private void ApplyTeamVisual()
        {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            {
            if (CurrentTeam == "A")
                renderer.material.color = Color.blue;
            else if (CurrentTeam == "B")
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.gray;
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
    }