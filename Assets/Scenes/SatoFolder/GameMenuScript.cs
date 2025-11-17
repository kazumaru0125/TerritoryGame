using UnityEngine;

public class GameMenuScript : MonoBehaviour
    {
    [SerializeField] GameObject MAP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
        {
        MAP.SetActive(false);
        }

    // Update is called once per frame
    void Update()
        {

        // RL軸の入力を取得
        float rl = Input.GetAxis("RL");

        // RL軸が押されている間 true、押されていなければ false
        bool isRLPressed = Mathf.Abs(rl) > 0.1f; // 小さな誤差を吸収

        // RLを押しているときMAPを表示、押していないとき非表示
        MAP.SetActive(isRLPressed);

        // キーボードH/Jで個別操作も可能
        if (Input.GetKey(KeyCode.H))
            {
            MAP.SetActive(true);
            }
        if (Input.GetKey(KeyCode.J))
            {
            MAP.SetActive(false);
            }

        }
    }
