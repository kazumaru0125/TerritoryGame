using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemRouletteScript : MonoBehaviour
    {
    [Header("ルーレットに使うPNG画像（Texture2D形式）")]
    public Texture2D[] itemTextures; // PNG画像をInspectorに登録 (6枚)

    [Header("ルーレットの表示先UI (RawImage)")]
    public RawImage rouletteImage; // 現在のアイテムを表示するUI

    [Header("ルーレット設定")]
    public float spinDuration = 2f; // ルーレットが回る時間（2秒で止まる）
    public float switchInterval = 0.02f; // 切り替え間隔（速さ）

    private bool isSpinning = false;
    private Texture2D decidedItem; // 決定した画像

    void Start()
        {
        // 初期状態では非表示
        if (rouletteImage != null)
            {
            rouletteImage.texture = null;
            rouletteImage.color = new Color(1, 1, 1, 0);
            }
        }

    void Update()
        {
        // Enterキーでルーレット開始
        if (Input.GetKeyDown(KeyCode.Return) && !isSpinning)
            {
            StartCoroutine(SpinRoulette());
            }

        // Vキーで非表示に戻す
        if (Input.GetKeyDown(KeyCode.V))
            {
            HideRoulette();
            }
        }

    IEnumerator SpinRoulette()
        {
        if (itemTextures == null || itemTextures.Length == 0 || rouletteImage == null)
            {
            Debug.LogWarning("画像またはUIが設定されていません！");
            yield break;
            }

        isSpinning = true;
        float timer = 0f;

        // 表示を有効化
        rouletteImage.color = new Color(1, 1, 1, 1);

        // 🎡 高速で画像を切り替える（一定速度）
        while (timer < spinDuration)
            {
            int randomIndex = Random.Range(0, itemTextures.Length);
            rouletteImage.texture = itemTextures[randomIndex];
            yield return new WaitForSeconds(switchInterval);
            timer += switchInterval;
            }

        // 🎯 最終的にランダムなアイテムを選択
        int finalIndex = Random.Range(0, itemTextures.Length);
        decidedItem = itemTextures[finalIndex];
        rouletteImage.texture = decidedItem;

        Debug.Log($"決定アイテム: {decidedItem.name}");

        isSpinning = false;

        // 🎁 アイテムをプレイヤーに渡す
        GiveItemToPlayer(decidedItem);
        }

    void HideRoulette()
        {
        if (rouletteImage != null)
            {
            rouletteImage.texture = null;
            rouletteImage.color = new Color(1, 1, 1, 0);
            }
        Debug.Log("ルーレット非表示");
        }

    void GiveItemToPlayer(Texture2D item)
        {
        Debug.Log($"プレイヤーが {item.name} を取得！");
        // 例: GameManager.Instance.AddItemTexture(item);
        }
    }
