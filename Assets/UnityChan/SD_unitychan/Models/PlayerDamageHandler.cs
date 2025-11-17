using UnityEngine;
using System.Collections;
using Photon.Pun;

public class PlayerDamageHandler : MonoBehaviourPun
    {
    private Animator animator;
    private PlayerRespawnScript respawnScript;

    [Header("Damage Particle")]
    public ParticleSystem damageParticle; // インスペクターでセット

    void Start()
        {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animatorが見つかりません");

        respawnScript = GetComponent<PlayerRespawnScript>();
        if (respawnScript == null)
            respawnScript = GetComponentInParent<PlayerRespawnScript>();

        if (damageParticle == null)
            Debug.LogWarning("Damage Particleが設定されていません");
        }

    private void Update()
        {
        if (Input.GetKeyUp(KeyCode.M))
            {
            // パーティクルが存在し、生きていて、Destroyされていない場合のみ再生
            if (IsValidParticle())
                damageParticle.Play();
            }
        }

    public void PlayDamageAnimation()
        {
        photonView.RPC(nameof(RPC_PlayDamageAnimation), RpcTarget.All);
        }

    [PunRPC]
    private void RPC_PlayDamageAnimation()
        {
        StartCoroutine(DamageRoutine());
        }

    private IEnumerator DamageRoutine()
        {
        if (animator != null)
            animator.SetBool("is_damage", true);

        // ここでも安全チェック
        if (IsValidParticle())
            damageParticle.Play();

        yield return new WaitForSeconds(1.0f);

        if (animator != null)
            animator.SetBool("is_damage", false);

        // リスポーン処理（自分のキャラのみ）
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
            if (status == null) return;

            status.StartHitbox();

            if (status.isAttacking)
                {
                Debug.Log("ダメージ受けました（Stay）");
                PlayDamageAnimation(); // RPC
                status.EndHitbox();
                }
            }
        }

    /// <summary>
    /// パーティクルが null / Destroy済み / 破棄直前かを安全に判定
    /// </summary>
    private bool IsValidParticle()
        {
        // Unity の Destroy 判定は null チェックで捕まるためこれでOK
        return damageParticle != null && damageParticle.gameObject != null;
        }
    }
