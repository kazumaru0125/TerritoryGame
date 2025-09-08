using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour
    {
    private Animator animator;
    private bool isRun = false; // トグル走り用フラグ

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

        // カメラ基準の方向ベクトル
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 入力取得
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = forward * z + right * x;

        // トグル切替（LBボタン: joystick button 8）
        if (Input.GetKeyDown("joystick button 8"))
            {
            isRun = !isRun;
            }

        // ダッシュ判定（Aボタン: joystick button 0）
        bool isDash = Input.GetKey("joystick button 0");

        if (move.magnitude > 0.05f)
            {
            isWalking = true;

            // 速度設定（ダッシュ or トグル走りなら速く）
            float speed = (isDash || isRun) ? 0.075f : 0.05f;
            move = move.normalized * speed;

            // 移動と回転
            transform.position += move;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            if (isDash || isRun) isRunning = true;
            }

        // アニメーション制御
        animator.SetBool("is_walking", isWalking);
        animator.SetBool("is_running", isRunning);
        }
    }
