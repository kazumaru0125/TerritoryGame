using UnityEngine;

public class PrayerController : MonoBehaviour
    {
    Animator anim;

    [Header("Getvitality")]
    public ParticleSystem vitalityParticle;

    private bool prevPrayering = false;

    void Start()
        {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animatorが見つかりません！");

        if (vitalityParticle == null)
            {
            Debug.LogWarning("Vitality Particle が設定されていません");
            }
        else
            {
            // ▼ 最初は完全に非表示（停止 & クリア）
            vitalityParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

    void Update()
        {
        bool isKeyHeld =
            Input.GetKey(KeyCode.F) ||
            Input.GetKey("joystick button 1");

        // アニメーション用フラグ更新
        anim.SetBool("is_prayering", isKeyHeld);

        // ▼ 状態変化に応じて Particle を再生/停止
        if (isKeyHeld && !prevPrayering)
            {
            // 祈り開始 → Particle 再生
            SafePlayVitalityParticle();
            }
        else if (!isKeyHeld && prevPrayering)
            {
            // 祈り終了 → Particle 停止
            if (vitalityParticle != null)
                {
                vitalityParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }

        prevPrayering = isKeyHeld;
        }

    private void SafePlayVitalityParticle()
        {
        if (vitalityParticle == null) return;

        try
            {
            vitalityParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            vitalityParticle.Play(true);
            }
        catch
            {
            Debug.LogWarning("vitalityParticle は Destroy 済みのため再生できません");
            }
        }
    }
