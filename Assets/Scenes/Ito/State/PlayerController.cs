using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviourPun
    {
    public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public float JumpForce = 9f;
    public float RunSpeed = 14.0f;
    public float WalkSpeed = 7.5f;
    public int Stamina = 100;

    public GameObject HetBox;
    public GameObject attackEffectPrefab;
    public GameObject damageEffectPrefab;

    public bool isTrapped;
    public bool IsRun { get; private set; }

    private IPlayerState currentState;
    private bool isAttackTriggered = false; // 攻撃トリガーフラグ

    GameObject currentDamageEffect;

    // スタン時間
    public float StunDuration = 2.0f;
    // スタン状態
    public bool IsStunned { get; private set; }

    public PlayerMoveingState moveState = new PlayerMoveingState();
    public PlayerAttackingState attackState = new PlayerAttackingState();
    public PlayerJumpingState jumpState = new PlayerJumpingState();
    public PlayerTrapDameageState trapDamageState = new PlayerTrapDameageState();

    void Start()
        {
        // Animator = GetComponent<Animator>();
        //  Rigidbody = GetComponent<Rigidbody>();
        ChangeState(moveState);
        HetBox.SetActive(false);
        }

    public void SetStun(bool isOn)
        {
        IsStunned = isOn;
        }

    void Update()
        {
        if (!photonView.IsMine) return;

        // ここでスタン中は一切の入力を受け付けない
        if (IsStunned)
            {
            // 走りトグルや攻撃・ジャンプなども全部スキップ
            currentState?.UpdateState(this); // アニメ進行など必要なら残す
            return;
            }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown("joystick button 8"))
            IsRun = !IsRun;

        // 攻撃トリガー（攻撃中は二重に遷移しない）
        if (!isAttackTriggered && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1")))
            {
            isAttackTriggered = true;
            ChangeState(attackState);
            HetBox.SetActive(true);
            }
        else
            {
            HetBox.SetActive(false);
            }

        // 状態更新
        currentState?.UpdateState(this);

        // 攻撃終了でトリガーフラグを解除
        if (currentState is PlayerMoveingState)
            isAttackTriggered = false;

        // ジャンプトリガーは攻撃フラグが立ってないときのみ許可
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0")) && IsGrounded() && !(currentState is PlayerAttackingState))
            ChangeState(jumpState);
        }

    public void ChangeState(IPlayerState newState)
        {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
        }

    public bool IsGrounded()
        {
        float checkDistance = 0.3f;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, checkDistance);
        }

    public void OnCallChangeFace()
        {
        // 何も処理しなくてOK
        }

    void Awake()
        {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        }

    //private void OnCollisionEnter(Collision collision)
    //    {
    //    // もし触れたオブジェクトのタグが "Player" なら
    //    if (collision.gameObject.CompareTag("Trap"))
    //        {
    //        isTrapped = true; // 捕まった状態
    //        Rigidbody.linearVelocity = Vector3.zero; // 動きを止める
    //        Animator.SetBool("is_walking", false);
    //        Animator.SetBool("is_running", false);
    //        }
    //    }
    private void OnCollisionEnter(Collision collision)
        {
        Debug.Log("Hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Trap"))
            {
            Debug.Log("Trap hit!");
            isTrapped = true;
            Rigidbody.linearVelocity = Vector3.zero;
            Animator.SetBool("is_walking", false);
            Animator.SetBool("is_running", false);

            if (photonView.IsMine)
                {
                ChangeState(trapDamageState);
                }
            }
        }

    // Collider.IsTrigger = true なオブジェクト用
    private void OnTriggerEnter(Collider other)
        {
        Debug.Log("OnTriggerEnter: " + other.name);

        if (other.CompareTag("Trap"))
            {
            Debug.Log("Trap trigger hit!");
            isTrapped = true;
            Rigidbody.linearVelocity = Vector3.zero;
            Animator.SetBool("is_walking", false);
            Animator.SetBool("is_running", false);

            if (photonView.IsMine)
                {
                ChangeState(trapDamageState);
                }
            }
        }


    private void OnCollisionExit(Collision collision)
        {
        if (collision.gameObject.CompareTag("Trap"))
            {
            //   isTrapped = false; // 捕まり解除
            }
        }




    [PunRPC]
    public void RPC_SetAttackState(bool isAttacking)
        {
        Animator.SetBool("is_attacking", isAttacking);
        }

    [PunRPC]
    public void RPC_UpdateMoveAnimation(bool isWalking, bool isRunning)
        {
        Animator.SetBool("is_walking", isWalking && !isRunning);
        Animator.SetBool("is_running", isRunning);
        }

    [PunRPC]
    public void RPC_PlayJumpAnimation()
        {
        Animator.Play("Jump", 0, 0);
        }

    // 攻撃エフェクトの同期
    [PunRPC]
    public void RPC_SpawnAttackEffect()
        {
        if (attackEffectPrefab != null)
            {
            // プレイヤーを親として生成（ローカル座標がそのまま使われる）
            GameObject effect = Instantiate(attackEffectPrefab, transform);

            // Prefab に設定されている localPosition / localRotation を使用
            effect.transform.localPosition = attackEffectPrefab.transform.localPosition;
            effect.transform.localRotation = attackEffectPrefab.transform.localRotation;
            }
        }

    [PunRPC]
    public void PRC_DamageEffect()
        {
        if (damageEffectPrefab != null)
            {
            // プレイヤーを親として生成（ローカル座標がそのまま使われる）
            GameObject effect = Instantiate(damageEffectPrefab, transform);

            // Prefab に設定されている localPosition / localRotation を使用
            effect.transform.localPosition = damageEffectPrefab.transform.localPosition;
            effect.transform.localRotation = damageEffectPrefab.transform.localRotation;
            }
        }

    [PunRPC]
    public void RPC_SetDizzyingState(bool isDizzying)
        {
        Animator.SetBool("is_dizzying", isDizzying);
        }


    [PunRPC]
    public void RPC_DamageEffectOn()
        {
        if (damageEffectPrefab != null && currentDamageEffect == null)
            {
            currentDamageEffect = Instantiate(damageEffectPrefab, transform);
            currentDamageEffect.transform.localPosition = damageEffectPrefab.transform.localPosition;
            currentDamageEffect.transform.localRotation = damageEffectPrefab.transform.localRotation;
            }
        }

    [PunRPC]
    public void RPC_DamageEffectOff()
        {
        if (currentDamageEffect != null)
            Destroy(currentDamageEffect);
        }


    }
