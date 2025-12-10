using UnityEngine;

public class CCamera : MonoBehaviour
{
    public float moveSpeed = 5f;  // testOBJ と同じ速さにしておく
    private float fixedY;         // カメラの高さを固定して保存する

    void Start()
    {
        // カメラの現在の高さ（Y座標）を記録しておく
        fixedY = transform.position.y;
    }

    //void Update()  // プレイヤーと同じタイミングで動かす
    //{
    //    // 入力されたキーから移動量を取得（testOBJと同じ処理）
    //    float moveX = Input.GetAxis("Horizontal");  // ← →
    //    float moveZ = Input.GetAxis("Vertical");    // ↑ ↓

    //    // 入力に速さと時間をかけて移動ベクトルを作成
    //    Vector3 move = new Vector3(moveX, 0.0f, moveZ) * moveSpeed * Time.deltaTime;

    //    // カメラの位置をプレイヤーと同じ量だけ移動させる
    //    transform.position += new Vector3(move.x, 0.0f, move.z);

    //    // 高さ（Y）は固定しておく
    //    transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    //}
}