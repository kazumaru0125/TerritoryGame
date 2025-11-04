using UnityEngine;
using System.Collections;

public class PlayerDamageHandler : MonoBehaviour
    {
    private Animator animator;
    private PlayerRespawnScript respawnScript; // ← 追加

    void Start()
        {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animatorが見つかりません");

        // 同じオブジェクト、または親オブジェクトにあるPlayerRespawnScriptを取得
        respawnScript = GetComponent<PlayerRespawnScript>();
        if (respawnScript == null)
            respawnScript = GetComponentInParent<PlayerRespawnScript>();
        }

    public void PlayDamageAnimation()
        {
        StartCoroutine(DamageRoutine());
        }

    private IEnumerator DamageRoutine()
        {
        animator.SetBool("is_damage", true);

        // ダメージアニメーション時間
        yield return new WaitForSeconds(1.0f);

        if (this != null && gameObject != null)
            {
            animator.SetBool("is_damage", false);
            }

//        animator.SetBool("is_damage", false);

        // ★ ダメージ後にリスポーンを呼び出す
        if (respawnScript != null)
            {
            Debug.Log("ダメージ後にリスポーン実行");
            respawnScript.RespawnAtRandomSpawnArea();
            }
        else
            {
            Debug.LogWarning("PlayerRespawnScriptが見つからなかったためリスポーンできません。");
            }
        }

    private void OnTriggerStay(Collider other)
        {
        var status = other.GetComponent<AttackHitboxStatus>();
        Debug.Log("[OnTriggerStay] " + (status != null ? status.isAttacking.ToString() : "null"));
        if (other.gameObject.CompareTag("AttackHitbox"))
            {
            status.StartHitbox();
            if (status != null && status.isAttacking)
                {
                Debug.Log("ダメージ受けました（Stay）");
                PlayDamageAnimation();
                status.EndHitbox();
                }
            }
        }
    }
