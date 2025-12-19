using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemRouletteScript : MonoBehaviour
    {
    [Header("ルーレットに使うPNG画像（Texture2D形式）")]
    public Texture2D[] itemTextures; // 画像をInspectorで登録

    [Header("ルーレットの表示先UI (RawImage)")]
    public RawImage rouletteImage;

    [Header("ルーレット設定")]
    public float spinDuration = 2f;
    public float switchInterval = 0.02f;

    private bool isSpinning = false;

    public static Texture2D decidedItem;
    public static int decidedItemNumber = -1;

    private int lastItem = -1;      // ←直前のアイテムを記録

    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Return) && !isSpinning)
            {
            StartCoroutine(SpinRoulette());
            }
        if (decidedItemNumber == -1)
            {
            // から状態
            rouletteImage.texture = null;
            // ※必要なら透明にする
            // rouletteImage.color = new Color(1,1,1,0);
            }

        }

    IEnumerator SpinRoulette()
        {
        if (itemTextures == null || itemTextures.Length == 0)
            {
            Debug.LogWarning("アイテム画像が設定されていません。");
            yield break;
            }

        // ★ランダムシードを変えて偏りを減らす
        Random.InitState(System.DateTime.Now.Millisecond + Random.Range(0, 999999));

        isSpinning = true;
        float timer = 0f;

        while (timer < spinDuration)
            {
            int randomIndex = Random.Range(0, itemTextures.Length);
            rouletteImage.texture = itemTextures[randomIndex];
            yield return new WaitForSeconds(switchInterval);
            timer += switchInterval;
            }

        // ★最終決定：直前と同じなら引き直す
        int finalIndex;
        do
            {
            finalIndex = Random.Range(0, itemTextures.Length);
            }
        while (finalIndex == lastItem);   // ←同じだったらやり直し

        lastItem = finalIndex; // 記録

        decidedItem = itemTextures[finalIndex];
        decidedItemNumber = finalIndex;
        rouletteImage.texture = decidedItem;

        Debug.Log($"決定アイテム番号: {finalIndex}, 名前: {decidedItem.name}");

        isSpinning = false;
        }

    public void StartRoulette()
        {
        if (!isSpinning)
            {
            StartCoroutine(SpinRoulette());
            }
        }
    }
