using UnityEngine;

public class Confusion : MonoBehaviour
{
    [Header("吸収させたいパーティクル")]
    [SerializeField] private ParticleSystem ps;            // 対象パーティクル

    [Header("パーティクルが向かう吸収ポイント")]
    [SerializeField] private Transform target;             // Player の体に置いた吸収ポイント

    [Header("吸引速度")]
    [SerializeField] private float speed = 5f;             // パーティクルが向かう速さ

    [Header("吸引が有効になる最大距離")]
    [SerializeField] private float enableDistance = 5f;    // この距離以内だけスクリプトを有効にする

    private ParticleSystem.Particle[] particles;           // パーティクル配列

    void LateUpdate()
    {
        // パーティクル or ターゲットが設定されていない場合は何もしない
        if (ps == null || target == null) return;

        // パーティクル配列が未作成の場合は初期化
        if (particles == null || particles.Length < ps.main.maxParticles)
            particles = new ParticleSystem.Particle[ps.main.maxParticles];

        // 現在生存しているパーティクル数を取得
        int count = ps.GetParticles(particles);

        // 吸収ポイントの位置を取得
        Vector3 targetPos = target.position;

        // すべてのパーティクルを処理
        for (int i = 0; i < count; i++)
        {
            // 吸収ポイントとの距離を計算
            float distance = Vector3.Distance(particles[i].position, targetPos);

            // 距離が指定範囲外なら吸収処理を行わずスキップ
            if (distance > enableDistance)
                continue;

            // 吸収ポイントへの方向を計算
            Vector3 direction = (targetPos - particles[i].position).normalized;

            // 吸収方向にパーティクルを移動
            particles[i].position += direction * speed * Time.deltaTime;
        }

        // パーティクルを更新
        ps.SetParticles(particles, count);
    }
}
