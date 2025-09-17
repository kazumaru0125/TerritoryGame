using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerRole : MonoBehaviourPunCallbacks
{
    public string CurrentTeam { get; private set; } // "A" or "B"

    private TextMeshPro teamText;
    private Transform uiTransform;

    private void Start()
        {
        UpdateTeam();

        // --- ローカルUIを頭の上に生成 ---
        GameObject uiObj = new GameObject("TeamUI");
        uiObj.transform.SetParent(transform);
        uiTransform = uiObj.transform;
        uiTransform.localPosition = new Vector3(0, 2.0f, 0);

        // スケール調整でUIサイズを小さく
        uiTransform.localScale = Vector3.one * 0.1f; // 0.5倍に縮小

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
            // カメラ方向に常に向ける
            Vector3 direction = uiTransform.position - Camera.main.transform.position;
            if (direction.sqrMagnitude > 0.001f) // ゼロ除算回避
                uiTransform.rotation = Quaternion.LookRotation(direction);
            }
        }


    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (target == photonView.Owner && changedProps.ContainsKey("Team"))
        {
            UpdateTeam();
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
            teamText.text = $"Team {CurrentTeam}";
            teamText.color = (CurrentTeam == "A") ? Color.blue : Color.red;
        }
    }

    //private void LateUpdate()
    //{
    //    if (uiTransform != null && Camera.main != null)
    //    {
    //        uiTransform.rotation = Quaternion.LookRotation(uiTransform.position - Camera.main.transform.position);
    //    }
    //}
}
