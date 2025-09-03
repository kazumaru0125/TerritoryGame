using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;         // 追従するキャラクター
    public float mouseSensitivity = 2.0f;
    float cameraPitch = 0.0f;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ピッチ上下制限
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -40f, 60f);  // 見下ろし/見上げの限界

        // カメラ自体を上下
        transform.localEulerAngles = new Vector3(cameraPitch, 0, 0);

        // キャラクター本体を左右
        target.Rotate(Vector3.up * mouseX);
    }
}
