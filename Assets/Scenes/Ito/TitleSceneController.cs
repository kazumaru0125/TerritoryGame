using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    // 遷移先のシーン名
    public string nextScene = "LobbyScene";

    void Update()
    {
        // スペースキーでシーン遷移
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextScene);
        }

        // 画面クリックでシーン遷移
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(nextScene);
        }

    }
}
