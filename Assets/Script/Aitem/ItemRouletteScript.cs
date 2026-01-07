using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemRouletteScript : MonoBehaviour
    {
    [Header("ルーレットに使うPNG画像（Texture2D形式）")]
    public Texture2D[] itemTextures;

    [Header("ルーレットの表示先UI (RawImage)")]
    public RawImage rouletteImage;

    [Header("ルーレット設定（※演出は使わないが残しておく）")]
    public float spinDuration = 2f;
    public float switchInterval = 0.02f;

    private bool isSpinning = false;

    public static Texture2D decidedItem;
    public static int decidedItemNumber = -1;

    private int lastItem = -1;

    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Return) && !isSpinning)
            {
            DecideItemInstant();
            }

        if (decidedItemNumber == -1)
            {
            rouletteImage.texture = null;
            }
        }

    void DecideItemInstant()
        {
        if (itemTextures == null || itemTextures.Length == 0)
            {
            Debug.LogWarning("アイテム画像が設定されていません。");
            return;
            }

        // 偏り軽減
        Random.InitState(System.DateTime.Now.Millisecond + Random.Range(0, 999999));

        int finalIndex;

        // 直前と同じなら引き直し
        do
            {
            finalIndex = Random.Range(0, itemTextures.Length);
            }
        while (finalIndex == lastItem);

        lastItem = finalIndex;

        decidedItem = itemTextures[finalIndex];
        decidedItemNumber = finalIndex;

        rouletteImage.texture = decidedItem;

        Debug.Log($"決定アイテム番号: {finalIndex}, 名前: {decidedItem.name}");
        }

    // ボタン用(外部から呼ぶ)
    public void StartRoulette()
        {
        DecideItemInstant();
        }
    }
