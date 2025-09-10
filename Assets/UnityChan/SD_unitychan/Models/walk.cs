using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
{
    private Animator animator;
    private bool isRun = false;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = forward * z + right * x;

        if (Input.GetKeyDown("joystick button 8"))
        {
            isRun = !isRun;
        }
        bool isDash = Input.GetKey("joystick button 0");

        float speed = (isDash || isRun) ? 0.075f : 0.05f;
        Vector3 moveDir = Vector3.zero;

        if (move.magnitude > 0.05f)
        {
            isWalking = true;
            moveDir = move.normalized * speed;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            if (isDash || isRun) isRunning = true;
        }
        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);

        // 移動ベクトルをFixedUpdateへ渡すためのフィールドを用意
        latestMove = moveDir;
    }

    private Vector3 latestMove = Vector3.zero;

    void FixedUpdate()
    {
        // Rigidbody.MovePositionによる物理移動
        if (latestMove.magnitude > 0)
        {
            rb.MovePosition(rb.position + latestMove);
        }
    }
}
