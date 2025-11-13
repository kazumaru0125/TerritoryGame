using UnityEngine;
using UnityEngine.Rendering;

public class ToggleVolumes : MonoBehaviour
{
    // VolumeコンポーネントをInspectorで指定できるようにする
    public Volume volume;

    // 最初に一度だけ呼ばれる
    void Start()
    {
        // Volumeが設定されていない場合、自動でこのGameObjectから取得
        if (volume == null)
        {
            volume = GetComponent<Volume>(); // 同じオブジェクトにあるVolumeを探す
        }

        // Volumeが無い場合は警告を出す
        if (volume == null)
        {
            Debug.LogWarning("Volumeが設定されていません。インスペクターで指定してください。");
        }
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // スペースキーが押された瞬間を検出
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Volumeが存在すれば有効／無効を切り替える
            if (volume != null)
            {
                volume.enabled = !volume.enabled; // ONとOFFを反転
            }
        }
    }
}
