#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;

// Unityエディタ専用のクラス。
// 再生終了時に TextMeshPro の Dynamic フォントアセットのアトラスを自動でクリアして
// Git で毎回差分が出るのを防ぐための補助スクリプト。
public static class TextMeshProAtlasClear
{
    // エディタのドメインリロード完了時に自動で呼ばれる初期化メソッド。
    // ここで「再生状態が変化したときに呼ばれるイベント」にハンドラを登録する。
    [InitializeOnLoadMethod]
    private static void Test()
    {
        // 二重登録を防ぐために一度必ず解除してから…
        EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        // 再生状態が変わるたびに PlayModeStateChanged が呼ばれるよう登録する。
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }

    // エディタの再生状態が変化したときに呼ばれるコールバック。
    private static void PlayModeStateChanged(PlayModeStateChange state)
    {
        // 「再生終了時（Play → Stop）」以外のタイミングでは何もしない。
        if (state != PlayModeStateChange.ExitingPlayMode)
            return;

        // プロジェクト内に存在するすべての TMP_FontAsset を取得する。
        // Resources.Load ではなく、シーンにないアセットも含めて検索するメソッド。
        var assets = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();

        // 取得したフォントアセットを順にチェックしていく。
        foreach (var asset in assets)
        {
            // Dynamic ではない（Static などの）フォントアセットは対象外。
            if (asset.atlasPopulationMode != AtlasPopulationMode.Dynamic)
                continue;

            // Dynamic フォントアセットのデータ（アトラス画像など）をクリアする。
            // setAtlasSizeToZero: true にすることでアトラス自体もリセットされる。
            asset.ClearFontAssetData(setAtlasSizeToZero: true);
        }
    }
}
#endif
