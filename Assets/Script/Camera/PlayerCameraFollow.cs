using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
    {
    private Transform target;

    public Vector3 offset2 = new Vector3(0, 2, -5);
    public float smoothSpeed = 10f;

    void LateUpdate()
        {
        if (target == null) return;

        // カメラ位置を計算
        Vector3 desiredPos = target.position + offset2;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        // 目線の高さに合わせて追従
        transform.LookAt(target.position + Vector3.up * 1.5f);
        }

    // カメラの追従対象をセットする
    public void SetTarget(Transform newTarget)
        {
        target = newTarget;
        }
    }