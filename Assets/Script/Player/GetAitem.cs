using UnityEngine;
using Photon.Pun;

public class GetAitem : MonoBehaviourPun
    {
    public GameObject player;

    public GameObject bombPrefab;
    public GameObject BearTrapPrefab;

    private Rigidbody rb;
    private ItemRouletteScript roulette;

    void Start()
        {
        if (player == null) player = gameObject;
        rb = player.GetComponent<Rigidbody>();

        // ルーレットUIオブジェクト取得
        GameObject rouletteObj = GameObject.Find("ItemRoulette");

        if (rouletteObj != null)
            {
            roulette = rouletteObj.GetComponent<ItemRouletteScript>();
            Debug.Log("ルーレットUIを取得しました");
            }
        else
            {
            Debug.LogError("ItemRoulette がシーンに見つかりません！");
            }
        }

    void Update()
        {
        if (!photonView.IsMine) return;

        // ▼ B：アイテム取得 → UIルーレット開始
        if (Input.GetKeyDown("joystick button 1"))
            {
            if (roulette != null)
                {
                roulette.StartRoulette(); // UI アニメ開始！
                Debug.Log("🎰 ルーレット開始！");
                }
            }

        // ▼ RB：アイテム使用
        if (Input.GetKeyDown("joystick button 5"))
            {
            int num = ItemRouletteScript.decidedItemNumber;

            if (num == -1)
                {
                Debug.Log("❗アイテム未所持");
                return;
                }

            Debug.Log($"🔥 アイテム使用: {num}");
            ActivateEffect(num);

            // 使用後は空欄へ
            ItemRouletteScript.decidedItemNumber = -1;

            // UI更新したかったら：roulette.ClearIcon() など追加できる
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
            default:
                Debug.Log("未定義アイテム");
                break;
            }
        }

    void SpawnBomb()
        {
        PhotonNetwork.Instantiate(bombPrefab.name, player.transform.position, Quaternion.identity);
        Debug.Log("爆弾生成！");
        }

    void HighJump()
        {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * 20f, ForceMode.VelocityChange);
        Debug.Log("ハイジャンプ！");
        }

    void SpawnBearTrap()
        {
        Vector3 pos = player.transform.position + Vector3.down * 0.5f;
        PhotonNetwork.Instantiate(BearTrapPrefab.name, pos, Quaternion.identity);
        Debug.Log(" トラバサミ設置！");
        }
    }
