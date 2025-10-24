using UnityEngine;

public class GetAitem : MonoBehaviour
    {
    [Header("プレイヤー本体（自分自身でもOK）")]
    public GameObject player;

    [Header("生成用プレハブ")]
    public GameObject bombPrefab;      // 爆弾
    public GameObject electricFloorPrefab; // 電撃床

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
        // Spaceキーで効果発動
        if (Input.GetKeyDown(KeyCode.X))
            {
            int num = ItemRouletteScript.decidedItemNumber;

            if (num == -1)
                {
                Debug.Log("まだアイテムが決まっていません。");
                return;
                }

            Debug.Log($"取得アイテム番号: {num}");
            ActivateEffect(num);
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
                SpawnElectricFloor();
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
            Instantiate(bombPrefab, player.transform.position, Quaternion.identity);
            Debug.Log("💣 爆弾を生成しました！");
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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // 一旦リセット
            rb.AddForce(Vector3.up * (jumpPower * 2f), ForceMode.VelocityChange);
            Debug.Log("🦘 ハイジャンプ発動！");
            }
        }

    void SpawnElectricFloor()
        {
        if (electricFloorPrefab != null)
            {
            Vector3 spawnPos = player.transform.position + Vector3.down * 0.5f;
            Instantiate(electricFloorPrefab, spawnPos, Quaternion.identity);
            Debug.Log("⚡ 電撃床を生成しました！");
            }
        else
            {
            Debug.LogWarning("電撃床プレハブが設定されていません！");
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
