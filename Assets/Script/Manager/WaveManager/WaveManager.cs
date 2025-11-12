using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class WaveManager : MonoBehaviourPun
    {
    [SerializeField] private float time = 60f;

    public TMP_Text Count;
    public TMP_Text Wave;

    [SerializeField] private OfudaCount ofudaCount; // Inspectorでセット

    private bool wave1Ended = false;

    private void Start()
        {
        // Wave開始時に全員Humanに固定
        if (PhotonNetwork.IsMasterClient)
            {
            foreach (Player p in PhotonNetwork.PlayerList)
                {
                Hashtable props = new Hashtable();
                props["Role"] = "Human";
                p.SetCustomProperties(props);
                }
            Debug.Log("全プレイヤーをHumanに設定");
            }

        Wave.text = "Wave1";
        }

    private void Update()
        {
        if (time > 0)
            {
            time -= Time.deltaTime;

            if (time <= 0 && !wave1Ended)
                {
                wave1Ended = true;
                time = 0;

                Wave.text = "Wave2";
                Debug.Log("Wave1終了");

                if (PhotonNetwork.IsMasterClient)
                    {
                    HandleWaveEnd();
                    }
                }
            }

        if (Count != null)
            {
            Count.text = Mathf.CeilToInt(time).ToString();
            }
        }

    // =======================================
    // Wave1終了時にHuman/Oniを割り当てる処理
    // =======================================
    private void HandleWaveEnd()
        {
        if (ofudaCount == null)
            {
            Debug.LogWarning("OfudaCountが設定されていません");
            return;
            }

        int a = ofudaCount.ATeamcuOfuda;
        int b = ofudaCount.BTeamcuOfuda;

        string humanTeam = (a > b) ? "A" : (b > a) ? "B" : null;
        string oniTeam = (a > b) ? "B" : (b > a) ? "A" : null;

        if (humanTeam != null && oniTeam != null)
            {
            foreach (Player p in PhotonNetwork.PlayerList)
                {
                if (p.CustomProperties.TryGetValue("Team", out object team))
                    {
                    string t = (string)team;
                    Hashtable props = new Hashtable();
                    props["Role"] = (t == humanTeam) ? "Human" : "Oni";
                    p.SetCustomProperties(props);
                    }
                }

            if (a > b)
                Debug.Log("Aチーム勝利！ → A:Human / B:Oni");
            else if (b > a)
                Debug.Log("Bチーム勝利！ → B:Human / A:Oni");
            }
        else
            {
            Debug.Log("引き分け → 役割変更なし");
            }
        }
    }
