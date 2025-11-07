using UnityEngine;

public class MoveCamera : MonoBehaviour
    {
    private float fixedY;
    private Vector3 offset = new Vector3(0, 10f, 0); // カメラの位置調整
    private GameObject currentTarget;

    void Start()
        {
        fixedY = transform.position.y;
        }

    void Update()
        {
        // 毎フレーム、有効な MiniCameraChildScript を探す
        MiniCameraChildScript[] allChildren = FindObjectsOfType<MiniCameraChildScript>();

        foreach (var child in allChildren)
            {
            if (child.IsActive && child.enabled)
                {
                currentTarget = child.gameObject;
                break;
                }
            }

        if (currentTarget == null) return;

        // 追従対象の位置
        Vector3 targetPos = currentTarget.transform.position + offset;

        // 高さ固定 or なめらか追従
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, 0.1f);
        transform.position = new Vector3(smoothPos.x, fixedY, smoothPos.z);
        }
    }
