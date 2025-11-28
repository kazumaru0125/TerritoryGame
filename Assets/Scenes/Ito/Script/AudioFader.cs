using UnityEngine;
using System;
using System.Collections;

public class AudioFader : MonoBehaviour
{
    // BGMなどの音源を制御するAudioSource
    public AudioSource audioSource;

    // フェードアウトにかける秒数（デフォルト2秒）
    public float fadeDuration = 2.0f;

    // フェードアウト処理を開始し、完了後にはonCompleteを実行
    public void FadeOut(Action onComplete)
    {
        // コルーチンでフェードアウト処理を開始
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    // 実際に音量を減衰させるコルーチン
    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        float startVolume = audioSource.volume; // フェード開始時の音量を記録
        float timer = 0f;                       // 経過時間

        // timerがfadeDurationに達するまで音量を減衰
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration); // 徐々に音量を下げる
            yield return null; // 次のフレームまで待機
        }

        audioSource.volume = 0; // 最終的に音量0
        audioSource.Stop();     // 音源の再生も停止
        onComplete?.Invoke();   // フェードアウト完了時にコールバック実行（例：シーン遷移）
    }
}
