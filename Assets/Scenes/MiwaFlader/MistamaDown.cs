using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DualShoukiGauge : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider shoukiGaugeA; // 瘴気ゲージA
    [SerializeField] private Slider shoukiGaugeB; // 瘴気ゲージB

    [Header("設定値")]
    [SerializeField] private float maxShouki = 5f; // ゲージ最大値
    private float shoukiA; // Aの現在値
    private float shoukiB; // Bの現在値

    private float velA = 0f; // Aのスムーズ補間用
    private float velB = 0f; // Bのスムーズ補間用

    void Start()
    {
        // 初期化
        shoukiA = maxShouki / 2f;
        shoukiB = maxShouki / 2f;

        shoukiGaugeA.maxValue = maxShouki;
        shoukiGaugeB.maxValue = maxShouki;

        shoukiGaugeA.value = shoukiA;
        shoukiGaugeB.value = shoukiB;
    }

    void Update()
    {
        // スムーズに反映
        float currentA = Mathf.SmoothDamp(shoukiGaugeA.value, shoukiA, ref velA, 0.1f);
        float currentB = Mathf.SmoothDamp(shoukiGaugeB.value, shoukiB, ref velB, 0.1f);

        shoukiGaugeA.value = currentA;
        shoukiGaugeB.value = currentB;

        // 入力処理
        if (Input.GetKeyDown(KeyCode.Space)) // Aを増やす
        {
            AddToGaugeA(1f);
        }
        else if (Input.GetKeyDown(KeyCode.Z)) // Bを増やす
        {
            AddToGaugeB(1f);
        }
    }

    /// <summary>
    /// Aゲージに加算 → その分Bから減算
    /// </summary>
    void AddToGaugeA(float amount)
    {
        float canAdd = Mathf.Min(amount, maxShouki - shoukiA, shoukiB);
        shoukiA += canAdd;
        shoukiB -= canAdd;
    }

    /// <summary>
    /// Bゲージに加算 → その分Aから減算
    /// </summary>
    void AddToGaugeB(float amount)
    {
        float canAdd = Mathf.Min(amount, maxShouki - shoukiB, shoukiA);
        shoukiB += canAdd;
        shoukiA -= canAdd;
    }
}
