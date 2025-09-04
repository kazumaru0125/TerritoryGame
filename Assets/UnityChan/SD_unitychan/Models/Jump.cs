using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump : MonoBehaviour
{
    float jumpForce = 5;
    bool isJump, isJumpWait; //ジャンプフラグの設定。Unityちゃんが飛んでいるか否か。
    Animator anim; //Unityちゃんのジャンプ設定するためのAnimator
    float jumpWaitTimer; //ジャンプ待機時間
    bool isGrounded;  // 地面にいるか
    // Start is called before the first frame update
    void Start()
    {
        //UntiyちゃんのAnimatorを取得する。
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
<<<<<<< HEAD

=======
>>>>>>> Character
        if (Input.GetKeyDown("space") && isGrounded && !isJumpWait)
        {
            anim.Play("Jump", 0, 0);
            isJumpWait = true;
            jumpWaitTimer = 0.5f;
        }
        if (isJumpWait)
        {
            jumpWaitTimer -= Time.deltaTime;
            if (jumpWaitTimer < 0)
            {
                GetComponent<Rigidbody>().velocity = transform.up * jumpForce;
                isJumpWait = false;
            }
        }
    }

}