using UnityEngine;

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

    void Start()
        {
        player = FindObjectOfType<TestPlayerRoll>();

        if (player == null)
            {
            Debug.LogError("TestPlayerRoll が見つかりません");
            return;
            }
        }

    void Update()
        {
        UpdateUI();
        }

    private void UpdateUI()
        {
        // Humanの残り
        int humanRemaining = player.GetRemainingLifeForTeam(player.CurrentTeam);

        // Oniチームの残り
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


    private bool IsUIActive(GameObject[] uiList)
{
    if (uiList == null) return false;

    foreach (var ui in uiList)
    {
        if (ui != null && ui.activeSelf)
            return true; // どれか1つでも表示中なら true
    }
    return false;
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
