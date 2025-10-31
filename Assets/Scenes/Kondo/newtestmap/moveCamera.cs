using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    // 🧭 追従の基準になるオブジェクト（例：testOBJ）
    [SerializeField]
    private GameObject referenceObj;

    [SerializeField]
    private GameObject nextitem;

    private float fixedY;      // カメラの高さを固定して保持

    bool changeCamera = true;

    // 🌸 基準からどれだけずらすか
    private Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

    // 🎬 Startは最初の1回だけ呼ばれる
    void Start()
    {
        fixedY = transform.position.y;  // 初期Y座標を記録
        // 安全チェック：参照が設定されていなかったら警告を出すの
        if (referenceObj == null)
        {
            Debug.LogWarning("referenceObj が設定されていません！インスペクタで testOBJ を指定してください♡");
        }
        if (nextitem == null)
        {
            Debug.LogWarning("nextitem が設定されていません！インスペクタで testOBJ を指定してください♡");
        }
    }

    // 🕊 Updateは毎フレーム呼ばれる（プレイヤーの移動に合わせて動く）
    void Update()
    {
        // 🍀 1回だけEnterキーを押した時にカメラを切り替える処理
        if (Input.GetKeyDown(KeyCode.Return)) // ← Returnキー（Enter）が押された瞬間だけ反応する
        {
            changeCamera = !changeCamera; // true ↔ false を切り替え
        }

        if (changeCamera == true)
        {
            // もし参照が設定されていなければ何もしない
            if (referenceObj == null) return;

            // 🎯 基準オブジェクトの現在位置を取得
            Vector3 basePos = referenceObj.transform.position;

            // 🌟 XとZだけ +5 した位置を計算（Yは同じ高さにする）
            Vector3 newPos = new Vector3(basePos.x + offset.x, fixedY, basePos.z + offset.z);

            // 📦 自分自身（このスクリプトを持つオブジェクト）をその位置に移動させる
            transform.position = newPos;
        }
        else
        {
            // もし参照が設定されていなければ何もしない
            if (nextitem == null) return;

            // 🎯 基準オブジェクトの現在位置を取得
            Vector3 basePos = nextitem.transform.position;

            // 🌟 XとZだけ位置を計算（Yは同じ高さにする）
            Vector3 newPos = new Vector3(basePos.x + offset.x, fixedY, basePos.z + offset.z);

            // 📦 自分自身（このスクリプトを持つオブジェクト）をその位置に移動させる
            transform.position = newPos;
        }
    }
}