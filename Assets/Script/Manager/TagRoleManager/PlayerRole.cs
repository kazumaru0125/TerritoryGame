using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerRole : MonoBehaviourPunCallbacks
    {
    public string CurrentRole { get; private set; }

    private TextMeshPro roleText;
    private Transform uiTransform;

    void Start()
        {
        UpdateRole();

        // --- ローカルUIを頭の上に生成 ---
        GameObject uiObj = new GameObject("RoleUI");
        uiObj.transform.SetParent(transform);
        uiTransform = uiObj.transform;
        uiTransform.localPosition = new Vector3(0, 2.2f, 0); // 頭上に表示

        roleText = uiObj.AddComponent<TextMeshPro>();
        roleText.alignment = TextAlignmentOptions.Center;
        roleText.fontSize = 2.5f;
        roleText.enableAutoSizing = true;
        roleText.text = "";
        roleText.color = Color.white;

        UpdateRoleUI();
        }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
        if (target == photonView.Owner && changedProps.ContainsKey("Role"))
            {
            UpdateRole();
            }
        }

    private void UpdateRole()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            {
            CurrentRole = (string)role;
            ApplyRoleVisual();
            UpdateRoleUI();

            Debug.Log($"{photonView.Owner.NickName} の役割は {CurrentRole} になりました");
            }
        }

    private void ApplyRoleVisual()
        {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            {
            if (CurrentRole == "Oni")
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.blue;
            }
        }

    private void UpdateRoleUI()
        {
        if (roleText != null)
            {
            roleText.text = CurrentRole;
            roleText.color = (CurrentRole == "Oni") ? Color.red : Color.blue;
            }
        }

    private void LateUpdate()
        {
        // カメラに向けてUIを常に正面にする
        if (uiTransform != null && Camera.main != null)
            {
            uiTransform.rotation = Quaternion.LookRotation(uiTransform.position - Camera.main.transform.position);
            }
        }

    private void OnCollisionEnter(Collision collision)
        {
        if (!photonView.IsMine) return;

        PlayerRole other = collision.gameObject.GetComponent<PlayerRole>();
        if (other == null) return;

        if (CurrentRole == "Oni" && other.CurrentRole == "Runner")
            {
            TagRoleManager roleManager = FindObjectOfType<TagRoleManager>();
            if (roleManager != null)
                {
                int oniId = photonView.Owner.ActorNumber;
                int runnerId = other.photonView.Owner.ActorNumber;

                roleManager.photonView.RPC("SwapRoles", RpcTarget.MasterClient, oniId, runnerId);

                Debug.Log($"RPC送信: Oni={oniId}, Runner={runnerId}");
                }
            else
                {
                Debug.LogError("TagRoleManager がシーンに見つかりません！");
                }
            }
        }
    }
