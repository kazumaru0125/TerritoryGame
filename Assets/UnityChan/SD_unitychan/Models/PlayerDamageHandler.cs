using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerDamageHandler : MonoBehaviourPun
    {
    private Animator animator;
    private PlayerRespawnScript respawnScript;

    void Start()
        {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animatorが見つかりません");

        respawnScript = GetComponent<PlayerRespawnScript>();
        if (respawnScript == null)
            respawnScript = GetComponentInParent<PlayerRespawnScript>();
        }

    public void PlayDamageAnimation()
        {
        //// 自分のキャラでなければアニメーションをトリガーしない
        //if (!photonView.IsMine)
        //    return;

        // RPCで全員に通知
        photonView.RPC(nameof(RPC_PlayDamageAnimation), RpcTarget.All);
        }

    [PunRPC]
    private void RPC_PlayDamageAnimation()
        {
        StartCoroutine(DamageRoutine());
        }

    private IEnumerator DamageRoutine()
        {
        if (animator == null) yield break;

        animator.SetBool("is_damage", true);

        // ダメージアニメーションの再生時間
        yield return new WaitForSeconds(1.0f);

        if (this != null && gameObject != null)
            animator.SetBool("is_damage", false);

        // リスポーン処理は自分のプレイヤーのみ実行
        if (photonView.IsMine && respawnScript != null)
            {
            Debug.Log("ダメージ後にリスポーン実行");
            respawnScript.RespawnAtRandomSpawnArea();
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
                PlayDamageAnimation(); // ← RPC経由で全員に同期される
                status.EndHitbox();
                }
            }
        }
    }
