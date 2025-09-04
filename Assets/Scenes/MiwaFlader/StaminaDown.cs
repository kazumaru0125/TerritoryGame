using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaGauge : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField] private float maxStamina = 100f;   // æœ€å¤§ã‚¹ã‚¿ãƒŸãƒŠ
    private float currentStamina;                       // ç¾åœ¨ã®ã‚¹ã‚¿ãƒŸãƒŠ

    [SerializeField] private float decreaseAmount = 10f; // æ¶ˆè²»é‡
    [SerializeField] private float recoverySpeed = 20f;   // å›å¾©é€Ÿåº¦

    [SerializeField] private Slider staminaSlider;       // ã‚¹ãƒ©ã‚¤ãƒ€ãƒ¼UI
    [SerializeField] private Image fillImage;            // ã‚¹ãƒ©ã‚¤ãƒ€ãƒ¼ã®Filléƒ¨åˆ†
=======
    [SerializeField] private Slider staminaSlider;   // ƒXƒ^ƒ~ƒiƒQ[ƒW‚ÌƒXƒ‰ƒCƒ_[
    [SerializeField] private float maxStamina = 5f;  // Å‘åƒXƒ^ƒ~ƒi
    [SerializeField] private float decreaseRate = 1f; // 1•b‚ ‚½‚è‚ÌÁ”ï—Ê
    [SerializeField] private float recoverRate = 1f;  // 1•b‚ ‚½‚è‚Ì‰ñ•œ—Ê
    [SerializeField] private Image fillImage;         // ƒXƒ‰ƒCƒ_[‚ÌFill•”•ª‚ÌImage

    private float stamina;              // ÀÛ‚ÌƒXƒ^ƒ~ƒi’l
    private float currentVelocity = 0f; // SmoothDamp—p
    [SerializeField] private float smoothTime = 0.2f; // ŠŠ‚ç‚©‚É’Ç]‚·‚éŠÔ
>>>>>>> UI

    void Start()
    {
        // ‰Šú‰»
        stamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        UpdateGaugeColor();
    }

    void Update()
    {
<<<<<<< HEAD
        // Spaceã‚’æŠ¼ã—ãŸã‚‰ã‚¹ã‚¿ãƒŸãƒŠæ¶ˆè²»
        if (Input.GetKeyDown(KeyCode.Space))
=======
        // ƒXƒy[ƒXƒL[‚ğ‰Ÿ‚µ‘±‚¯‚Ä‚¢‚éŠÔAƒXƒ^ƒ~ƒi‚ğÁ”ï
        if (Input.GetKey(KeyCode.A) && stamina > 0)
>>>>>>> UI
        {
            stamina -= decreaseRate * Time.deltaTime;
            stamina = Mathf.Max(0, stamina); // 0–¢–‚É‚È‚ç‚È‚¢‚æ‚¤‚É§ŒÀ
        }
        // ƒXƒy[ƒX‚ğ‰Ÿ‚µ‚Ä‚¢‚È‚¢‚Í©“®‰ñ•œ
        else if (stamina < maxStamina)
        {
            stamina += recoverRate * Time.deltaTime;
            stamina = Mathf.Min(maxStamina, stamina); // Å‘å’l‚ğ’´‚¦‚È‚¢‚æ‚¤§ŒÀ
        }

<<<<<<< HEAD
        // è‡ªç„¶å›å¾©
        if (currentStamina < maxStamina)
=======
        // ƒXƒ‰ƒCƒ_[‚ğŠŠ‚ç‚©‚ÉXV
        float smoothedValue = Mathf.SmoothDamp(staminaSlider.value, stamina, ref currentVelocity, smoothTime);
        staminaSlider.value = smoothedValue;
        UpdateGaugeColor();
    }
    private void UpdateGaugeColor()
    {
        if (fillImage == null) return;

        float ratio = stamina / maxStamina;

        if (ratio > 0.3f) // 30%ˆÈã ¨ —Î
>>>>>>> UI
        {
            fillImage.color = Color.green;
        }
        else if (ratio > 0.1f) // 10%ˆÈã30%–¢– ¨ ‰©
        {
            fillImage.color = Color.yellow;
        }
        else // 10%ˆÈ‰º ¨ Ô
        {
            fillImage.color = Color.red;
        }
<<<<<<< HEAD

        // ã‚¹ãƒ©ã‚¤ãƒ€ãƒ¼ã«åæ˜ 
        staminaSlider.value = currentStamina;

        // ã‚¹ã‚¿ãƒŸãƒŠé‡ã«å¿œã˜ã¦è‰²ã‚’å¤‰æ›´ï¼ˆ100%â†’ç·‘ã€50%â†’é»„è‰²ã€0%â†’èµ¤ï¼‰
        float ratio = currentStamina / maxStamina;
        fillImage.color = Color.Lerp(Color.red, Color.green, ratio);
=======
>>>>>>> UI
    }
}
