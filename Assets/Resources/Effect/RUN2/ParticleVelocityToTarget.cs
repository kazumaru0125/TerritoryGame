using UnityEngine;

public class ParticleVelocityToTarget : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;        // 対象パーティクル
    [SerializeField] private Transform target;         // パーティクルが向かう先

    [Header("軸ごとの吸引強さ（マルチプライヤー）")]
    [SerializeField] private float xPower = 1f;        // X方向の強さ
    [SerializeField] private float yPower = 1f;        // Y方向の強さ
    [SerializeField] private float zPower = 1f;        // Z方向の強さ

    [Header("パーティクル位置オフセット")]
    [SerializeField] private Vector3 offset = Vector3.zero; // プレイヤーに対してどこに置くか

    private void Start()
    {
        // パーティクルが設定されていなければ取得
        if (ps == null)
            ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (ps == null || target == null) return;

        // ★★★ パーティクルの位置を Player の位置に毎フレーム合わせる ★★★
        ps.transform.position = target.position + offset;

        // Velocity over Lifetime モジュール取得
        var vel = ps.velocityOverLifetime;

        // パーティクルが目指す方向を計算
        Vector3 dir = (target.position - ps.transform.position).normalized;

        // 軸ごとに Offset を適用
        vel.orbitalOffsetX = dir.x * xPower;
        vel.orbitalOffsetY = dir.y * yPower;
        vel.orbitalOffsetZ = dir.z * zPower;
    }
}
