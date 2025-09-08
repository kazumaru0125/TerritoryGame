using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump : MonoBehaviour
{
    float jumpForce = 5;
    bool isJumpWait; //ジャンプ待機フラグ
    Animator anim; //Animator
    float jumpWaitTimer;
    bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // スペースキーまたはXboxコントローラーのAボタン(joystick button 5)でジャンプ判定
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 5")) && isGrounded && !isJumpWait)
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
