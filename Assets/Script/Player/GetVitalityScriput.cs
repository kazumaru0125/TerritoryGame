using UnityEngine;

public class GetVitalityScriput : MonoBehaviour
    {
    public int Vitality;

    private void OnCollisionEnter(Collision collision)
        {
        if (collision.gameObject.CompareTag("vitality"))
            {
            // シーン内の DecreaseTMPNumber を探して Aチームゲージに加算
            DecreaseTMPNumber manager = FindObjectOfType<DecreaseTMPNumber>();
            if (manager != null)
                {
                manager.AddATeamVitality(Vitality); // 1だけ加算（必要なら値を変える）
                }

            // 触れたvitalityオブジェクトを削除する場合
            Destroy(collision.gameObject);
            }
        }
    }
