using UnityEngine;

public class TitleIdleEndRollController : MonoBehaviour
{
    public float idleTimeToShowEndRoll = 5f;
    public GameObject titleCanvasRoot;
    public GameObject endRollCanvasRoot;

    float lastInputTime;
    bool endRollVisible = false;

    void Start()
    {
        lastInputTime = Time.time;

        if (endRollCanvasRoot != null)
            endRollCanvasRoot.SetActive(false);
        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(true);
    }

    void Update()
    {
        bool hasInput = HasAnyInput();

        if (!endRollVisible)
        {
            // タイトル表示中：無操作タイマー
            if (hasInput)
                lastInputTime = Time.time;

            if (Time.time - lastInputTime >= idleTimeToShowEndRoll)
                ShowEndRoll();
        }
        else
        {
            // エンドロール表示中：何か入力があったらタイトルに戻る
            if (hasInput)
                HideEndRollAndBackToTitle();
        }
    }

    bool HasAnyInput()
    {
        // キーボード・マウス全般
        if (Input.anyKeyDown) return true;

        // スティック入力
        if (Input.GetAxisRaw("Horizontal") != 0) return true;
        if (Input.GetAxisRaw("Vertical") != 0) return true;

        // Xbox コントローラ A/B ボタン
        if (Input.GetKeyDown("joystick button 0")) return true; // A
        if (Input.GetKeyDown("joystick button 1")) return true; // B

        return false;
    }

    void ShowEndRoll()
    {
        endRollVisible = true;

        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(false);

        if (endRollCanvasRoot != null)
            endRollCanvasRoot.SetActive(true); // OnEnable で毎回初期化される
    }

    public void HideEndRollAndBackToTitle()
    {
        endRollVisible = false;
        lastInputTime = Time.time;

        if (endRollCanvasRoot != null)
            endRollCanvasRoot.SetActive(false);

        if (titleCanvasRoot != null)
            titleCanvasRoot.SetActive(true);
    }
}
