using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GoalScript : MonoBehaviour
    {
    public string winScene = "ResultWinScene";
    public string loseScene = "ResultLossScene";

    DecreaseTMPNumber gauge;
    string touchingTeam = null;

    string myTeam;   // ← 追加！

    void Start()
        {
        gauge = FindObjectOfType<DecreaseTMPNumber>();

        if (gauge == null)
            {
            Debug.LogError("ステージ上に DecreaseTMPNumber が見つかりません！");
            }

        // 自分のチーム取得
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
            myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log("My Team = " + myTeam);
            }
        }

    void Update()
        {
        if (touchingTeam == null) return;

        if (gauge == null) return;

        int value = (touchingTeam == "A")
            ? gauge.ATeamcurrentValue
            : gauge.BTeamcurrentValue;

        // 条件を満たしていなければ何もしない
        if (value < 2) return;

        // Xbox B ボタン
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
            Debug.Log("CLEAR! Team = " + touchingTeam);

            // 勝ち負け判定！
            if (touchingTeam == myTeam)
                {
                SceneManager.LoadScene(winScene);
                }
            else
                {
                SceneManager.LoadScene(loseScene);
                }
            }
        }

    private void OnTriggerEnter(Collider other)
        {
        if (!other.CompareTag("Player")) return;

        PhotonView view = other.GetComponent<PhotonView>();

        if (view != null && view.Owner.CustomProperties.ContainsKey("Team"))
            {
            touchingTeam = (string)view.Owner.CustomProperties["Team"];
            Debug.Log("Enter → Team : " + touchingTeam);
            }
        }

    private void OnTriggerExit(Collider other)
        {
        if (!other.CompareTag("Player")) return;

        touchingTeam = null;
        }
    }
