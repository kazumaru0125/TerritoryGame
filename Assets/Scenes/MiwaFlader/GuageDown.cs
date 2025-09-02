using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageGauge : MonoBehaviour
{
    // ゲージの Image（Fill を使う）
    [SerializeField] private Image gaugeImage;

    // 最大値と現在値
    [SerializeField] private float maxValue = 100f;
    private float currentValue;

    // 1回で減らす量
    [SerializeField] private float decreaseAmount = 10f;

    void Start()
    {
        // 最初は満タン
        currentValue = maxValue;
        UpdateGauge();
    }

    void Update()
    {
        // Spaceキーが押されたら減らす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Decrease(decreaseAmount);
        }
    }

    // 値を減らす処理
    public void Decrease(float amount)
    {
        currentValue -= amount;
        if (currentValue < 0) currentValue = 0; // 0以下にはしない
        UpdateGauge();
    }

    // ゲージの見た目を更新
    private void UpdateGauge()
    {
        gaugeImage.fillAmount = currentValue / maxValue;
    }
}

