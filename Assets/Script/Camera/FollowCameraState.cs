using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraState : ICameraState
    {
    private float yaw = 0f; // 水平方向の回転角度
    private float pitch = 10f; // 縦方向の角度（固定）
    private float sensitivity = 2.0f; // マウス感度
    private Vector3 cameraOffset = new Vector3(0, 1.5f, -3.5f); // TPS視点のカメラ位置

    private List<Renderer> transparentObjects = new List<Renderer>(); // 透明にしたオブジェクトを管理
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>(); // 元のマテリアルを保存

    private float minPitch = -10f; // 最小ピッチ（下方向）
    private float maxPitch = 50f;  // 最大ピッチ（上方向）
    private float maxYaw = 10f;    // 最大の水平回転角度
    private float minYaw = -10f;   // 最小の水平回転角度
    
    public void EnterState(PlayerCameraController camera)
        {
        Debug.Log("Entering Follow Camera State");
        }

    public void UpdateState(PlayerCameraController camera)
        {
        if (camera.target != null)
            {
            // マウスの左右視点回転
            yaw += Input.GetAxis("Mouse X") * sensitivity;

            // yawの範囲を制限
            yaw = Mathf.Clamp(yaw, minYaw, maxYaw);

            // マウスの上下視点回転
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;

            // pitchの範囲を制限
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            // カメラ位置を計算（TPS視点）
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 offsetPosition = rotation * cameraOffset;
            Vector3 targetPosition = camera.target.position + offsetPosition;

            // プレイヤーの後ろに追従
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime * camera.smoothSpeed);
            camera.transform.LookAt(camera.target.position + Vector3.up * 1.5f); // 目線の高さに合わせる

            // オブジェクトを半透明にする処理
            HandleTransparency(camera);
            }

  
        }

    public void ExitState(PlayerCameraController camera)
        {
        Debug.Log("Exiting Follow Camera State");
        ResetTransparency(); // 状態を抜けるときに透明度をリセット
        }

    private void HandleTransparency(PlayerCameraController camera)
        {
        // 以前透明にしたオブジェクトを元に戻す
        ResetTransparency();

        Vector3 cameraPos = camera.transform.position;
        Vector3 targetPos = camera.target.position + Vector3.up * 1.5f;
        Vector3 direction = targetPos - cameraPos;
        float distance = Vector3.Distance(cameraPos, targetPos);

        RaycastHit[] hits = Physics.RaycastAll(cameraPos, direction.normalized, distance);
        foreach (RaycastHit hit in hits)
            {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && !originalMaterials.ContainsKey(renderer))
                {
                // 元のマテリアルを保存
                originalMaterials[renderer] = renderer.materials;

                // 半透明のマテリアルを作成
                Material[] newMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < renderer.materials.Length; i++)
                    {
                    Material newMat = new Material(renderer.materials[i]);
                    newMat.SetFloat("_Mode", 2); // 透明モード
                    newMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    newMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    newMat.SetInt("_ZWrite", 0);
                    newMat.DisableKeyword("_ALPHATEST_ON");
                    newMat.EnableKeyword("_ALPHABLEND_ON");
                    newMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    newMat.color = new Color(newMat.color.r, newMat.color.g, newMat.color.b, 0.3f); // 透明度 30%
                    newMaterials[i] = newMat;
                    }
                renderer.materials = newMaterials;
                transparentObjects.Add(renderer);
                }
            }
        }

    private void ResetTransparency()
        {
        foreach (Renderer renderer in transparentObjects)
            {
            if (renderer != null && originalMaterials.ContainsKey(renderer))
                {
                renderer.materials = originalMaterials[renderer]; // 元のマテリアルに戻す
                }
            }
        transparentObjects.Clear();
        originalMaterials.Clear();
        }
    }
