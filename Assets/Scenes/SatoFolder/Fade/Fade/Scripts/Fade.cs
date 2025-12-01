using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
    {
    IFade fade;
    float cutoutRange;

    void Awake()
        {
        Init();
        // 開始時は真っ暗にする
        cutoutRange = 1f;
        fade.Range = cutoutRange;
        }

    void Start()
        {
        // シーン開始時にフェードイン
        FadeOut(1.0f);
        }


    void Update()
        {
        // スペースキーが押されたらフェードアウト
        if (Input.GetKeyDown(KeyCode.Space))
            {
            //FadeOut(1.0f, () =>
            //{
            //    Debug.Log("フェードアウト完了！");
            //    // ここにシーン遷移などを追加できる
            //    // SceneManager.LoadScene("NextScene");
            //});
            }

        if (Input.GetKeyDown(KeyCode.Z))
            {
        
            //FadeIn(1.0f, () =>
            //{
            //    Debug.Log("フェードイン完了！");
            //    // ここにシーン遷移などを追加できる
            //    // SceneManager.LoadScene("NextScene");
            //});
            }
        }

    public void CloseFade()
        {
        FadeIn(1.0f, () =>
        {
            Debug.Log("フェードイン完了！");
        });
        }

    void Init()
        {
        fade = GetComponent<IFade>();
        }

    void OnValidate()
        {
        Init();
        if (fade != null) fade.Range = cutoutRange;
        }

    IEnumerator FadeoutCoroutine(float time, System.Action action)
        {
        float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);
        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
            {
            cutoutRange = (endTime - Time.timeSinceLevelLoad) / time;
            fade.Range = cutoutRange;
            yield return endFrame;
            }
        cutoutRange = 0;
        fade.Range = cutoutRange;

        action?.Invoke();
        }

    IEnumerator FadeinCoroutine(float time, System.Action action)
        {
        float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);
        var endFrame = new WaitForEndOfFrame();

        while (Time.timeSinceLevelLoad <= endTime)
            {
            cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad) / time);
            fade.Range = cutoutRange;
            yield return endFrame;
            }
        cutoutRange = 1;
        fade.Range = cutoutRange;

        action?.Invoke();
        }

    public Coroutine FadeOut(float time, System.Action action = null)
        {
        StopAllCoroutines();
        return StartCoroutine(FadeoutCoroutine(time, action));
        }

    public Coroutine FadeIn(float time, System.Action action = null)
        {
        StopAllCoroutines();
        return StartCoroutine(FadeinCoroutine(time, action));
        }
    }
