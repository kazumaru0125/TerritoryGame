using UnityEngine;
using Photon.Pun;

public class GetAitem : MonoBehaviourPun
    {
    [Header("プレイヤー本体（自分自身でもOK）")]
    public GameObject player;

    [Header("生成用プレハブ（Resources 直下に入れておく）")]
    public GameObject bombPrefab;           // 爆弾
    public GameObject BearTrapPrefab;  // 電撃床

    [Header("プレイヤーのパラメータ")]
    public float stamina = 100f;
    public float moveSpeed = 5f;
    public float jumpPower = 7f;

    private Rigidbody rb;

    void Start()
        {
        if (player == null) player = gameObject;
        rb = player.GetComponent<Rigidbody>();
        }

    void Update()
        {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.X))
            {
            int num = ItemRouletteScript.decidedItemNumber;

            if (num == -1)
                {
                Debug.Log("まだアイテムが決まっていません。");
                return;
                }

            Debug.Log($"取得アイテム番号: {num}");

            // RPC ではなくローカルで直接呼び出す
            ActivateEffect(num);
            }

        if (Input.GetKeyDown(KeyCode.C))
            {
            int num = ItemRouletteScript.decidedItemNumber;

            if (num == -1)
                {
                Debug.Log("まだアイテムが決まっていません。");
                return;
                }

            Debug.Log($"取得アイテム番号: {num}");

            // RPC ではなくローカルで直接呼び出す
            ActivateEffect(num);

            num = -2;
            }
        }

    void ActivateEffect(int itemNumber)
        {
        switch (itemNumber)
            {
            case 0:
                SpawnBomb();
                break;

            case 1:
                HighJump();
                break;

            case 2:
                SpawnBearTrap();
                break;

            case 3:
                IncreaseStamina();
                break;

            case 4:
                IncreaseSpeed();
                break;

            default:
                Debug.Log("未定義の効果です。");
                break;
            }
        }

    void SpawnBomb()
        {
        if (bombPrefab != null)
            {
            if (photonView.IsMine)
                {
                // 自分のクライアントだけで生成
                PhotonNetwork.Instantiate(bombPrefab.name, player.transform.position, Quaternion.identity);
                Debug.Log("💣 爆弾を生成しました！（自分のみ）");
                }
            }
        else
            {
            Debug.LogWarning("爆弾プレハブが設定されていません！");
            }
        }


    void HighJump()
        {
        if (rb != null)
            {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * (jumpPower * 2f), ForceMode.VelocityChange);
            Debug.Log(" ハイジャンプ発動！（ローカル）");
            }
        }

    void SpawnBearTrap()
        {
        if (BearTrapPrefab != null)
            {
            if (photonView.IsMine)
                {
                Vector3 spawnPos = player.transform.position + Vector3.down * 0.5f;
                PhotonNetwork.Instantiate(BearTrapPrefab.name, spawnPos, Quaternion.identity);
                Debug.Log("トラばさみを生成しました！（自分のみ）");
                }
            }
        else
            {
            Debug.LogWarning("トラばさみが設定されていません！");
            }
        }

    void IncreaseStamina()
        {
        stamina += 50f;
        Debug.Log($"💪 スタミナ上昇！ 現在のスタミナ: {stamina}");
        }

    void IncreaseSpeed()
        {
        moveSpeed += 2f;
        Debug.Log($"🏃 速度上昇！ 現在の速度: {moveSpeed}");
        }
    }
