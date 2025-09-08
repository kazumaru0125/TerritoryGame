using UnityEngine;
using UnityEngine.UI;

public class StaminaGaugeController : MonoBehaviour
{
    [SerializeField] private Material staminaMat; // マテリアルをInspectorで設定
    [SerializeField] private float maxStamina = 100f;
    private float currentStamina;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateGauge();
    }

    private void Update()
    {
        // スペースキーで消費する例
        if (Input.GetKey(KeyCode.Space))
        {
            currentStamina -= 20f * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateGauge();
        }
        else
        {
            // 自然回復
            currentStamina += 10f * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            UpdateGauge();
        }
    }

    private void UpdateGauge()
    {
        float fillAmount = currentStamina / maxStamina;
        staminaMat.SetFloat("_FillAmount", fillAmount);
    }
}
