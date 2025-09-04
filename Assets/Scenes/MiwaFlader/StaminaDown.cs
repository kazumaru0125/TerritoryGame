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
    [SerializeField] private Image fillImage;         // スライダーのFill部分

    private float stamina;              // 現在のスタミナ量
    private float currentVelocity = 0f; // SmoothDamp用の速度
    [SerializeField] private float smoothTime = 0.2f; // スライダーが追従する速度

    void Start()
    {
        // スタミナ初期化
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        UpdateGaugeColor();
    }

    void Update()
    {
        // Aキーを押している間はスタミナを減らす
        if (Input.GetKey(KeyCode.A) && stamina > 0)
        {
            stamina -= decreaseRate * Time.deltaTime;
            stamina = Mathf.Max(0, stamina); // 0未満にならないよう制御
        }
        // そうでなければ回復
        else if (stamina < maxStamina)
        {
            stamina += recoverRate * Time.deltaTime;
            stamina = Mathf.Min(maxStamina, stamina); // 最大値を超えないよう制御
        }

        // スライダーを滑らかに更新
        float smoothedValue = Mathf.SmoothDamp(staminaSlider.value, stamina, ref currentVelocity, smoothTime);
        staminaSlider.value = smoothedValue;

        // ゲージの色を更新
        UpdateGaugeColor();
    }

    private void UpdateGaugeColor()
    {
        if (fillImage == null) return;

        float ratio = stamina / maxStamina;

        if (ratio > 0.3f)       // 30%以上 → 緑
        {
            fillImage.color = Color.green;
        }
        else if (ratio > 0.1f)  // 10%以上30%未満 → 黄
        {
            fillImage.color = Color.yellow;
        }
        else                    // 10%未満 → 赤
        {
            fillImage.color = Color.red;
        }
    }
}
