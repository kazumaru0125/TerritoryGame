using UnityEngine;

public class ItemBoxScript : MonoBehaviour
    {
    private Animation anim;

    [Header("Particles")]
    public GameObject idleParticle;   // 閉じ状態
    public GameObject getParticle;    // 開いた状態

    void Start()
        {
        anim = GetComponent<Animation>();

        // 最初は閉じた状態に固定
        SetToClosedPose();
        }

    void Update()
        {
        // R キーで開くテスト
        if (Input.GetKeyDown(KeyCode.R))
            {
            OpenSequence();
            }
        }

    private void OnTriggerStay(Collider other)
        {
        if (other.CompareTag("Player"))
            {
            if (Input.GetKey("joystick button 5"))
                {
                OpenSequence();
                }
            }
        }



    // ---------------------------------------------------------------
    // ★ "開く → 10秒固定 → 閉じる" の一連の流れ
    // ---------------------------------------------------------------
    public void OpenSequence()
        {
        //CancelInvoke();  // リセット
        OpenAnimation(); // まず開くアニメを再生

        // 開くアニメ終了後に 10秒固定 → 閉じる
        float openDuration = anim.clip.length;
        Invoke(nameof(StartOpenHold), openDuration);
        }

    // ---------------------------------------------------------------
    // ★ 開いた状態で 10秒固定
    // ---------------------------------------------------------------
    void StartOpenHold()
        {
        SetToOpenPose(); // 開きポーズに固定

        // 10秒後に閉じ状態へ戻す
        Invoke(nameof(SetToClosedPose), 10f);
        }

    // ---------------------------------------------------------------
    // ★ 開くアニメーションを 0 → 100% 再生
    // ---------------------------------------------------------------
    void OpenAnimation()
        {
        AnimationState state = anim[anim.clip.name];

        if (idleParticle) idleParticle.SetActive(false);
        if (getParticle) getParticle.SetActive(true);

        state.speed = 1f / state.length;
        state.normalizedTime = 0f;
        anim.Play();

        Debug.Log("開くアニメを再生");
        }

    // ---------------------------------------------------------------
    // ★ 開き状態で固定（100%）
    // ---------------------------------------------------------------
    void SetToOpenPose()
        {
        AnimationState state = anim[anim.clip.name];

        state.speed = 0f;
        state.normalizedTime = 1f;
        anim.Play();
        anim.Sample();
        anim.Stop();

        if (idleParticle) idleParticle.SetActive(false);
        if (getParticle) getParticle.SetActive(true);

        Debug.Log("開き状態で固定（10秒）");
        }

    // ---------------------------------------------------------------
    // ★ 閉じ状態で固定（1秒地点）
    // ---------------------------------------------------------------
    void SetToClosedPose()
        {
        AnimationState state = anim[anim.clip.name];

        float t = 1f / state.length; // 1秒地点の normalizedTime
        state.speed = 0f;
        state.normalizedTime = t;
        anim.Play();
        anim.Sample();
        anim.Stop();

        if (idleParticle) idleParticle.SetActive(true);
        if (getParticle) getParticle.SetActive(false);

        Debug.Log("閉じ状態へ戻す");
        }
    }
