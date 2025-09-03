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
        // ‰½‚àˆ—‚µ‚È‚­‚ÄOK
    }

    void Update()
    {
        bool isWalking = false;
        bool isRunning = false;

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move += forward * speed;
            if (speed == 0.01f) isRunning = true;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey("down"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move -= forward * speed;
            if (speed == 0.01f) isRunning = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move -= right * speed;
            if (speed == 0.01f) isRunning = true;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))
        {
            isWalking = true;
            float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 0.01f : 0.005f;
            move += right * speed;
            if (speed == 0.01f) isRunning = true;
        }

        transform.position += move;

        Vector3 moveDir = new Vector3(move.x, 0, move.z);
        if (moveDir.magnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
    }


}
