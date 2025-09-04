using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider;   // スタミナゲージのスライダー
    [SerializeField] private float maxStamina = 5f;  // 最大スタミナ
    [SerializeField] private float decreaseRate = 1f; // 1秒あたりの消費量
    [SerializeField] private float recoverRate = 1f;  // 1秒あたりの回復量
    [SerializeField] private Image fillImage;         // スライダーのFill部分のImage

    private float stamina;              // 実際のスタミナ値
    private float currentVelocity = 0f; // SmoothDamp用
    [SerializeField] private float smoothTime = 0.2f; // 滑らかに追従する時間

    void Start()
    {
        // 初期化
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        UpdateGaugeColor();
    }

    void Update()
    {
        // スペースキーを押し続けている間、スタミナを消費
        if (Input.GetKey(KeyCode.A) && stamina > 0)
        {
            stamina -= decreaseRate * Time.deltaTime;
            stamina = Mathf.Max(0, stamina); // 0未満にならないように制限
        }
        // スペースを押していない時は自動回復
        else if (stamina < maxStamina)
        {
            stamina += recoverRate * Time.deltaTime;
            stamina = Mathf.Min(maxStamina, stamina); // 最大値を超えないよう制限
        }

        // スライダーを滑らかに更新
        float smoothedValue = Mathf.SmoothDamp(staminaSlider.value, stamina, ref currentVelocity, smoothTime);
        staminaSlider.value = smoothedValue;
        UpdateGaugeColor();
    }
    private void UpdateGaugeColor()
    {
        if (fillImage == null) return;

        float ratio = stamina / maxStamina;

        if (ratio > 0.3f) // 30%以上 → 緑
        {
            fillImage.color = Color.green;
        }
        else if (ratio > 0.1f) // 10%以上30%未満 → 黄
        {
            fillImage.color = Color.yellow;
        }
        else // 10%以下 → 赤
        {
            fillImage.color = Color.red;
        }
    }
}
