using UnityEngine;
using Photon.Pun;
using TMPro;

public class lanternStatus : MonoBehaviour, IPunObservable
    {
    [Header("Vitality Settings")]
    public int Maxvitality = 20;
    public int RecoveryVitality = 1;
    public int CurrentVitality = 20;

    [Header("Visual Effects")]
    public GameObject Aura;
    private bool auraActive = true;   // ← AURAの同期用

    [Header("State")]
    public bool isActive = true;

    [Header("UI")]
    [SerializeField] private TMP_Text vitalityText;

    void Start()
        {
        CurrentVitality = Maxvitality;

        // Aura の初期状態
        auraActive = true;
        Aura.SetActive(true);

        UpdateVisible();
        UpdateUI();
        }

    // プレイヤーが Vitality を消費させる用途
    public void AddVitality(int value)
        {
        CurrentVitality += value;

        if (CurrentVitality > Maxvitality)
            CurrentVitality = Maxvitality;

        if (CurrentVitality < 0)
            CurrentVitality = 0;

        // ----- AURA の ON/OFF 状態更新 -----
        if (CurrentVitality <= 0)
            {
            Aura.SetActive(false);
            auraActive = false;
            }
        else
            {
            Aura.SetActive(true);
            auraActive = true;
            }
        //-----------------------------------

        UpdateVisible();
        UpdateUI();
        }

    private void UpdateVisible()
        {
        gameObject.SetActive(isActive);
        }

    private void UpdateUI()
        {
        if (vitalityText != null)
            {
            vitalityText.text = $"{CurrentVitality}";
            }
        }

    // ------------- Photon同期処理 ----------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            {
            // 自分 → 他プレイヤーへ送信
            stream.SendNext(CurrentVitality);
            stream.SendNext(isActive);
            stream.SendNext(auraActive); // Aura状態を送る
            }
        else
            {
            // 他プレイヤー → 自分が受信
            CurrentVitality = (int)stream.ReceiveNext();
            isActive = (bool)stream.ReceiveNext();
            auraActive = (bool)stream.ReceiveNext();

            UpdateVisible();
            UpdateUI();

            // 受信した Aura 状態を反映
            Aura.SetActive(auraActive);
            }
        }
    // ---------------------------------------------

    public int GetRecoveryVitality()
        {
        return RecoveryVitality;
        }

    [PunRPC]
    public void RpcConsumeVitality(int amount)
        {
        if (!PhotonNetwork.IsMasterClient) return;

        AddVitality(-amount);
        }
    }
