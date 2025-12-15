using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    public string nextScene = "LobbyScene";
    public AudioFader audioFader;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKeyDown("joystick button 0") ||
            Input.GetKeyDown("joystick button 1"))
        {
            // BGMフェードアウト後にシーン遷移
            audioFader.FadeOut(() => SceneManager.LoadScene(nextScene));
           
        }
    }
}
