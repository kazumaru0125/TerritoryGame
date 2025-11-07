using UnityEngine;

public class MiniCameraChildScript : MonoBehaviour
{
    [SerializeField]
    private bool isActive = false;

    // 他のスクリプトがこれを参照して判断できるように
    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }

    // デバッグ用に有効化・無効化を確認
    private void OnEnable()
    {
        Debug.Log($"{gameObject.name} の MiniCameraChildScript が有効になりました。");
    }

    private void OnDisable()
    {
        Debug.Log($"{gameObject.name} の MiniCameraChildScript が無効になりました。");
    }
}
