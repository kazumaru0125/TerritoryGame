using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // TextMeshProを使うときに必要

public class DecreaseTMPNumber : MonoBehaviour
{
    // 数字を表示するTextMeshProの参照
    [SerializeField] private TMP_Text Enemyvitality;

    // 減らす量（例：1ずつ減らす）
    [SerializeField] private int decreaseValue = 1;

    void Update()
    {
        // Spaceキーが押されたら処理
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 現在のテキストを数値に変換
            if (int.TryParse(Enemyvitality.text, out int number))
            {
                // 数値を減らす（0未満にならないよう制限）
                number = Mathf.Max(0, number - decreaseValue);

                // テキストに反映
                Enemyvitality.text = number.ToString();
            
            }
            else
            {
                Debug.LogWarning("TextMeshProに数字が入っていません！");
            }
        }
    }
}