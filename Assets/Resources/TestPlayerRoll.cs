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

   private ChangeFade change;

    [Header("ロール切替時のフェード時間")]
    [SerializeField] private float fadeOutTime = 1.0f;
    [SerializeField] private float fadeInTime = 1.0f;

    [Header("フェード後の処理待機時間")]
    [SerializeField] private float switchDelay = 0.3f;

    private ChangeFade fadeController;



    public string CurrentTeam { get; private set; }  // "A" or "B"
    public string CurrentRole { get; private set; }  // "Human" or "Oni"

    private TextMeshPro teamText;
    private Transform uiTransform;

    private void  Start()
        {
        //change = FindObjectOfType<ChangeFade>();

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
   
        }




    private IEnumerator InitFade()
        {
        yield return null;              // 1フレーム待つ
        yield return new WaitForEndOfFrame(); // もう 1フレーム待つ

        change = FindObjectOfType<ChangeFade>();
        }

    private void LateUpdate()
        {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
            {
            //ToggleRoleForTeam(CurrentTeam);
            StartCoroutine(SwapRolesWithEffects(CurrentTeam));
            }

        if (uiTransform != null && Camera.main != null)
            {
            uiTransform.position = transform.position + uiOffset;
            Vector3 direction = uiTransform.position - Camera.main.transform.position;
            if (direction.sqrMagnitude > 0.001f)
                uiTransform.rotation = Quaternion.LookRotation(direction);
            }
        }


    // -----------------------------
    // --- Photon 初期設定 ----------
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
            photonView.Owner.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Team", assignedTeam } }
            );
            }

        if (!photonView.Owner.CustomProperties.ContainsKey("Role"))
            {
            photonView.Owner.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { "Role", "Human" } }
            );
            }
        }


    // -----------------------------
    // --- RPC / Property Update ----
    // -----------------------------
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
    // --- Team 更新 ---------------
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


    // -----------------------------
    // --- Role 更新 (フェード付き) -
    // -----------------------------
    private void UpdateRole()
        {
        if (!photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            return;

        CurrentRole = (string)role;

        // フェード付きにする
        if (change != null)
            {
            change.FadeIn(0.3f, () =>
            {
                ApplyRoleChangeInternal();
                change.FadeOut(0.3f);
            });
            }
        else
            {
            ApplyRoleChangeInternal();
            }
        }

    // フェード中に呼ばれる "本来の処理"
    private void ApplyRoleChangeInternal()
        {
        UpdateModelByRole();
        UpdateTeamUI();
        }


    // -----------------------------
    // --- モデル切替 ---------------
    // -----------------------------
    private void UpdateModelByRole()
        {
        if (humanModel == null || oniModel == null) return;

        bool isHuman = CurrentRole == "Human";
        humanModel.SetActive(isHuman);
        oniModel.SetActive(!isHuman);

        Animator anim = (isHuman ? humanModel : oniModel).GetComponent<Animator>();
        if (anim != null)
            {
            anim.Rebind();
            anim.Update(0);
            }
        }


    // -----------------------------
    // --- UI処理 -------------------
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
        if (renderer)
            renderer.material.color = (CurrentTeam == "A") ? Color.blue : Color.red;
        }

    private void UpdateTeamUI()
        {
        if (teamText)
            {
            teamText.text = $"Team {CurrentTeam}\nRole {CurrentRole}";
            teamText.color = (CurrentTeam == "A") ? Color.blue : Color.red;
            }
        }


    // ==============================
    // DualShoukiGauge からの要求
    // ==============================
    public void RequestRoleChange(string newRole)
        {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable { { "Role", newRole } }
        );
        }


    // ==============================
    // Fade RPC （全員フェード用）
    // ==============================
    [PunRPC]
    public void FadeAllPlayersRPC()
        {
        if (change != null)
            {
            change.FadeIn(0.5f, () =>
            {
                change.FadeOut(0.5f);
            });
            }
        }


    // ==============================
    // チームダメージ → ロール反転処理
    // ==============================

    [PunRPC]
    public void AddTeamDamageRPC(string team)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        var room = PhotonNetwork.CurrentRoom;
        if (room == null) return;

        string damagedKey = (team == "A") ? "TeamA_Damage" : "TeamB_Damage";
        string attackerKey = (team == "A") ? "TeamB_Damage" : "TeamA_Damage";

        int damagedValue = room.CustomProperties.ContainsKey(damagedKey) ? (int)room.CustomProperties[damagedKey] : 0;
        int attackerValue = room.CustomProperties.ContainsKey(attackerKey) ? (int)room.CustomProperties[attackerKey] : 0;

        damagedValue += 1;
        attackerValue = Mathf.Max(0, attackerValue - 1);

        room.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable {
                { damagedKey, damagedValue },
                { attackerKey, attackerValue }
            }
        );

        //if (damagedValue >= 3)
        //    {
        //    PhotonView.Get(this).RPC(nameof(PlayDisappointmentAllRPC), RpcTarget.All);


        //    // 全員フェード
        //    photonView.RPC("FadeAllPlayersRPC", RpcTarget.All);

        //    // 暗転してからロール反転
        //    StartCoroutine(DelayedRoleSwap(2.5f, team));

        //    // リセット
        //    room.SetCustomProperties(
        //        new ExitGames.Client.Photon.Hashtable { { damagedKey, 0 } }
        //    );
        //    }

        if (damagedValue >= 3)
            {

            // ① 全員に失望アニメーション
            //PhotonView.Get(this).RPC(nameof(PlayDisappointmentAllRPC), RpcTarget.All);

            //// ② 1.5秒後に全員フェード＋ロール反転
            //StartCoroutine(PlayDisappointmentThenFadeAndSwap(team));

            StartCoroutine(SwapRolesWithEffects(team));


            // ③ リセット
            room.SetCustomProperties(
                new ExitGames.Client.Photon.Hashtable { { damagedKey, 0 } }
            );
            }



        }

    private IEnumerator PlayDisappointmentThenFadeAndSwap(string team)
        {
        // 1.5秒待機
        yield return new WaitForSeconds(1.5f);

        // ③ 全員フェード実行
        photonView.RPC("FadeAllPlayersRPC", RpcTarget.All);

        // Fade が 1.0秒相当ならその時間だけ待つ（あなたの FadeIn+Out は 0.5+0.5 = 1秒）
        yield return new WaitForSeconds(1.0f);

        // ④ ロール反転（AもBも反転）
        ToggleRoleForTeam(team);
        ToggleRoleForTeam(team == "A" ? "B" : "A");
        }


    private IEnumerator DelayedRoleSwap(float delay, string team)
        {
        yield return new WaitForSeconds(delay);

        ToggleRoleForTeam(team);
        ToggleRoleForTeam(team == "A" ? "B" : "A");
        }

    public int GetRemainingLifeForTeam(string team)
        {
        // 例: "A" → "A_Life"
        string key = team + "_Life";

        if (PhotonNetwork.CurrentRoom != null &&
            PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key))
            {
            return (int)PhotonNetwork.CurrentRoom.CustomProperties[key];
            }

        // 未設定の場合はデフォルト 3 とする
        return 3;
        }

    [PunRPC]
    public void PlayDisappointmentAllRPC()
        {
        DisappointmentController dis = GetComponentInChildren<DisappointmentController>();

        if (dis != null)
            {
            dis.photonView.RPC(nameof(DisappointmentController.PlayDisappointmentAnimation), RpcTarget.All);
            }
        }


    // ========================================
    //  完全統一：ガッカリ → 待機 → Fade → 反転 → FadeOut
    // ========================================
    private IEnumerator SwapRolesWithEffects(string teamToSwap)
        {
        string other = (teamToSwap == "A") ? "B" : "A";

        // ① 全員ガッカリ
        photonView.RPC(nameof(PlayDisappointmentAllRPC), RpcTarget.All);

        // ② 1.5秒演出待機
        yield return new WaitForSeconds(1.8f);

        // ③ Fade In
        if (change != null)
            yield return change.FadeIn(1.0f);

        // ④ ロール反転
        ToggleRoleForTeam(teamToSwap);
        ToggleRoleForTeam(other);

        // ⑤ 必要あれば位置も同期
        //photonView.RPC(nameof(TeleportByRoleRPC), RpcTarget.All);

        // ⑥ Fade Out
        if (change != null)
            yield return change.FadeOut(0.0f);
        }


    }
