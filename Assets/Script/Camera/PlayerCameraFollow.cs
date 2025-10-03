using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
    {
    private Transform target;

    public Vector3 offset2 = new Vector3(0, 2, -5);
    public float smoothSpeed = 10f;
    public float stickSensitivity = 3f;

    // カメラ角度管理
    private float yaw = 0f;
    private float pitch = 20f;

    public float minPitch = -20f;
    public float maxPitch = 60f;

    // デフォルト角度
    public float defaultYaw = 0f;
    public float defaultPitch = 20f;

    // カメラ衝突判定用
    public LayerMask collisionMask;

    void LateUpdate()
        {
        if (target == null) return;

        // Xbox右スティック入力
        float stickX = Input.GetAxis("Horizontal2") * stickSensitivity;
        float stickY = Input.GetAxis("Vertical2") * stickSensitivity;

        // カメラ角度更新
        yaw += stickX;
        pitch -= stickY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 押し込みリセット（JoystickButton9 = R3）
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
            {
            yaw = defaultYaw;
            pitch = defaultPitch;
            }

        // 回転をクォータニオンに変換
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // プレイヤーの基準位置（目線より少し上）
        Vector3 targetPos = target.position + Vector3.up * 1.5f;

        // 本来置きたい位置（回転を反映したオフセット）
        Vector3 desiredPos = targetPos + rotation * offset2;

        // Raycastで遮蔽物をチェック
        RaycastHit hit;
        if (Physics.Linecast(targetPos, desiredPos, out hit, collisionMask))
            {
            desiredPos = hit.point + hit.normal * 0.2f;
            }

        // スムーズに移動
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        // プレイヤーを注視
        transform.LookAt(targetPos);
        }

    public void SetTarget(Transform newTarget)
        {
        target = newTarget;
        }
    }
