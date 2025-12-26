using UnityEngine;

public class Confusion : MonoBehaviour
    {
    [Header("吸収させたいパーティクル")]
    [SerializeField] private ParticleSystem ps;

    [Header("パーティクルが向かう吸収ポイント（自動取得）")]
    [SerializeField] private Transform target;

    [Header("吸引速度")]
    [SerializeField] private float speed = 5f;

    [Header("吸引が有効になる最大距離 (Player 距離)")]
    [SerializeField] private float enableDistance = 5f;

    [Header("サイズ変化")]
    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 1.2f;

    private ParticleSystem.Particle[] particles;

    void Start()
        {
        if (target == null)
            {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogWarning("Playerタグのオブジェクトが見つかりません。");
            }
        }

    void LateUpdate()
        {
        if (ps == null || target == null) return;

        // --- Bボタン押していないなら非表示にして終了 ---
        if (!Input.GetKey(KeyCode.JoystickButton1))
            {
            if (ps.isPlaying)
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            return;
            }
        else
            {
            if (!ps.isPlaying)
                ps.Play();
            }

        // --- Player が範囲外なら処理しない ---
        float playerDistance = Vector3.Distance(ps.transform.position, target.position);
        if (playerDistance > enableDistance)
            return;

        // --- パーティクル吸引処理 ---
        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        int count = ps.GetParticles(particles);
        Vector3 targetPos = target.position;

        for (int i = 0; i < count; i++)
            {
            float distance = Vector3.Distance(particles[i].position, targetPos);

            // ⭐ 距離に応じてサイズを大きくする ⭐
            float t = Mathf.Clamp01(1f - (distance / enableDistance));
            float size = Mathf.Lerp(minSize, maxSize, t);
            particles[i].startSize = size;

            // 吸引移動
            Vector3 dir = (targetPos - particles[i].position).normalized;
            particles[i].position += dir * speed * Time.deltaTime;
            }

        ps.SetParticles(particles, count);
        }
    }
