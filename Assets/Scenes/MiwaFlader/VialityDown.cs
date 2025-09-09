using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshProを使うときに必要
using static UnityEngine.Rendering.DebugUI;

public class DecreaseTMPNumber : MonoBehaviour
{
    // 数字を表示するTextMeshProの参照
    [SerializeField] private TMP_Text Enemyvitality;

    // 増減する量（例：1ずつ増減する）
    [SerializeField] private int changeValue = 1;

    // 数値の上限（任意で設定可能）
    [SerializeField] private int maxValue = 100;

    void Update()
    {
        // Spaceキーが押されたら数値を減らす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (int.TryParse(Enemyvitality.text, out int number))
            {
                // 数値を減らす（0未満にならないよう制限）
                number = Mathf.Max(0, number - changeValue);
                Enemyvitality.text = number.ToString() ;

            }
            else
            {
                Debug.LogWarning("TextMeshProに数字が入っていません！");
            }
        }

        // Zキーが押されたら数値を増やす
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (int.TryParse(Enemyvitality.text, out int number))
            {
                // 数値を増やす（上限を超えないよう制限）
                number = Mathf.Min(maxValue, number + changeValue);
                Enemyvitality.text = number.ToString() ;
            }
            else
            {
                Debug.LogWarning("TextMeshProに数字が入っていません！");
            }
        }
    }
}
