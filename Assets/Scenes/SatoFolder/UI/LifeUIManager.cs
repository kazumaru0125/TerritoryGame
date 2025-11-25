using UnityEngine;

public class LifeUIManager : MonoBehaviour
    {
    [Header("Human 用 UI (1,2,3)")]
    [SerializeField] private GameObject[] humanUI;

    [Header("Oni 用 UI (4,5,6)")]
    [SerializeField] private GameObject[] oniUI;

    private TestPlayerRoll player;

    void Start()
        {
        // 自分の TestPlayerRoll を取得（同じプレイヤーオブジェクトに付いている想定）
        player = FindObjectOfType<TestPlayerRoll>();

        if (player == null)
            {
            Debug.LogError("TestPlayerRoll がシーンに見つかりません");
            return;
            }

        UpdateUI();
        }

    void Update()
        {
        // ロール変わった瞬間に UI 更新したい場合は Update に入れても OK
        UpdateUI();
        }

    /// <summary>
    /// ロールに応じて UI を切り替える
    /// </summary>
    private void UpdateUI()
        {
        if (player.CurrentRole == "Human")
            {
            SetUI(humanUI, true);
            SetUI(oniUI, false);
            }
        else // Oni
            {
            SetUI(humanUI, false);
            SetUI(oniUI, true);
            }
        }

    /// <summary>
    /// 配列内の UI をまとめて ON/OFF
    /// </summary>
    private void SetUI(GameObject[] uiList, bool active)
        {
        if (uiList == null) return;

        foreach (var ui in uiList)
            {
            if (ui != null)
                ui.SetActive(active);
            }
        }
    }
