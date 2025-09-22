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

    private void Start()
        {
        UpdateTeam();
        UpdateRole();

        // --- UIê∂ê¨ ---
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

    private void LateUpdate()
        {
        if (uiTransform != null && Camera.main != null)
            {
            Vector3 direction = uiTransform.position - Camera.main.transform.position;
            if (direction.sqrMagnitude > 0.001f)
                uiTransform.rotation = Quaternion.LookRotation(direction);
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

            Debug.Log($"{photonView.Owner.NickName} ÇÃÉ`Å[ÉÄÇÕ {CurrentTeam} Ç…Ç»ÇËÇ‹ÇµÇΩ");
            }
        }

    private void UpdateRole()
        {
        if (photonView.Owner.CustomProperties.TryGetValue("Role", out object role))
            {
            CurrentRole = (string)role;
            UpdateTeamUI();

            Debug.Log($"{photonView.Owner.NickName} ÇÃñäÑÇÕ {CurrentRole} Ç…Ç»ÇËÇ‹ÇµÇΩ");
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

