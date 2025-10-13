using UnityEngine;

public class PlayerRespawnScript : MonoBehaviour
    {
    void Update()
        {
        // 「L」キーを押したら実行
        if (Input.GetKeyDown(KeyCode.L))
            {
            RespawnAtRandomSpawnArea();
            }
        }

    void RespawnAtRandomSpawnArea()
        {
        // Tagが "SpawnArea" のオブジェクトをすべて取得
        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag("SpawnArea");

        if (spawnAreas.Length == 0)
            {
            Debug.LogWarning("SpawnAreaタグのオブジェクトが見つかりません。");
            return;
            }

        // ランダムに1つ選ぶ
        GameObject randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // 選んだSpawnAreaの位置を取得
        Vector3 areaPos = randomArea.transform.position;

        // 例えばSpawnAreaの少し上に移動（y座標を少し上げる）
        Vector3 newPosition = new Vector3(areaPos.x, areaPos.y + 1.0f, areaPos.z);

        // このスクリプトがついているオブジェクトを移動
        transform.position = newPosition;

        Debug.Log($"{gameObject.name} が {randomArea.name} の上に移動しました。");
        }
    }
