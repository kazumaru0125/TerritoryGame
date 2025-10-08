using UnityEngine;

public class WallPenetrationFix : MonoBehaviour
{
    // FixedUpdateは物理演算タイミングで呼ばれるのでここで処理する
    void FixedUpdate()
    {
        // 自分のアタッチしているColliderを取得
        Collider myCollider = GetComponent<Collider>();

        // 自分のColliderの範囲内（OverlapBoxの中心と半分の大きさ）にあるすべてのColliderを取得
        // BoundsはColliderのバウンディングボックス。少しだけ内側に縮めてOverlapBoxを作る形にしている
        Collider[] overlaps = Physics.OverlapBox(myCollider.bounds.center, myCollider.bounds.extents * 0.95f);

        // 取得したColliderの中から自分自身とTriggerではないものを探して処理
        foreach (Collider col in overlaps)
        {
            // 自分自身のColliderは除外し、Trigger状態のColliderは物理衝突無視でパス
            if (col != myCollider && !col.isTrigger)
            {
                // 押し出す方向のベクトル受け取り用
                Vector3 direction;
                // 押し出す距離受け取り用
                float distance;    

                // ComputePenetrationは二つのColliderのめり込み情報を計算する
                // 自分のColliderと位置・回転
                // 相手のColliderと位置・回転
                // もしCollider同士がめり込んでいたらtrueを返し、押し出し方向と距離を取得できる
                bool penetrated = Physics.ComputePenetration(
                    myCollider, transform.position, transform.rotation,
                    col, col.transform.position, col.transform.rotation,
                    out direction, out distance);

                // めり込んでいた場合は、めり込み方向に距離分だけ自分の位置を押し戻す
                if (penetrated)
                {
                    // キャラクターやオブジェクトの位置を少しだけ動かしめり込みを解消する
                    transform.position += direction * distance;
                }
            }
        }
    }
}
