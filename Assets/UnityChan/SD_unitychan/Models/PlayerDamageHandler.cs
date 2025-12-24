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
                //  damageParticle.Play();
                SafePlayDamageParticle();
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

    //private IEnumerator DamageRoutine()
    //    {
    //    if (animator != null)
    //        animator.SetBool("is_damage", true);

    //    // ここでも安全チェック
    //    if (IsValidParticle())
    //        //damageParticle.Play();
    //        SafePlayDamageParticle();
    //    yield return new WaitForSeconds(1.0f);

    //    if (animator != null)
    //        animator.SetBool("is_damage", false);

    //    // ダメージを受けたらチームに1ダメージ加算
    //    // ダメージ時のチーム加算
    //    if (photonView.IsMine)
    //        {
    //        TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
    //        if (role != null)
    //            {
    //            role.photonView.RPC("AddTeamDamageRPC", RpcTarget.MasterClient, role.CurrentTeam);
    //            Debug.Log("[PlayerDamageHandler] AddTeamDamageRPC 呼び出し: " + role.CurrentTeam);
    //            }
    //        }



    //    // リスポーン処理（自分のキャラのみ）
    //    if (photonView.IsMine && respawnScript != null)
    //        {
    //        Debug.Log("ダメージ後にリスポーン実行");
    //        respawnScript.RespawnAtRandomSpawnArea();
    //        }



    //    }


    private IEnumerator DamageRoutine()
        {
        if (animator != null)
            animator.SetBool("is_damage", true);

        if (IsValidParticle())
            SafePlayDamageParticle();

        yield return new WaitForSeconds(1.0f);

        if (animator != null)
            animator.SetBool("is_damage", false);

        if (photonView.IsMine)
            {
            TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
            if (role != null)
                {
                // 🔵 ダメージ加算（1回だけ）
                role.photonView.RPC("AddTeamDamageRPC", RpcTarget.MasterClient, role.CurrentTeam);

                // 🔵 ★死亡スコア -10（安全にここで）
                role.photonView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, -10);

                Debug.Log("[PlayerDamageHandler] Damage & Score -10: " + role.CurrentTeam);
                }
            }

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

    private void SafePlayDamageParticle()
        {
        // 完全に Destroy 済みも検出
        if (damageParticle == null) return;

        try
            {
            damageParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            damageParticle.Play(true);
            }
        catch
            {
            Debug.LogWarning("Damage Particle は Destroy 済みのため再生できません");
            }
        }



    }
