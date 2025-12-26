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
    public int currentWave;

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

        Wave.text = "お札を集めろ！";
        currentWave = 1;
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

              //  Wave.text = "Wave2";
                Debug.Log("Wave1終了");
                currentWave = 2;
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
            // 👇 ここが追加部分
            if (PhotonNetwork.IsMasterClient)
                {
                PhotonView pv = FindObjectOfType<TestPlayerRoll>().photonView;
                pv.RPC("RPC_Wave1RoleAssign", RpcTarget.All, humanTeam);

                }

            if (a > b)
                Debug.Log("Aチーム勝利！ → Wave1 Human=A / Oni=B");
            else if (b > a)
                Debug.Log("Bチーム勝利！ → Wave1 Human=B / Oni=A");
            }
        else
            {
            Debug.Log("引き分け → 役割変更なし");
            }
        }

    }
