using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
    {
    [SerializeField]
    private float time = 60f; // 秒単位で初期値を30秒に

    public TMP_Text Count;
    public TMP_Text Wave;



    void Update()
        {

        if (time > 0)
            {
            time -= Time.deltaTime; // フレームに依存せず減少
            if (time <= 0)
                {
                time = 0; // 0秒で止める
                Wave.text = "Wave2";
                Debug.Log("終了");
                }
            else
                {
                Wave.text = "Wave1";
                }
            }

        // UI に表示（整数秒）
        if (Count != null)
            {
            Count.text = Mathf.CeilToInt(time).ToString();
            }
        }
    }
