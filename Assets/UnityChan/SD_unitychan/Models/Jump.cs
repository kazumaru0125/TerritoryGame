using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jump : MonoBehaviour
{
    float jumpForce = 5;
    bool isJumpWait; //ï¿½Wï¿½ï¿½ï¿½ï¿½ï¿½vï¿½Ò‹@ï¿½tï¿½ï¿½ï¿½O
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
<<<<<<< HEAD
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // ï¿½Xï¿½yï¿½[ï¿½Xï¿½Lï¿½[ï¿½Ü‚ï¿½ï¿½ï¿½Xboxï¿½Rï¿½ï¿½ï¿½gï¿½ï¿½ï¿½[ï¿½ï¿½ï¿½[ï¿½ï¿½Aï¿½{ï¿½^ï¿½ï¿½(joystick button 5)ï¿½ÅƒWï¿½ï¿½ï¿½ï¿½ï¿½vï¿½ï¿½ï¿½ï¿½
=======
>>>>>>> Character
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
<<<<<<< HEAD
                GetComponent<Rigidbody>().linearVelocity = transform.up * jumpForce;
=======
                var rb = GetComponent<Rigidbody>();
                if (rb != null) rb.linearVelocity = transform.up * jumpForce;
>>>>>>> Character
                isJumpWait = false;
            }
        }
    }
}
