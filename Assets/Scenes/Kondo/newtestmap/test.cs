using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // 🧭 追従の基準になるオブジェクト（例：testOBJ）
    [SerializeField]
    private GameObject referenceObj;

    // 🌸 基準からどれだけずらすか
    private Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

    // 🎬 Startは最初の1回だけ呼ばれる
    void Start()
    {
        // 安全チェック：参照が設定されていなかったら警告を出すの
        if (referenceObj == null)
        {
            Debug.LogWarning("referenceObj が設定されていません！インスペクタで testOBJ を指定してください♡");
        }
       
        if (referenceObj == null)
        {
            referenceObj = GameObject.Find("testOBJ(Clone)");
        }
        
    }

    // 🕊 Updateは毎フレーム呼ばれる（プレイヤーの移動に合わせて動く）
    void Update()
    {
        // もし参照が設定されていなければ何もしない
        if (referenceObj == null) return;

        // 🎯 基準オブジェクトの現在位置を取得
        Vector3 basePos = referenceObj.transform.position;

        // 🌟 XとZだけ +5 した位置を計算（Yは同じ高さにする）
        Vector3 newPos = new Vector3(basePos.x + offset.x, basePos.y, basePos.z + offset.z);

        // 📦 自分自身（このスクリプトを持つオブジェクト）をその位置に移動させる
        transform.position = newPos;
    }
}