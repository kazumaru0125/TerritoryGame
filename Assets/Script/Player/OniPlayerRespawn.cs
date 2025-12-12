using UnityEngine;
using Photon.Pun;

public class OniPlayerRespawn : MonoBehaviourPunCallbacks
    {
    public void RespawnAtRandomSpawnArea()
        {
        //if (!photonView.IsMine) return; // 自分のプレイヤーのみ実行

        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag("OniSpawnArea");
        if (spawnAreas.Length == 0)
            {
            Debug.LogWarning("OniSpawnAreaタグのオブジェクトが見つかりません。");
            return;
            }

        GameObject randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector3 areaPos = randomArea.transform.position;
        Vector3 newPosition = new Vector3(areaPos.x, areaPos.y + 1.0f, areaPos.z);

        // 自分の位置を直接変更（←自分は自分で動かす）
        transform.position = newPosition;

        // ★ 全員にこの座標を通知
        photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, newPosition);

        Debug.Log($"[自分側] {gameObject.name} が {randomArea.name} の上に移動しました。");
        }

    // 他プレイヤーが見る位置を更新
    [PunRPC]
    void RPC_SetRespawnPosition(Vector3 newPosition)
        {
        transform.position = newPosition;
        Debug.Log($"[他クライアント] {gameObject.name} がリスポーンしました。");
        }
    }
