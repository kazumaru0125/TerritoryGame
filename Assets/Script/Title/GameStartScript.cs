using UnityEngine;
using UnityEngine.SceneManagement; // ← 必須

public class GameStartScript : MonoBehaviour
    {
    // ボタンにアタッチして呼び出す
    public void OnStartGameButtonClicked()
        {
        // LobbyScene に遷移
        SceneManager.LoadScene("LobbyScene");
        }
    }
