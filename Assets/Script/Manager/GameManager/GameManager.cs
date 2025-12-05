using UnityEngine;

public class GameManager : MonoBehaviour
    {
    [Header("終了確認パネル")]
    public GameObject quitConfirmPanel; // 確認パネル

    private bool isConfirmOpen = false;

    void Update()
        {
        // ESCキーが押されたら確認ダイアログ表示
        if (Input.GetKeyDown(KeyCode.Escape))
            {
            if (!isConfirmOpen)
                {
                OpenQuitConfirm();
                }
            else
                {
                CloseQuitConfirm();
                }
            }
        }

    // 終了確認パネルを開く
    public void OpenQuitConfirm()
        {
        quitConfirmPanel.SetActive(true);
        isConfirmOpen = true;
        }

    // 終了確認パネルを閉じる
    public void CloseQuitConfirm()
        {
        quitConfirmPanel.SetActive(false);
        isConfirmOpen = false;
        }

    // 「はい」ボタンに設定（2秒待って終了）
    public void QuitGame()
        {
        StartCoroutine(QuitAfterDelay());
        }

    private System.Collections.IEnumerator QuitAfterDelay()
        {
        yield return new WaitForSeconds(1f);
        Debug.Log("ゲーム終了");
        Application.Quit();
        }
    }
