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
        playerController = player;

        // 攻撃判定用GameObject取得・非アクティブ化
        attackHitbox = player.transform.Find("AttackHitbox")?.gameObject;
        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isAttackingPlaying = false;
    }

    public void UpdateState(PlayerController player)
    {
        float rt = Input.GetAxis("RT");

        if (!isAttackingPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") || rt > 0.5f)
            {
                // 攻撃開始：当たり判定を有効化
                if (attackHitbox != null)
                    attackHitbox.SetActive(true);

                isAttackingPlaying = true;

                Debug.LogError("しばく");
                // 攻撃アニメーションがあればここで再生する
                // playerController.GetComponent<Animator>().SetTrigger("Attack");
            }
        }
        else
        {
            // 攻撃終了の判定はアニメーション時間や別トリガーで行うのが望ましい
            // ここでは簡単に一定時間後に当たり判定無効化と状態遷移を行う例

            // ここは例なので攻撃持続時間1秒後に終了させましょう
            playerController.StartCoroutine(EndAttackAfterDelay(1.0f));
        }
    }

    public void ExitState(PlayerController player)
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
        isAttackingPlaying = false;
    }

    private IEnumerator EndAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isAttackingPlaying = false;

        // 攻撃終了後、アイドル状態に戻す
        playerController.ChangeState(playerController.idelState);
    }
}
