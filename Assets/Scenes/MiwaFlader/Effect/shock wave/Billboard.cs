using UnityEngine;

public class Billboard : MonoBehaviour
{
    // カメラへの参照を保持
    private Camera mainCamera;

    void Start()
    {
        // メインカメラを取得
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // カメラが存在すれば、その方向を向く
        if (mainCamera != null)
        {
            // オブジェクトが常にカメラの正面を向くように回転
            transform.forward = mainCamera.transform.forward;
        }
    }
}
