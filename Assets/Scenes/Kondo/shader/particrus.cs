using UnityEngine;

public class particrus : MonoBehaviour
{
    // 操作したいParticle Systemをインスペクターで指定できるようにする
    public ParticleSystem particleSystem;

    // 起動時に一度だけ呼ばれる
    void Start()
    {
        // ParticleSystemが未設定なら同じオブジェクトから自動取得
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>(); // 自動取得
        }

        // それでも見つからなければ警告を出す
        if (particleSystem == null)
        {
            Debug.LogWarning("Particle Systemが設定されていません。インスペクターで指定してください。");
        }
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // Lキーを押した瞬間の処理
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Particle Systemが存在する場合のみ処理する
            if (particleSystem != null)
            {
                particleSystem.Stop();  // まず停止
                particleSystem.Clear(); // すべてのパーティクルをクリア
                particleSystem.Play();  // もう一度再生しなおす
            }
        }

        // Kキーを押した瞬間の処理
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Particle Systemが存在する場合のみ処理する
            if (particleSystem != null)
            {
                // ParticleSystemのメイン設定を一時的に取得
                var main = particleSystem.main;

                // ループをオフにする
                main.loop = false;

                // 状況をコンソールに出力（確認用）
                Debug.Log("Particleのループをオフにしました。");
            }
        }
    }
}
