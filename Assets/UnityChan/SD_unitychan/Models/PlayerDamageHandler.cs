using UnityEngine;
using System.Collections;

public class PlayerDamageHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animatorが見つかりません");
    }

    public void PlayDamageAnimation()
    {
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        animator.SetBool("is_damage", true);

        // ダメージアニメーションの長さや適当な無敵時間など
        yield return new WaitForSeconds(1.0f);

        animator.SetBool("is_damage", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackHitbox"))
        {
            var status = other.GetComponent<AttackHitboxStatus>();
            if (status != null && status.isAttacking)
            {
                Debug.Log("ダメージ受けました");
                PlayDamageAnimation();
            }
            else
            {
                Debug.Log("攻撃判定はあるがisAttackingがfalse");
            }
        }
    }
}
