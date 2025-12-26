using UnityEngine;
using UnityEngine.UI;

public class HumanSoulAnimationSequence : MonoBehaviour
{
    [Header("人魂のAnimatorを順番に登録")]
    [SerializeField] private Animator[] soulAnimators;

    [Header("人魂のRawImageを順番に登録（Animatorと同じ順）")]
    [SerializeField] private RawImage[] soulImages;

    [Header("優先度を操作するCanvas")]
    [SerializeField] private Canvas targetCanvas;

    [Header("下げる優先度")]
    [SerializeField] private int loweredSortingOrder = 0;
    [Header("上げる優先度")]
    [SerializeField] private int raisedSortingOrder = 100;

    private int currentIndex = 0;          // 次に再生する人魂
    private int finishedCount = 0;         // 終了したアニメ数
    private int originalSortingOrder;      // 元の優先度

    void Start()
    {
        // Canvasの元の優先度を保存
        if (targetCanvas != null)
        {
            originalSortingOrder = targetCanvas.sortingOrder;
        }

        // すべて初期状態に戻す
        ResetAllSouls();
    }

    void Update()
    {
        // Spaceキーで次の人魂を再生
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayNextSoul();
        }
    }

    // 次の人魂を1つ再生
    private void PlayNextSoul()
    {
        // 最初の1回だけ優先度を上げる
        if (currentIndex == 0)
        {
            RaiseCanvasPriority();
        }
        if (currentIndex >= soulAnimators.Length)
        {
            return;
        }

        Animator anim = soulAnimators[currentIndex];

        if (anim != null)
        {
            anim.enabled = true;
            anim.Play("HumanSoul", 0, 0f);
        }

        currentIndex++;
    }

    // ★ AnimationEvent から呼ばれる
    public void OnHumanSoulAnimationFinished()
    {
        finishedCount++;

        // すべての人魂アニメが終わったら
        if (finishedCount >= soulAnimators.Length)
        {
            RestoreToOriginalState();
        }
    }

    // ============================
    // 元の状態に戻す処理
    // ============================
    private void RestoreToOriginalState()
    {
        // Canvasの優先度を一度下げて戻す
        if (targetCanvas != null)
        {
            targetCanvas.sortingOrder = loweredSortingOrder;
            targetCanvas.sortingOrder = originalSortingOrder;
        }

        // 全人魂を初期状態に戻す
        ResetAllSouls();
    }
    private void RaiseCanvasPriority()
    {
        // Canvasが設定されていれば優先度を上げる
        if (targetCanvas != null)
        {
            targetCanvas.sortingOrder = raisedSortingOrder;
        }
    }
    public void StopSequence()
    {
        ResetAllSouls();
    }
    // 全人魂を初期状態に戻す
    private void ResetAllSouls()
    {
        currentIndex = 0;
        finishedCount = 0;

        for (int i = 0; i < soulAnimators.Length; i++)
        {
            // ① Animatorを完全に停止（最重要）
            if (soulAnimators[i] != null)
            {
                soulAnimators[i].enabled = false;
            }

            // ② Alphaを元に戻す
            if (soulImages != null && i < soulImages.Length && soulImages[i] != null)
            {
                Color c = soulImages[i].color;
                c.a = 1f;
                soulImages[i].color = c;
            }
        }
    }
}
