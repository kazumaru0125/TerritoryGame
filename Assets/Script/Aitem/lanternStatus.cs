using UnityEngine;
using Photon.Pun;
using TMPro;

public class lanternStatus : MonoBehaviour, IPunObservable
    {
    public int Maxvitality = 20;
    public int RecoveryVitality = 1;

    public GameObject Aura;

    public int CurrentVitality = 20;

    public bool isActive = true;

    // ← ここで TMP_Text を追加
    [SerializeField] private TMP_Text vitalityText;

    void Start()
        {
        CurrentVitality = Maxvitality;
        UpdateVisible();
        Aura.SetActive(true);
        UpdateUI();
        }

    // プレイヤーに引かれる用の関数
    public void AddVitality(int value)
        {
        CurrentVitality += value;

        if (CurrentVitality > Maxvitality)
            CurrentVitality = Maxvitality;

        if (CurrentVitality < 0)
            CurrentVitality = 0;

        if (CurrentVitality <= 0)
            Aura.SetActive(false);

        UpdateVisible();
        UpdateUI(); // UIも更新
        }

    private void UpdateVisible()
        {
        gameObject.SetActive(isActive);
        }

    private void UpdateUI()
        {
        if (vitalityText != null)
            {
            //vitalityText.text = $"{CurrentVitality}/{Maxvitality}";
            vitalityText.text = $"{CurrentVitality}";
            }
        }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            {
            stream.SendNext(CurrentVitality);
            stream.SendNext(isActive);
            }
        else
            {
            CurrentVitality = (int)stream.ReceiveNext();
            isActive = (bool)stream.ReceiveNext();
            UpdateVisible();
            UpdateUI(); // 受信したらUI更新
            }
        }

    public int GetRecoveryVitality()
        {
        return RecoveryVitality;
        }
    }
