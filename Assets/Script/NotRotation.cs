using UnityEngine;

public class NotRotation : MonoBehaviour
    {
    private Quaternion fixedRotation;

    void Start()
        {
        // ゲーム開始時の回転を記録（この回転を維持する）
        fixedRotation = transform.rotation;
        }

    void LateUpdate()
        {
        // 毎フレーム、親の回転の影響を打ち消して固定
        transform.rotation = fixedRotation;
        }
    }
