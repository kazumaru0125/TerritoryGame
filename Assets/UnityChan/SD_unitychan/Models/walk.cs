using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnCallChangeFace()
    {
        // âΩÇ‡èàóùÇµÇ»Ç≠ÇƒOK
    }

    void Update()
    {
        bool isWalking = false;
        bool isRunning = false;
        Vector3 move = Vector3.zero;

        // ëOå„
        if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move += transform.forward * speed;
            if (speed == 0.01f) isRunning = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey("down"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move -= transform.forward * speed;
            if (speed == 0.01f) isRunning = true;
        }
        // ç∂âE
        if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move -= transform.right * speed;
            if (speed == 0.01f) isRunning = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move += transform.right * speed;
            if (speed == 0.01f) isRunning = true;
        }

        // é¿ç€ÇÃà⁄ìÆ
        transform.position += move;

        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
    }
}
