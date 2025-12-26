using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ResultSceneController : MonoBehaviourPunCallbacks
    {
    private bool isProcessing = false;

    void Update()
        {
        if (isProcessing) return;

        bool space = Input.GetKeyDown(KeyCode.Space);
        bool padA = Input.GetKeyDown(KeyCode.JoystickButton0);
        bool padB = Input.GetKeyDown(KeyCode.JoystickButton1);

        if (space || padA || padB)
            {
            isProcessing = true;

            if (PhotonNetwork.InRoom)
                {
                PhotonNetwork.LeaveRoom();
                }
            else
                {
                SceneManager.LoadScene("TitleScene");
                }
            }
        }

    // ★ Room を出終わった瞬間に呼ばれる
    public override void OnLeftRoom()
        {
        // ここで Title へ戻す（←これなら GameServer → Master 完了後）
        SceneManager.LoadScene("TitleScene");
        }
    }
