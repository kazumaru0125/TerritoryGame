using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class OfudaCount : MonoBehaviourPunCallbacks, IPunObservable
    {
    [SerializeField] private TMP_Text ATeamOfudaUI;
    [SerializeField] private TMP_Text BTeamOfudaUI;

    [SerializeField] private int changeValue = 1;
    [SerializeField] private int maxValue = 5;

    public int ATeamcuOfuda = 0;
    public int BTeamcuOfuda = 0;

    void Start()
        {
        ATeamcuOfuda = 0;
        BTeamcuOfuda = 0;
        UpdateUI();
        }

    void Update()
        {
        if (!photonView.IsMine) return; // 自分のオブジェクトでないなら入力は無視

        //if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //    ATeamcurrentValue = Mathf.Max(0, ATeamcurrentValue - changeValue);
        //    UpdateUI();
        //    }

        //if (Input.GetKeyDown(KeyCode.E))
        //    {
        //    BTeamcurrentValue = Mathf.Max(0, BTeamcurrentValue - changeValue);
        //    UpdateUI();
        //    }

        //if (Input.GetKeyDown(KeyCode.W))
        //    {
        //    ATeamcurrentValue = Mathf.Min(maxValue, ATeamcurrentValue + changeValue);
        //    UpdateUI();
        //    }

        //if (Input.GetKeyDown(KeyCode.R))
        //    {
        //    BTeamcurrentValue = Mathf.Min(maxValue, BTeamcurrentValue + changeValue);
        //    UpdateUI();
        //    }
        }

    void UpdateUI()
        {
        ATeamOfudaUI.text = $"お札：{ATeamcuOfuda.ToString()}";
        BTeamOfudaUI.text = $"お札：{BTeamcuOfuda.ToString()}";
        }

    public void AddATeamVitality(int value)
        {
        if (!photonView.IsMine) return;

        ATeamcuOfuda = Mathf.Min(maxValue, ATeamcuOfuda + value);
        UpdateUI();
        }

    public void AddBTeamVitality(int value)
        {
        if (!photonView.IsMine) return;

        BTeamcuOfuda = Mathf.Min(maxValue, BTeamcuOfuda + value);
        UpdateUI();
        }


    // --- PUNの同期処理 ---
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting) // 自分の値を送信
            {
            stream.SendNext(ATeamcuOfuda);
            stream.SendNext(BTeamcuOfuda);
            }
        else // 受信
            {
            ATeamcuOfuda = (int)stream.ReceiveNext();
            BTeamcuOfuda = (int)stream.ReceiveNext();
            UpdateUI();
            }
        }
    }
