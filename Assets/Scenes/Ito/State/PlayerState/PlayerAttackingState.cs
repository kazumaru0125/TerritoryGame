using System.Collections;
using UnityEngine;

public class PlayerAttackingState : IPlayerState
{
    private bool isAttackingPlaying = false;
    private PlayerController playerController;

    // 攻撃判定用GameObject
    private GameObject attackHitbox;

    public void EnterState(PlayerController player)
    {
        Debug.Log("攻撃開始");
        playerController = player;
        // 攻撃判定用GameObject取得・非アクティブ化
        attackHitbox = player.transform.Find("AttackHitbox")?.gameObject;
        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isAttackingPlaying = false;
    }

    public void UpdateState(PlayerController player)
    {
        Debug.Log("攻撃中: hitbox有効化");
        //float rt = Input.GetAxis("RT");

        //if (!isAttackingPlaying)
        //{
        //    if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
        //    {
        //        Debug.Log("攻撃中: hitbox有効化");
        //        if (attackHitbox != null)
        //        {
        //            attackHitbox.SetActive(true);
        //            attackHitbox.GetComponent<AttackHitboxStatus>().isAttacking = true;
        //        }
        //        isAttackingPlaying = true;
        //        playerController.StartCoroutine(EndAttackAfterDelay(0.3f));
        //    }
        //}
    }


    public void ExitState(PlayerController player)
    {
        Debug.Log("攻撃終了");
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
        isAttackingPlaying = false;
    }

    private IEnumerator EndAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        attackHitbox.GetComponent<AttackHitboxStatus>().isAttacking = false;

        isAttackingPlaying = false;

        // 攻撃終了後アイドル状態へ
        playerController.ChangeState(playerController.idelState);
    }

}