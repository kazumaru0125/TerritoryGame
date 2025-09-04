using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiasmaDown : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100f;   // 最大瘴気
    private float currentStamina;                       // 現在の瘴気

    [SerializeField] private float decreaseAmount = 10f; // 消費量
    [SerializeField] private float recoverySpeed = 20f;   // 回復速度

    [SerializeField] private Slider staminaSlider;       // スライダーUI
    [SerializeField] private Image fillImage;            // スライダーのFill部分

    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    void Update()
    {
        // Spaceを押したらスタミナ消費
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentStamina -= decreaseAmount;
            if (currentStamina < 0f)
                currentStamina = 0f;
        }

        // 自然回復
        if (currentStamina < maxStamina)
        {
            currentStamina += recoverySpeed * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }

        // スライダーに反映
        staminaSlider.value = currentStamina;

        // スタミナ量に応じて色を変更（100%→緑、50%→黄色、0%→赤）
        float ratio = currentStamina / maxStamina;
        fillImage.color = Color.Lerp(Color.red, Color.yellow, ratio);
    }
}
