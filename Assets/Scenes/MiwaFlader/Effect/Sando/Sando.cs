using UnityEngine;

public class PlayerMoveWithSmoke : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("砂煙エフェクト")]
    [SerializeField] private ParticleSystem footSmoke;

    void Start()
    {
        // エフェクトが設定されていたら止めておく
        if (footSmoke != null)
            footSmoke.Stop();
    }

    void Update()
    {
        // --- 入力取得 ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // --- 移動処理 ---
        Vector3 move = new Vector3(h, 0, v).normalized * moveSpeed * Time.deltaTime;
        transform.position += move;

        // --- 入力有無でエフェクト制御 ---
        if (h != 0 || v != 0) // 何かキーが押されている
        {
            // 反対方向を求める（進行方向の逆）
            Vector3 oppositeDir = new Vector3(h, 0, v).normalized * -1f;

            if (oppositeDir != Vector3.zero)
            {
                // エフェクトの向きを進行方向の逆に回転させる
                footSmoke.transform.rotation = Quaternion.LookRotation(oppositeDir);
            }

            if (!footSmoke.isPlaying)
                footSmoke.Play();
        }
        else // 入力がない（キーを離した）
        {
            if (footSmoke.isPlaying)
                footSmoke.Stop();
        }
    }
}
