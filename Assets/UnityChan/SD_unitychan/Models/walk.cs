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
        // 何も処理しなくてOK
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

        // Aボタン押下判定（joystick button 0はXboxのAボタン）
        bool isDash = Input.GetKey("joystick button 0");

        if (move.magnitude > 0.05f)
        {
            isWalking = true;
            float speed = isDash ? 0.075f : 0.05f; // ダッシュ時は速度2倍
            move = move.normalized * speed;
            transform.position += move;

            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            if (isDash) isRunning = true;
        }

        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
    }

}
