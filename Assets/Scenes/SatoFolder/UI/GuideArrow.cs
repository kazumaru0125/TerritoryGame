using UnityEngine;

public class GuideArrow : MonoBehaviour
    {
    void Update()
        {
        // 常にプレイヤー側から見やすい角度
        transform.Rotate(Vector3.up * Time.deltaTime * 30f, Space.World);
        }
    }
