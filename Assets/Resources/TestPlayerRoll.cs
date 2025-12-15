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

    [Header("UI設定")]
    [SerializeField] private Vector3 uiOffset = new Vector3(0, 2.0f, 0);

    public string CurrentTeam { get; private set; }
    public string CurrentRole { get; private set; }

    private TextMeshPro teamText;
    private Transform uiTransform;

    private string pendingRole;

    // ==============================
    // Start
    // ==============================
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
#if UNITY_EDITOR
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
            {
            DebugToggleRoleForMyTeam();
            PlayDisappointmentAndChangeRole(
      (CurrentRole == "Human") ? "Oni" : "Human"
  );

            }
#endif

        // UIを常にカメラ方向へ
        if (uiTransform != null && Camera.main != null)
            {
            uiTransform.position = transform.position + uiOffset;
            Vector3 dir = uiTransform.position - Camera.main.transform.position;
            if (dir.sqrMagnitude > 0.001f)
                uiTransform.rotation = Quaternion.LookRotation(dir);
            }
        }


    // ==============================
    // デバッグ：自分のチーム全員Role切替
    // ==============================
    private void DebugToggleRoleForMyTeam()
        {
        if (!PhotonNetwork.IsMasterClient)
            {
            Debug.LogWarning("[Debug] Masterのみ実行可");
            return;
            }

        Debug.Log("[Debug] Qキー Fade付きRole切替");

        foreach (var p in PhotonNetwork.PlayerList)
            {
            if (!p.CustomProperties.TryGetValue("Team", out object t)) continue;
            if ((string)t != CurrentTeam) continue;

            string role = p.CustomProperties.TryGetValue("Role", out object r)
                ? (string)r : "Human";

            string newRole = (role == "Human") ? "Oni" : "Human";

            // ★ ここが超重要
            photonView.RPC(
                nameof(RequestRoleChangeRPC),
                p,
                newRole
            );
            }
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
    void RequestRoleChangeRPC(string newRole)
        {
        RequestRoleChange(newRole);
        }




    // ==============================
    // Team & Role 初期割当
    // ==============================
    private void AssignTeamAndRoleIfEmpty()
        {
        int countA = 0, countB = 0;

        foreach (var p in PhotonNetwork.PlayerList)
            {
            if (p.CustomProperties.TryGetValue("Team", out object t))
                {
                if ((string)t == "A") countA++;
                else if ((string)t == "B") countB++;
                }
            }

        if (!photonView.Owner.CustomProperties.ContainsKey("Team"))
            {
            string team = (countA <= countB) ? "A" : "B";
            photonView.Owner.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Team", team } }
            );
            }

        if (!photonView.Owner.CustomProperties.ContainsKey("Role"))
            {
            photonView.Owner.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Role", "Human" } }
            );
            }
        }





    // ==============================
    // Photon Callback
    // ==============================
    public override void OnPlayerPropertiesUpdate(
        Player target,
        ExitGames.Client.Photon.Hashtable changedProps)
        {
        if (target != photonView.Owner) return;

        if (changedProps.ContainsKey("Team")) UpdateTeam();
        if (changedProps.ContainsKey("Role")) UpdateRole();
        }

    // ==============================
    // Team / Role 表示反映
    // ==============================
    private void UpdateTeam()
        {
        if (!photonView.Owner.CustomProperties.TryGetValue("Team", out object team))
            return;

        CurrentTeam = (string)team;
        ApplyTeamVisual();
        UpdateTeamUI();
        }

    private void UpdateRole()
        {
        if (!photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            return;

        string newRole = (string)role;
        if (CurrentRole == newRole) return;

        CurrentRole = newRole;
        UpdateModelByRole();
        UpdateTeamUI();
        }

    // ==============================
    // 暗転 → Role変更 → 明転
    // ==============================
    public void ChangeRoleWithFade(string newRole)
        {
        if (!photonView.IsMine) return;
        if (CurrentRole == newRole) return;
        if (ChangeFade.Instance == null) return;

        ChangeFade.Instance.FadeIn(1.0f, () =>
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Role", newRole } }
            );
            RespawnByRole(newRole);
            ChangeFade.Instance.FadeOut(0.5f);
        });

        }

    //    public void PlayDisappointmentAndChangeRole(string newRole)
    //{
    //    if (!photonView.IsMine) return;
    //    if (CurrentRole == newRole) return;

    //    var disappointment = GetComponentInChildren<DisappointmentController>();
    //    if (disappointment == null)
    //    {
    //        Debug.LogError("DisappointmentController が見つかりません");
    //        return;
    //    }

    //    // 次のロールを一時保存（AnimationEvent から使う）
    //    pendingRole = newRole;

    //    // 落胆アニメ再生（全員同期）
    //    disappointment.Play();
    //}

    public void PlayDisappointmentAndChangeRole(string newRole)
        {
        if (!photonView.IsMine) return;
        if (CurrentRole == newRole) return;

        // ★ Oni のときは Disappointment を完全スキップ
        if (CurrentRole == "Oni")
            {
            Debug.Log("OniなのでDisappointmentなしでRole変更");
            ChangeRoleWithFade(newRole);
            return;
            }

        // ★ Human のときのみ探す
        var disappointment = humanModel.GetComponentInChildren<DisappointmentController>(true);
        if (disappointment == null)
            {
            Debug.LogError("Humanモデルに DisappointmentController が見つかりません");
            return;
            }

        // 次のロールを保持（AnimationEvent用）
        pendingRole = newRole;

        // 落胆アニメ（RPC同期）
        disappointment.Play();
        }





    // ==============================
    // モデル切替
    // ==============================
    private void UpdateModelByRole()
        {
        if (humanModel == null || oniModel == null) return;

        bool isHuman = CurrentRole == "Human";

        humanModel.SetActive(isHuman);
        oniModel.SetActive(!isHuman);

        GameObject active = isHuman ? humanModel : oniModel;
        Animator anim = active.GetComponent<Animator>();
        if (anim != null)
            {
            anim.Rebind();
            anim.Update(0);
            }
        }

    // ==============================
    // UI
    // ==============================
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

        UpdateTeamUI();
        }

    private void UpdateTeamUI()
        {
        if (teamText == null) return;

        teamText.text = $"Team {CurrentTeam}\nRole {CurrentRole}";
        teamText.color = (CurrentTeam == "A") ? Color.blue : Color.red;
        }

    private void ApplyTeamVisual()
        {
        var r = GetComponent<Renderer>();
        if (r != null)
            r.material.color = (CurrentTeam == "A") ? Color.blue : Color.red;
        }

    // ==============================
    // ダメージ管理（Master）
    // ==============================
    [PunRPC]
    public void AddTeamDamageRPC(string team)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        var room = PhotonNetwork.CurrentRoom;
        if (room == null) return;

        string damagedKey = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";
        string attackerKey = (team == "A") ? "TeamB_Damage" : "TeamA_Damage";

        int damaged = room.CustomProperties.ContainsKey(damagedKey)
            ? (int)room.CustomProperties[damagedKey] : 0;

        int attacker = room.CustomProperties.ContainsKey(attackerKey)
            ? (int)room.CustomProperties[attackerKey] : 0;

        damaged++;
        attacker = Mathf.Max(0, attacker - 1);

        room.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable {
                { damagedKey, damaged },
                { attackerKey, attacker }
            }
        );

        if (damaged >= 3)
            {
            ToggleRoleForTeam("A");
            ToggleRoleForTeam("B");

            room.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { damagedKey, 0 } }
            );
            }
        }

    // ==============================
    // 残りライフ取得（★保持）
    // ==============================
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

    // ==============================
    // チーム全体ロール反転
    // ==============================
    private void ToggleRoleForTeam(string team)
        {
        foreach (var p in PhotonNetwork.PlayerList)
            {
            if (!p.CustomProperties.TryGetValue("Team", out object t)) continue;
            if ((string)t != team) continue;

            string role = p.CustomProperties.TryGetValue("Role", out object r)
                ? (string)r : "Human";

            string newRole = (role == "Human") ? "Oni" : "Human";

            p.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Role", newRole } }
            );
            }
        }

    // ==============================
    // 外部公開API（★復活）
    // ==============================
    public void RequestRoleChange(string newRole)
        {
        if (CurrentRole == newRole) return;

        // ローカルプレイヤーのみ暗転演出
        if (photonView.IsMine)
            {
            ChangeRoleWithFade(newRole);
            }
        else
            {
            // 他人の表示は即反映（Photon同期待ち）
            UpdateRole();
            }

        Debug.Log($"[TestPlayerRoll] RequestRoleChange -> {newRole}");
        }

    //public void ExecuteFadeRoleChange(System.Action onComplete = null)
    //    {
    //    if (!photonView.IsMine) return;
    //    if (ChangeFade.Instance == null) return;

    //    ChangeFade.Instance.FadeIn(1.0f, () =>
    //    {
    //        PhotonNetwork.LocalPlayer.SetCustomProperties(
    //            new ExitGames.Client.Photon.Hashtable { { "Role", pendingRole } }
    //        );

    //        ChangeFade.Instance.FadeOut(0.5f, () =>
    //        {
    //            onComplete?.Invoke();
    //        });
    //    });
    //    }


    public void ExecuteFadeRoleChange(System.Action onComplete = null)
        {
        if (!photonView.IsMine) return;
        if (ChangeFade.Instance == null) return;

        ChangeFade.Instance.FadeIn(1.0f, () =>
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Role", pendingRole } }
            );

            // ★ ここでも Role別リスポーン
            RespawnByRole(pendingRole);

            ChangeFade.Instance.FadeOut(0.5f, () =>
            {
                onComplete?.Invoke();
            });
        });
        }



    public Animator GetCurrentAnimator()
        {
        GameObject active = (CurrentRole == "Human") ? humanModel : oniModel;
        if (active == null) return null;
        return active.GetComponent<Animator>();
        }

    // ==============================
    // Role別リスポーン（TestPlayerRoll完結）
    // ==============================
    //private void RespawnByRole(string role)
    //    {
    //    if (!photonView.IsMine) return;

    //    string tag = (role == "Oni") ? "OniSpawnArea" : "SpawnArea";

    //    GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(tag);
    //    if (spawnAreas.Length == 0)
    //        {
    //        Debug.LogWarning($"[{tag}] が見つかりません");
    //        return;
    //        }

    //    GameObject randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
    //    Vector3 pos = randomArea.transform.position + Vector3.up;

    //    // 自分は自分で動かす
    //    transform.position = pos;

    //    // 他人に同期
    //    photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, pos);

    //    Debug.Log($"[{role}] {tag} にリスポーン");
    //    }


    //private void RespawnByRole(string role)
    //    {
    //    if (!photonView.IsMine) return;

    //    string tag = (role == "Oni") ? "OniSpawnArea" : "SpawnArea";
    //    GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(tag);
    //    if (spawnAreas.Length == 0)
    //        {
    //        Debug.LogWarning($"[{tag}] が見つかりません");
    //        return;
    //        }

    //    GameObject area = spawnAreas[Random.Range(0, spawnAreas.Length)];

    //    // ★ Collider 必須
    //    Collider col = area.GetComponent<Collider>();
    //    if (col == null)
    //        {
    //        Debug.LogError($"[{area.name}] に Collider がありません");
    //        return;
    //        }

    //    // ===== 正確な「上面中央」 =====
    //    Vector3 spawnPos = col.bounds.center;
    //    spawnPos.y = col.bounds.max.y;

    //    // ===== CharacterController を考慮 =====
    //    CharacterController cc = GetComponent<CharacterController>();
    //    if (cc != null)
    //        {
    //        cc.enabled = false;
    //        spawnPos.y += cc.height * 0.5f + cc.skinWidth;
    //        }

    //    transform.position = spawnPos;

    //    if (cc != null)
    //        cc.enabled = true;

    //    // 他クライアント同期
    //    photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, spawnPos);

    //    Debug.Log($"[{role}] 正確に {area.name} の上へリスポーン");
    //    }


    private void RespawnByRole(string role)
        {
        if (!photonView.IsMine) return;

        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(
            role == "Oni" ? "OniSpawnArea" : "SpawnArea"
        );

        if (spawnAreas.Length == 0) return;

        GameObject area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            }

        Vector3 spawnPos = area.transform.position + Vector3.up;

        transform.position = spawnPos;

        photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, spawnPos);
        }


    private IEnumerator RespawnByRoleCoroutine(string role)
        {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            rb.isKinematic = true; // 移動中の物理干渉を防ぐ
            }

        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        yield return null; // ★ 1フレーム待つ

        string tag = (role == "Oni") ? "OniSpawnArea" : "SpawnArea";
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag(tag);
        if (spawnAreas.Length == 0) yield break;

        GameObject area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // ===== Collider の上面を取得 =====
        Collider col = area.GetComponent<Collider>();
        Vector3 spawnPos = (col != null) ? col.bounds.center : area.transform.position;
        if (col != null) spawnPos.y = col.bounds.max.y; // 上面

        // ===== CharacterController を考慮して高さ補正 =====
        if (cc != null)
            {
            spawnPos.y += cc.height * 0.5f + cc.skinWidth;
            }

        transform.position = spawnPos;

        // 他クライアント同期
        photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, spawnPos);

        if (cc != null) cc.enabled = true;
        if (rb != null) rb.isKinematic = false;
        if (rb != null) rb.WakeUp();

        Debug.Log($"[{role}] {area.name} に正確にリスポーン");
        }


    [PunRPC]
    private void RPC_SetRespawnPosition(Vector3 pos)
        {
        transform.position = pos;
        }






    }
