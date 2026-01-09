using UnityEngine;
using Photon.Pun;

public class GoalScript : MonoBehaviour
    {
    DecreaseTMPNumber gauge;
    string touchingTeam = null;
    string myTeam;

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
        if (touchingTeam == null || gauge == null) return;

        // ゴールに触れたチームのゲージ値を取得
        int value = (touchingTeam == "A") ? gauge.ATeamcurrentValue : gauge.BTeamcurrentValue;

        // ゲージが100未満なら何もしない
        if (value < 100) return;

        // Xbox B ボタン押下
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
            Debug.Log("CLEAR! Team = " + touchingTeam);

            // 勝利 RPC を呼ぶのはマスタークライアントだけ
            if (PhotonNetwork.IsMasterClient)
                {
                gauge.photonView.RPC("OnTeamWin", RpcTarget.All, touchingTeam);
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
