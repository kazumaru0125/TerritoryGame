using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun; // PUN2必須

public class DecreaseTMPNumber : MonoBehaviourPunCallbacks, IPunObservable
    {
    [SerializeField] private TMP_Text ATeamVitality;
    [SerializeField] private TMP_Text BTeamVitality;

    [SerializeField] private int changeValue = 1;
    [SerializeField] private int maxValue = 100;

    private int ATeamcurrentValue = 0;
    private int BTeamcurrentValue = 0;

    void Start()
        {
        ATeamcurrentValue = 0;
        BTeamcurrentValue = 0;
        UpdateUI();
        }

    void Update()
        {
        if (!photonView.IsMine) return; // 自分のオブジェクトでないなら入力は無視

        if (Input.GetKeyDown(KeyCode.Q))
            {
            ATeamcurrentValue = Mathf.Max(0, ATeamcurrentValue - changeValue);
            UpdateUI();
            }

        if (Input.GetKeyDown(KeyCode.E))
            {
            BTeamcurrentValue = Mathf.Max(0, BTeamcurrentValue - changeValue);
            UpdateUI();
            }

        if (Input.GetKeyDown(KeyCode.W))
            {
            ATeamcurrentValue = Mathf.Min(maxValue, ATeamcurrentValue + changeValue);
            UpdateUI();
            }

        if (Input.GetKeyDown(KeyCode.R))
            {
            BTeamcurrentValue = Mathf.Min(maxValue, BTeamcurrentValue + changeValue);
            UpdateUI();
            }
        }

    void UpdateUI()
        {
        ATeamVitality.text = ATeamcurrentValue.ToString();
        BTeamVitality.text = BTeamcurrentValue.ToString();
        }

    // --- PUNの同期処理 ---
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting) // 自分の値を送信
            {
            stream.SendNext(ATeamcurrentValue);
            stream.SendNext(BTeamcurrentValue);
            }
        else // 受信
            {
            ATeamcurrentValue = (int)stream.ReceiveNext();
            BTeamcurrentValue = (int)stream.ReceiveNext();
            UpdateUI();
            }
        }
    }
