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

    private bool isRun = false; // クラス変数として宣言（Update外）

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

        // トグル切替するためにボタン押された瞬間だけ判定
        if (Input.GetKeyDown("joystick button 8"))
        {
            isRun = !isRun;
        }

        // Aボタン押下判定（joystick button 0）
        bool isDash = Input.GetKey("joystick button 0");

        if (move.magnitude > 0.05f)
        {
            isWalking = true;
<<<<<<< HEAD
            float speed = (isDash || isRun) ? 0.02f : 0.01f; // ダッシュ時は速度2倍
=======
            float speed = isDash ? 0.075f : 0.05f; // ダッシュ時は速度2倍
>>>>>>> NewTest
            move = move.normalized * speed;
            transform.position += move;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            if (isDash || isRun) isRunning = true;
        }

        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
    }
}
