using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class DecreaseTMPNumber : MonoBehaviourPunCallbacks, IPunObservable
    {
    [SerializeField] private TMP_Text myTeamVitalityText;
    [SerializeField] private TMP_Text enemyTeamVitalityText;

    [SerializeField] private int changeValue = 1;
    [SerializeField] private int maxValue = 100;

    private int ATeamcurrentValue = 0;
    private int BTeamcurrentValue = 0;
    private bool isGameEnded = false;

    private string myTeam; // 自分のチーム ("A" or "B")

    void Start()
        {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
            myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            }
        else
            {
            myTeam = "A";
            }

        ATeamcurrentValue = 0;
        BTeamcurrentValue = 0;
        UpdateUI();
        }

    void Update()
        {
        if (!photonView.IsMine || isGameEnded) return;

        // 勝利判定
        if (ATeamcurrentValue >= maxValue)
            {
            photonView.RPC(nameof(OnTeamWin), RpcTarget.All, "A");
            isGameEnded = true;
            }
        else if (BTeamcurrentValue >= maxValue)
            {
            photonView.RPC(nameof(OnTeamWin), RpcTarget.All, "B");
            isGameEnded = true;
            }
        }

    void UpdateUI()
        {
        if (myTeam == "A")
            {
            myTeamVitalityText.text = ATeamcurrentValue + "%";
            enemyTeamVitalityText.text = BTeamcurrentValue + "%";
            }
        else
            {
            myTeamVitalityText.text = BTeamcurrentValue + "%";
            enemyTeamVitalityText.text = ATeamcurrentValue + "%";
            }
        }

    public void AddATeamVitality(int value)
        {
        if (!photonView.IsMine || isGameEnded) return;

        // ✅ 0未満・最大値超過を防止
        ATeamcurrentValue = Mathf.Clamp(ATeamcurrentValue + value, 0, maxValue);

        UpdateUI();
        }

    public void AddBTeamVitality(int value)
        {
        if (!photonView.IsMine || isGameEnded) return;

        // ✅ 0未満・最大値超過を防止
        BTeamcurrentValue = Mathf.Clamp(BTeamcurrentValue + value, 0, maxValue);

        UpdateUI();
        }

    [PunRPC]
    void OnTeamWin(string team)
        {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log($"{team} Team WIN!");

        ChangeSceneManager.Instance.GoToTitleScene(2f);
        }

    private IEnumerator DisconnectAndGoToTitle(float delay)
        {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Disconnect();
        }

    public override void OnDisconnected(DisconnectCause cause)
        {
        Debug.Log("Photon disconnected: " + cause);
        SceneManager.LoadScene("TitleScene");
        }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            {
            stream.SendNext(ATeamcurrentValue);
            stream.SendNext(BTeamcurrentValue);
            }
        else
            {
            ATeamcurrentValue = (int)stream.ReceiveNext();
            BTeamcurrentValue = (int)stream.ReceiveNext();
            UpdateUI();
            }
        }
    }
