using UnityEngine;
using System.Collections;

public class LifeUIManager : MonoBehaviour
    {
    [Header("Human 用 UI (remaining=3)")]
    [SerializeField] private GameObject[] humanR3;

    [Header("Human 用 UI (remaining=2)")]
    [SerializeField] private GameObject[] humanR2;

    [Header("Human 用 UI (remaining=1)")]
    [SerializeField] private GameObject[] humanR1;

    [Header("Oni 用 UI (remaining=3)")]
    [SerializeField] private GameObject[] oniR3;

    [Header("Oni 用 UI (remaining=2)")]
    [SerializeField] private GameObject[] oniR2;

    [Header("Oni 用 UI (remaining=1)")]
    [SerializeField] private GameObject[] oniR1;

    private TestPlayerRoll player;

    private void Start()
        {
        // ★ プレイヤーが生成されるまで待つ
        StartCoroutine(WaitForPlayer());
        }

    private IEnumerator WaitForPlayer()
        {
        // 少し待ってから開始（Photon の生成待ち）
        yield return new WaitForSeconds(0.1f);

        while (player == null)
            {
            player = FindAnyObjectByType<TestPlayerRoll>();
            if (player != null) break;
            yield return null;
            }

        Debug.Log("LifeUIManager: Player を検出しました -> " + player.name);

        // 初回 UI 更新
        UpdateUI();
        }

    private void Update()
        {
        // プレイヤーが見つかるまでは何もしない
        if (player == null) return;

        UpdateUI();
        }

    private void UpdateUI()
        {
        if (player == null) return;

        // Human残り
        int humanRemaining = player.GetRemainingLifeForTeam(player.CurrentTeam);

        // Oni残り
        string oniTeam = (player.CurrentTeam == "A") ? "B" : "A";
        int oniRemaining = player.GetRemainingLifeForTeam(oniTeam);

        // 全部消す
        HideAllUI();

        if (player.CurrentRole == "Human")
            {
            if (humanRemaining == 3) ShowUI(humanR3);
            else if (humanRemaining == 2) ShowUI(humanR2);
            else if (humanRemaining == 1) ShowUI(humanR1);
            }
        else // Oni
            {
            if (oniRemaining == 3) ShowUI(oniR3);
            else if (oniRemaining == 2) ShowUI(oniR2);
            else if (oniRemaining == 1) ShowUI(oniR1);
            }
        }

    private void HideAllUI()
        {
        ShowUI(humanR3, false);
        ShowUI(humanR2, false);
        ShowUI(humanR1, false);

        ShowUI(oniR3, false);
        ShowUI(oniR2, false);
        ShowUI(oniR1, false);
        }

    private void ShowUI(GameObject[] list, bool active = true)
        {
        if (list == null) return;

        foreach (var obj in list)
            {
            if (obj != null)
                obj.SetActive(active);
            }
        }
    }
