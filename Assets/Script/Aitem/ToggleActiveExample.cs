using UnityEngine;
using System.Collections;

public class ToggleActiveExample : MonoBehaviour
    {
    public GameObject targetObject;

    void Start()
        {
        targetObject.SetActive(false);
        }

    void Update()
        {
        if (Input.GetKeyDown(KeyCode.Space))
            {
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive);

            // True（アクティブ）になった場合のみ2秒後にFalseに戻す
            if (!isActive)
                {
                StartCoroutine(DeactivateAfterDelay(1.5f)); // 2秒後に非アクティブ
                }
            }
        }

    IEnumerator DeactivateAfterDelay(float delay)
        {
        yield return new WaitForSeconds(delay);
        targetObject.SetActive(false);
        }
    }
