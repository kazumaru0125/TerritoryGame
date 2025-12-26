using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour
{
    void Update()
    {
        // キーボード Space
        bool space = Input.GetKeyDown(KeyCode.Space);

        // Xbox コントローラー A/B
        bool padA = Input.GetKeyDown(KeyCode.JoystickButton0); 
        bool padB = Input.GetKeyDown(KeyCode.JoystickButton1); 

        if (space || padA || padB)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
