using UnityEngine;
using System.Collections;

public class BearTrap : MonoBehaviour
    {
    private Animation anim;

    void Start()
        {
        anim = GetComponent<Animation>();

        // 念のため、最初に止めておく
        if (anim != null)
            anim.Stop();
        }

    void Update()
        {
        // Rキーで動かす
        if (Input.GetKeyDown(KeyCode.R))
            {
            PlayTrapAnimation();
            }
        }

    private void OnCollisionEnter(Collision collision)
        {
        // もし触れたオブジェクトのタグが "Player" なら
        if (collision.gameObject.CompareTag("Player"))
            {
            PlayTrapAnimation();
            }
        }

    void PlayTrapAnimation()
        {
        if (anim != null)
            {
            anim.Play();  // 再生開始
            Debug.Log("BearTrapアニメーション開始！");
            StartCoroutine(DeleteAfterAnimation());
            }
        }

    IEnumerator DeleteAfterAnimation()
        {
        // 現在のアニメーションクリップの長さを取得
        float clipLength = anim.clip.length;

        // アニメーション終了まで待つ
        yield return new WaitForSeconds(clipLength);

        // さらに5秒待ってから削除
        yield return new WaitForSeconds(3f);

        Debug.Log("BearTrap削除");
        Destroy(gameObject);
        }
    }
