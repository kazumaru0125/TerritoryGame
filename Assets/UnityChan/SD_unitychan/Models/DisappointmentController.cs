using UnityEngine;
using Photon.Pun;

public class DisappointmentController : MonoBehaviourPun
    {
    private Animator anim;
    private bool isDisappointmentPlaying = false;

    void Start()
        {
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.LogError("Animator が見つかりません！");
        }

    void Update()
        {
        // 他プレイヤーは操作しない
        if (!photonView.IsMine)
            return;

        // 入力受付
        if (!isDisappointmentPlaying)
            {
            if (Input.GetKeyDown(KeyCode.O))
                {
                isDisappointmentPlaying = true;
                photonView.RPC(nameof(PlayDisappointmentAnimation), RpcTarget.All);
                }
            }
        else
            {
            // アニメーション終了チェック（ローカルだけでOK）
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            // Animator の state 名は実際のステート名に合わせる
            if (!stateInfo.IsName("is_Disappointmenting") || stateInfo.normalizedTime >= 1f)
                {
                isDisappointmentPlaying = false;
                }
            }
        }

    [PunRPC]
    public void PlayDisappointmentAnimation()
        {
        anim.SetBool("is_Disappointmenting", true);

        // すぐオフにすると再生されないため、AnimatorController側で
        // Transition Exit Time を使うのが推奨
        // 終了後に自動で false に戻したいなら Coroutine を使う
        StartCoroutine(ResetBoolAfterAnimation());
        }

    private System.Collections.IEnumerator ResetBoolAfterAnimation()
        {
        yield return new WaitForEndOfFrame();

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // 長さ分待つ
        yield return new WaitForSeconds(stateInfo.length);

        anim.SetBool("is_Disappointmenting", false);
        }
    }
