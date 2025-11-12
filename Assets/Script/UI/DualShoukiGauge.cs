using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class DualShoukiGauge : MonoBehaviourPunCallbacks, IPunObservable
    {
    [Header("UI Sliders")]
    [SerializeField] private Slider shoukiGaugeA;
    [SerializeField] private Slider shoukiGaugeB;

    [Header("設定値")]
    [SerializeField] private float maxShouki = 3f; // 3段階でMax
    private float shoukiA;
    private float shoukiB;

    private float velA = 0f;
    private float velB = 0f;

    // --- ロール制御を持つスクリプト参照 ---
    private TestPlayerRoll playerRoll;

    private void Start()
        {
        // PlayerRollスクリプトを探す（同じオブジェクトにある前提）
        playerRoll = GetComponent<TestPlayerRoll>();
        if (playerRoll == null)
            {
            Debug.LogWarning("TestPlayerRoll が見つかりません。");
            }

        shoukiGaugeA.maxValue = maxShouki;
        shoukiGaugeB.maxValue = maxShouki;

        // 初期値：ロールに応じて設定
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
            {
            ApplyRoleGauge(roleObj.ToString());
            }
        else
            {
            shoukiA = shoukiB = maxShouki / 2f;
            }

        shoukiGaugeA.value = shoukiA;
        shoukiGaugeB.value = shoukiB;
        }

    private void Update()
        {
        // スムーズ補間
        shoukiGaugeA.value = Mathf.SmoothDamp(shoukiGaugeA.value, shoukiA, ref velA, 0.1f);
        shoukiGaugeB.value = Mathf.SmoothDamp(shoukiGaugeB.value, shoukiB, ref velB, 0.1f);

        if (!photonView.IsMine) return;

        // --- キー操作 ---
        if (Input.GetKeyDown(KeyCode.X))
            {
            OniLoseHumanGain(maxShouki / 3f);
            }
        else if (Input.GetKeyDown(KeyCode.C))
            {
            HumanLoseOniGain(maxShouki / 3f);
            }

        // --- Role自動切り替え ---
        if (playerRoll != null)
            {
            if (playerRoll.CurrentRole == "Oni" && shoukiA <= 0f)
                {
                playerRoll.RequestRoleChange("Human");
                }
            else if (playerRoll.CurrentRole == "Human" && shoukiB >= maxShouki)
                {
                playerRoll.RequestRoleChange("Oni");
                }
            }
        }


    // -----------------------------
    // --- ゲージ操作 ----------
    // -----------------------------
    private void OniLoseHumanGain(float amount)
        {
        // Oniゲージ減少、Humanゲージ増加
        float canChange = Mathf.Min(amount, shoukiA, maxShouki - shoukiB);
        shoukiA -= canChange;
        shoukiB += canChange;
        }

    private void HumanLoseOniGain(float amount)
        {
        // Humanゲージ減少、Oniゲージ増加
        float canChange = Mathf.Min(amount, shoukiB, maxShouki - shoukiA);
        shoukiB -= canChange;
        shoukiA += canChange;
        }

    // -----------------------------
    // --- Photon同期 ----------
    // -----------------------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        if (stream.IsWriting)
            {
            stream.SendNext(shoukiA);
            stream.SendNext(shoukiB);
            }
        else
            {
            shoukiA = (float)stream.ReceiveNext();
            shoukiB = (float)stream.ReceiveNext();
            }
        }

    // -----------------------------
    // --- Role変更検知 ----------
    // -----------------------------
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey("Role"))
            {
            string newRole = changedProps["Role"].ToString();
            ApplyRoleGauge(newRole);
            }
        }

    // -----------------------------
    // --- Roleに応じた初期化 ----
    // -----------------------------
    private void ApplyRoleGauge(string role)
        {
        if (role == "Oni")
            {
            shoukiA = maxShouki;
            shoukiB = 0f;
            }
        else if (role == "Human")
            {
            shoukiA = 0f;
            shoukiB = maxShouki;
            }
        }

    //private void ApplyRoleGauge(string role)
    //    {
    //    // Oniの場合のみAを最大、Humanは両方0
    //    if (role == "Oni")
    //        {
    //        shoukiA = maxShouki;
    //        shoukiB = 0f;
    //        }
    //    else if (role == "Human")
    //        {
    //        shoukiA = 0f;
    //        shoukiB = 0f;
    //        }
    //    }

    }
