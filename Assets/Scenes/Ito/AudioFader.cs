using UnityEngine;
using System;
using System.Collections;

public class AudioFader : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 2.0f;

    public void FadeOut(Action onComplete)
    {
        StartCoroutine(FadeOutCoroutine(onComplete));
    }

    private IEnumerator FadeOutCoroutine(Action onComplete)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
        onComplete?.Invoke();
    }
}
