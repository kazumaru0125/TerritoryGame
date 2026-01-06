using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownUI : MonoBehaviour
    {
    [SerializeField] private RawImage[] countdownImages;
    [SerializeField] private float interval = 1f;

    public System.Action OnCountdownFinished;

    private void Awake()
        {
        foreach (var img in countdownImages)
            img.gameObject.SetActive(false);
        }

    public void Play()
        {
        StartCoroutine(CountdownRoutine());
        }

    private IEnumerator CountdownRoutine()
        {
        for (int i = 0; i < countdownImages.Length; i++)
            {
            countdownImages[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(interval);
            countdownImages[i].gameObject.SetActive(false);
            }

        // 3,2,1,GO ‘S•\Ž¦Š®—¹
        OnCountdownFinished?.Invoke();
        }
    }
