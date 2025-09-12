using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump : MonoBehaviour
{
    float jumpForce = 7;
    bool isJumpWait;
    Animator anim; //Animator
    float jumpWaitTimer;
    bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("Animator‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
        if (GetComponent<Rigidbody>() == null) Debug.LogError("Rigidbody‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
    }
    void FixedUpdate()
    {
        // Update‚æ‚èŒÅ’èŠÔ‚Å‚Ì•¨—”»’è‚ªˆÀ’è
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.3f);
    }

    void Update()
    {
        if ((Input.GetKeyDown("space") || Input.GetKeyDown("joystick button 5")) && isGrounded && !isJumpWait)
        {
            if (anim != null) anim.Play("Jump", 0, 0);
            isJumpWait = true;
            jumpWaitTimer = 0.5f;
        }
        if (isJumpWait)
        {
            jumpWaitTimer -= Time.deltaTime;
            if (jumpWaitTimer < 0)
            {
                var rb = GetComponent<Rigidbody>();
                if (rb != null) rb.linearVelocity = transform.up * jumpForce;
                isJumpWait = false;
            }
        }
    }
}
