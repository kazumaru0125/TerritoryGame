using UnityEngine;
using Photon.Pun;
public class TestMove : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    // 【Animatorコントローラー（キャラのアニメーション制御用）】
    private Animator animator;
    // 【走りトグル用：一度押すと切り替わる変数】
    private bool isRun = false;
    //// 【通常歩き速度】
    //[SerializeField] float walkSpeed ;
    //// 【走り（ダッシュ）速度】
    //[SerializeField] float runSpeed ;

    [SerializeField] Jump jumpScript;



    void Start()
        {
        // Animator取得（キャラにAnimatorコンポーネント必須）
        animator = GetComponent<Animator>();
        }

    public void OnCallChangeFace()
        {
        // 何も処理しなくてOK
        }

    void Update()
        {
        // 【重要】自分のプレイヤー以外は処理しない（Photon同期の基本）
       // if (!photonView.IsMine)
        //    return;

        float stamina = playerStatus.num_playerStamina;
        // アニメーション状態フラグ
        // 歩き中か
        bool isWalking = false;
        // 走り中か
        bool isRunning = false;

        // カメラ基準の移動ベクトル計算
        // カメラ前方向
        Vector3 forward = Camera.main.transform.forward;
        // カメラ右方向
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();


        // 入力を取得（WASDやコントローラーのスティック、InputManagerで設定）
        // 横（A/D, ←/→）
        float x = Input.GetAxis("Horizontal");
        // 縦（W/S, ↑/↓）
        float z = Input.GetAxis("Vertical");

        // 入力から移動方向ベクトルを生成（カメラ向き考慮）
        Vector3 move = forward * z + right * x;

        // 走り「トグル」処理：LeftShiftまたはLB（ボタン8）でON/OFF切り替え
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 8"))
            {
            if(playerStatus.num_playerStamina>0)
                {
                isRun = !isRun;
                }
           
            }

        bool isGrounded = (jumpScript == null) ? true : jumpScript.isGrounded;

        // 一時的なダッシュ：（ボタン0）を押している間
        //　一旦保留で残しておくnum_playerWalkSpeed
        bool isDash = Input.GetKey("joystick button 2") && isGrounded;

        // 実際の速度：ダッシュorトグル走りならrunSpeed、そうでなければwalkSpeed
        float speed = (isDash || isRun) && isGrounded ? playerStatus.num_playerDashSpeed : playerStatus.num_playerWalkSpeed;

        // 入力があれば移動・回転処理
        if (move.magnitude > 0.05f)
            {
            isWalking = true;
            if (isDash || isRun)
                isRunning = true;

            // 方向を正規化し、速度×フレーム時間で移動量を算出
            Vector3 moveDir = move.normalized * speed * Time.deltaTime;
            // 位置を直接加算で移動
            transform.position += moveDir;

            // キャラクターを移動方向にスムーズに向ける
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }

        // アニメーション用パラメータをAnimatorに反映
        // 歩きモーションON/OFF
        animator.SetBool("is_walking", isWalking);
        // 走りモーションON/OFF
        animator.SetBool("is_running", isRunning);
        }
    }
