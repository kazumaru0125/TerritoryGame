using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;         // 追従するキャラクター
    public float mouseSensitivity = 2.0f;
    public float stickSensitivity = 1.0f;
    private Vector3 initialRotation;
    private float cameraPitch = 0.0f;
    private float cameraYaw = 0.0f;

    void Start()
    {
        // 最初のローカル回転を保存（X=ピッチ、Y=Yaw、Z=ロール）
        initialRotation = transform.localEulerAngles;
        cameraPitch = initialRotation.x;
        cameraYaw = initialRotation.y;
    }

    void LateUpdate()
    {
        // Rキー or 右スティック押し込みでリセット
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown("joystick button 9"))
        {
            cameraPitch = initialRotation.x;
            cameraYaw = initialRotation.y;
        }

        // マウス入力取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Xbox右スティック入力取得（Horizontal2, Vertical2はInput Manager設定で右スティックの軸）
        float stickX = Input.GetAxis("Horizontal2") * stickSensitivity;
        float stickY = Input.GetAxis("Vertical2") * stickSensitivity;

        // 入力を合算
        float finalX = mouseX + stickX;
        float finalY = mouseY + stickY;

        // ピッチ上下制限
        cameraPitch -= finalY;
        cameraPitch = Mathf.Clamp(cameraPitch, -40f, 60f);

        // Yaw（左右回転）を加算
        cameraYaw += finalX;

        // 回転を適用（Yaw＋Pitch、ロールは0）
        transform.localEulerAngles = new Vector3(cameraPitch, cameraYaw, 0);

        // キャラクター本体の回転は左右回転のみ（Yaw）
        target.localEulerAngles = new Vector3(0, cameraYaw, 0);
    }
}
