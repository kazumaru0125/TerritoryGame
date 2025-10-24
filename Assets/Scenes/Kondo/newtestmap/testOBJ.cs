using UnityEngine;

public class testOBJ : MonoBehaviour
{
    public float moveSpeed = 5f;  // オブジェクトが動く速さを調整できる変数よ

    void Update()  // 毎フレーム（1/60秒ごと）に呼ばれるメソッドなの
    {
        // 入力されたキーから移動量を作るのよ♡
        float moveX = Input.GetAxis("Horizontal");  // 左右の矢印キー（←→）の入力を取得
        float moveZ = Input.GetAxis("Vertical");    // 上下の矢印キー（↑↓）の入力を取得

        // 入力に速さと時間をかけて滑らかに動かすの
        Vector3 move = new Vector3(moveX, 0.0f, moveZ) * moveSpeed * Time.deltaTime;

        // 実際に位置を変えるのよ！
        transform.position += move;
    }
}
