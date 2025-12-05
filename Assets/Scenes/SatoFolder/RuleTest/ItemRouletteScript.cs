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

    //  他のスクリプトから参照できる変数
    public static Texture2D decidedItem;
    public static int decidedItemNumber = -1; // アイテム番号（-1は未決定の意味）

    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Return) && !isSpinning)
            {
            StartCoroutine(SpinRoulette());
            }
        }

    IEnumerator SpinRoulette()
        {
        if (itemTextures == null || itemTextures.Length == 0)
            {
            Debug.LogWarning("アイテム画像が設定されていません。");
            yield break;
            }

        isSpinning = true;
        float timer = 0f;

        while (timer < spinDuration)
            {
            int randomIndex = Random.Range(0, itemTextures.Length);
            rouletteImage.texture = itemTextures[randomIndex];
            yield return new WaitForSeconds(switchInterval);
            timer += switchInterval;
            }

        // 最終結果
        int finalIndex = Random.Range(0, itemTextures.Length);
        decidedItem = itemTextures[finalIndex];
        decidedItemNumber = finalIndex; 
        rouletteImage.texture = decidedItem;

        Debug.Log($"決定アイテム番号: {finalIndex}, 名前: {decidedItem.name}");

        isSpinning = false;
        }
    }
