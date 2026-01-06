using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class WaveManager : MonoBehaviourPun
    {
    [SerializeField] private float time = 60f;

    [SerializeField] private CountdownUI countdownUI;
    [SerializeField] private float waveTime = 60f;

    private float stime;
    private bool waveStarted = false;


    public TMP_Text Count;
    public TMP_Text Wave;

    [SerializeField] private OfudaCount ofudaCount; // Inspectorでセット

    private bool wave1Ended = false;
    public int currentWave;


    private void Start()
        {
        //// Wave開始時に全員Humanに固定
        //if (PhotonNetwork.IsMasterClient)
        //    {
        //    foreach (Player p in PhotonNetwork.PlayerList)
        //        {
        //        Hashtable props = new Hashtable();
        //        props["Role"] = "Human";
        //        p.SetCustomProperties(props);
        //        }
        //    Debug.Log("全プレイヤーをHumanに設定");
        //    }

        //Wave.text = "お札を集めろ！";
        //currentWave = 1;

        if (PhotonNetwork.IsMasterClient)
            {
            foreach (Player p in PhotonNetwork.PlayerList)
                {
                Hashtable props = new Hashtable();
                props["Role"] = "Human";
                p.SetCustomProperties(props);
                }
            }

        Wave.text = "";
        Count.text = "";

        // カウントダウン終了通知を登録
        countdownUI.OnCountdownFinished += StartWave1;

        // カウントダウン開始
        countdownUI.Play();

        }


    private void StartWave1()
        {
        if (PhotonNetwork.IsMasterClient)
            {
            photonView.RPC(nameof(RPC_StartWave1), RpcTarget.All);
            }
        }

    [PunRPC]
    private void RPC_StartWave1()
        {
        waveStarted = true;
        time = waveTime;
        currentWave = 1;

        Wave.text = "お札を集めろ！";
        }



    //private void Update()
    //    {
    //    if (time > 0)
    //        {
    //        time -= Time.deltaTime;

    //        if (time <= 0 && !wave1Ended)
    //            {
    //            wave1Ended = true;
    //            time = 0;

    //            //  Wave.text = "Wave2";
    //            Debug.Log("Wave1終了");
    //            currentWave = 2;
    //            if (PhotonNetwork.IsMasterClient)
    //                {
    //                HandleWaveEnd();
    //                }
    //            }
    //        }

    //    if (Count != null)
    //        {
    //        Count.text = Mathf.CeilToInt(time).ToString();
    //        }
    //    }

    private void Update()
        {
        if (!waveStarted) return;

        if (time > 0)
            {
            time -= Time.deltaTime;
            Count.text = Mathf.CeilToInt(time).ToString();

            if (time <= 0 && !wave1Ended)
                {
                wave1Ended = true;
                time = 0;
                currentWave = 2;

                if (PhotonNetwork.IsMasterClient)
                    HandleWaveEnd();
                }
            }
        }


    // =======================================
    // Wave1終了時にHuman/Oniを割り当てる処理
    // =======================================
    //private void HandleWaveEnd()
    //    {
    //    if (ofudaCount == null)
    //        {
    //        Debug.LogWarning("OfudaCountが設定されていません");
    //        return;
    //        }

    //    int a = ofudaCount.ATeamcuOfuda;
    //    int b = ofudaCount.BTeamcuOfuda;

    //    string humanTeam = (a > b) ? "A" : (b > a) ? "B" : null;
    //    string oniTeam = (a > b) ? "B" : (b > a) ? "A" : null;

    //    if (humanTeam != null && oniTeam != null)
    //        {
    //        // 👇 ここが追加部分
    //        if (PhotonNetwork.IsMasterClient)
    //            {
    //            PhotonView pv = FindObjectOfType<TestPlayerRoll>().photonView;
    //            pv.RPC("RPC_Wave1RoleAssign", RpcTarget.All, humanTeam);

    //            }

    //        if (a > b)
    //            Debug.Log("Aチーム勝利！ → Wave1 Human=A / Oni=B");
    //        else if (b > a)
    //            Debug.Log("Bチーム勝利！ → Wave1 Human=B / Oni=A");
    //        }
    //    else
    //        {
    //        Debug.Log("引き分け → 役割変更なし");
    //        }
    //    }


    private void HandleWaveEnd()
        {
        if (ofudaCount == null)
            {
            Debug.LogWarning("OfudaCountが設定されていません");
            return;
            }

        int a = ofudaCount.ATeamcuOfuda;
        int b = ofudaCount.BTeamcuOfuda;

        string humanTeam;
        string oniTeam;

        if (a > b)
            {
            humanTeam = "A";
            oniTeam = "B";
            Debug.Log("Aチーム勝利！ → Human=A / Oni=B");
            }
        else if (b > a)
            {
            humanTeam = "B";
            oniTeam = "A";
            Debug.Log("Bチーム勝利！ → Human=B / Oni=A");
            }
        else
            {
            // 👇 引き分け時はランダム
            bool aIsHuman = Random.value < 0.5f;
            humanTeam = aIsHuman ? "A" : "B";
            oniTeam = aIsHuman ? "B" : "A";

            Debug.Log($"引き分け → ランダム決定！ Human={humanTeam} / Oni={oniTeam}");
            }

        // MasterClientのみが役割決定を通知
        if (PhotonNetwork.IsMasterClient)
            {
            PhotonView pv = FindObjectOfType<TestPlayerRoll>().photonView;
            pv.RPC("RPC_Wave1RoleAssign", RpcTarget.All, humanTeam);
            }
        }

    }
