using UnityEngine;

public class JumpingPlatform : MonoBehaviour
    {
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float jumpCooldown = 5f; // 5秒のインターバル
    private bool jumpFlag = false;
    private float timer = 0f;

    private void Update()
        {
        // ジャンプ後、インターバルがある場合
        if (jumpFlag)
            {
            timer -= Time.deltaTime; // 秒単位で減らす
            if (timer <= 0f)
                {
                jumpFlag = false; // 再びジャンプ可能に
                Debug.Log("JumpFlagリセット");
                }
            }
        }

    private void OnCollisionEnter(Collision collision)
        {
        if (collision.gameObject.CompareTag("Player"))
            {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
                {
                if (!jumpFlag)
                    {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    jumpFlag = true;
                    timer = jumpCooldown; // タイマーをリセット
                    Debug.Log("Jumpしました");
                    }
                else
                    {
                    Debug.Log("Jumpできません（クールダウン中）");
                    }
                }
            else
                {
                Debug.LogWarning("Player に Rigidbody がありません！");
                }
            }
        }
    }
