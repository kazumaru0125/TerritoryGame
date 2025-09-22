using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon; // for Hashtable
using TMPro;

public class WaveManager : MonoBehaviour
    {
    [SerializeField] private float time = 60f;

    public TMP_Text Count;
    public TMP_Text Wave;

    [SerializeField] private OfudaCount ofudaCount; // Inspectorでセット

    private bool wave1Ended = false; // Wave1終了フラグ

    private void Start()
        {
        // Wave開始時に全員 Human にする
        foreach (Player p in PhotonNetwork.PlayerList)
            {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props["Role"] = "Human";
            p.SetCustomProperties(props);
            }
        }

    void Update()
        {
        if (time > 0)
            {
            time -= Time.deltaTime;
            if (time <= 0 && !wave1Ended)
                {
                time = 0;
                Wave.text = "Wave2";
                wave1Ended = true;

                Debug.Log("Wave1終了");

                // 勝敗判定
                if (ofudaCount != null)
                    {
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
                        }

                    if (a > b)
                        Debug.Log("Aチーム勝利！ → A:Human / B:Oni");
                    else if (b > a)
                        Debug.Log("Bチーム勝利！ → B:Human / A:Oni");
                    else
                        Debug.Log("引き分け！");
                    }
                }
            else if (!wave1Ended)
                {
                Wave.text = "Wave1";
                }
            }

        if (Count != null)
            {
            Count.text = Mathf.CeilToInt(time).ToString();
            }
        }
    }
