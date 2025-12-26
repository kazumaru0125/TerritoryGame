using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
    public string nextScene = "LobbyScene";
    public AudioFader audioFader;
    public TitleIdleMovieController idleMovieController; // 追加

    void Update()
    {
        // ムービー再生中はシーン遷移させない
        if (idleMovieController != null && idleMovieController.IsMoviePlaying)
            return;

        // ★ さっきのフレームでムービースキップに使われた入力は無視
        if (idleMovieController != null && idleMovieController.ConsumedInputThisFrame)
            return;

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown("joystick button 0") ||
            Input.GetKeyDown("joystick button 1"))
        {
            audioFader.FadeOut(() => SceneManager.LoadScene(nextScene));
        }
    }

}
