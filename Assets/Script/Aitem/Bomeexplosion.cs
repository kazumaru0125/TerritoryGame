using UnityEngine;
using Photon.Pun;

public class Bomeexplosion : MonoBehaviourPun
    {
    public ParticleSystem FastFire;
    public ParticleSystem Explosion;
    public GameObject HitBox;

    public bool ExplosionFlag;

    void Start()
        {
        Explosion.Stop();
        ExplosionFlag = false;
        HitBox.SetActive(false);

        if (photonView.IsMine)   // 所有者のみタイマー処理
            {
            StartCoroutine(ExplosionRoutine());
            }
        }

    private System.Collections.IEnumerator ExplosionRoutine()
        {
        yield return new WaitForSeconds(1f);

        photonView.RPC(nameof(RPC_ExplosionStart), RpcTarget.All);

        yield return new WaitForSeconds(1f);

        photonView.RPC(nameof(RPC_DestroySelf), RpcTarget.All);
        }

    [PunRPC]
    void RPC_ExplosionStart()
        {
        ExplosionFlag = true;

        if (FastFire != null) FastFire.Stop();
        if (Explosion != null) Explosion.Play();
        if (HitBox != null) HitBox.SetActive(true);
        }

    [PunRPC]
    void RPC_DestroySelf()
        {
        if (HitBox != null) HitBox.SetActive(false);
        Destroy(gameObject);
        }
    }
