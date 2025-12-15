using UnityEngine;
using System.Collections;

public class ChangeFade : MonoBehaviour
    {
    public static ChangeFade Instance { get; private set; }

    private IFade fade;
    private float cutoutRange;

    private void Awake()
        {
        if (Instance != null)
            {
            Destroy(gameObject);
            return;
            }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        fade = GetComponent<IFade>();

        // ãNìÆéûÇÕê^Ç¡à√
        cutoutRange = 1f;
        fade.Range = cutoutRange;
        }

    private void Start()
        {
        FadeOut(1.0f);
        }


    public Coroutine FadeOut(float time, System.Action onComplete = null)
        {
        StopAllCoroutines();
        return StartCoroutine(FadeoutCoroutine(time, onComplete));
        }

    public Coroutine FadeIn(float time, System.Action onComplete = null)
        {
        StopAllCoroutines();
        return StartCoroutine(FadeinCoroutine(time, onComplete));
        }

    private IEnumerator FadeoutCoroutine(float time, System.Action action)
        {
        float endTime = Time.timeSinceLevelLoad + time * cutoutRange;
        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
            {
            cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
            fade.Range = cutoutRange;
            yield return endFrame;
            }

        cutoutRange = 0f;
        fade.Range = cutoutRange;
        action?.Invoke();
        }

    private IEnumerator FadeinCoroutine(float time, System.Action action)
        {
        float endTime = Time.timeSinceLevelLoad + time * (1f - cutoutRange);
        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
            {
            cutoutRange = 1f - ((endTime - Time.timeSinceLevelLoad) / time);
            fade.Range = cutoutRange;
            yield return endFrame;
            }

        cutoutRange = 1f;
        fade.Range = cutoutRange;
        action?.Invoke();
        }
    }
