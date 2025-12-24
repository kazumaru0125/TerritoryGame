using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class OfudaCount : MonoBehaviourPunCallbacks, IPunObservable
    {
    [SerializeField] private TMP_Text ATeamOfudaUI;
    [SerializeField] private TMP_Text BTeamOfudaUI;

    [SerializeField] private int maxValue = 5;

    public int ATeamcuOfuda = 0;
    public int BTeamcuOfuda = 0;

    private string myTeam = "A";

    void Start()
        {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
            {
            myTeam = PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString();
            }

        UpdateUI();
        }

    void UpdateUI()
        {
        // 👇 自分チームに応じて UI 入れ替えだけ行う
        if (myTeam == "A")
            {
            ATeamOfudaUI.text = $"×{ATeamcuOfuda}";
            BTeamOfudaUI.text = $"×{BTeamcuOfuda}";
            }
        else
            {
            ATeamOfudaUI.text = $"×{BTeamcuOfuda}";
            BTeamOfudaUI.text = $"×{ATeamcuOfuda}";
            }
        }

    // ====== ★ここは削除しないと言われたメソッド ======
    public void AddATeamVitality(int value)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        ATeamcuOfuda = Mathf.Min(maxValue, ATeamcuOfuda + value);
        UpdateUI();
        }

    public void AddBTeamVitality(int value)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        BTeamcuOfuda = Mathf.Min(maxValue, BTeamcuOfuda + value);
        UpdateUI();
        }
    // ====================================================

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            {
            stream.SendNext(ATeamcuOfuda);
            stream.SendNext(BTeamcuOfuda);
            }
        else
            {
            ATeamcuOfuda = (int)stream.ReceiveNext();
            BTeamcuOfuda = (int)stream.ReceiveNext();
            UpdateUI();
            }
        }
    }
