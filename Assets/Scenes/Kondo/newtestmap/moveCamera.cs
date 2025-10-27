using UnityEngine;

public class moveCamera : MonoBehaviour
{
    private Transform player;  // 自動で探す用
    private float fixedY;      // カメラの高さを固定して保持

    void Start()
    {
        fixedY = transform.position.y;  // 初期Y座標を記録
        // シーン上にある "testOBJ(Clone)" を探してTransformを取得するの
        GameObject obj = GameObject.Find("testOBJ(Clone)");
        if (obj != null)
        {
            player = obj.transform;
        }
    }

    //void LateUpdate()
    //{
    //    if (player == null) return;

    //    Vector3 playerPos = player.position;
    //    transform.position = new Vector3(playerPos.x, fixedY, playerPos.z);
    //}
}