using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // ’Ç]‘ÎÛƒLƒƒƒ‰
    private Vector3 offset;    // ’Ç]‚Ì‹——£EˆÊ’u‚Ì·

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // ˆÊ’u‚¾‚¯’Ç]B‰ñ“]‚Í•Ï‚¦‚È‚¢
        transform.position = player.position + offset;
    }
}