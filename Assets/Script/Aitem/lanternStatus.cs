using UnityEngine;
using Photon.Pun;

public class lanternStatus : MonoBehaviour, IPunObservable
    {
    public int Maxvitality = 20;
    public int RecoveryVitality = 1;

    public GameObject Aura;

    // 現在のランタン残量（プレイヤーが吸うたびに減る）
    public int CurrentVitality = 20;

    // 同期したい状態
    public bool isActive = true;

    void Start()
        {
        CurrentVitality = Maxvitality;
        UpdateVisible();
        Aura.SetActive(true);
        }

    // ★ ここが重要：プレイヤーに引かれる用の関数
    public void AddVitality(int value)
        {
        CurrentVitality += value;

        // 上限チェック
        if (CurrentVitality > Maxvitality)
            CurrentVitality = Maxvitality;

        if (CurrentVitality < 0)
            CurrentVitality = 0;

        // 0になったらランタンを非表示にするなど
        if (CurrentVitality <= 0)
           // isActive = false;
           Aura.SetActive(false);

        UpdateVisible();
        }

    // ランタンの表示状態
    private void UpdateVisible()
        {
        gameObject.SetActive(isActive);
        }

    // Photon同期
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
            }
        }

    // プレイヤーが1回で吸える回復量
    public int GetRecoveryVitality()
        {
        return RecoveryVitality;
        }
    }
