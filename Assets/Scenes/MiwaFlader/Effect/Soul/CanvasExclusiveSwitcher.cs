using UnityEngine;

public class CanvasExclusiveSwitcher : MonoBehaviour
{
    [Header("表示するCanvas A")]
    [SerializeField] private Canvas canvasA;

    [Header("表示するCanvas B")]
    [SerializeField] private Canvas canvasB;

    private bool isCanvasAActive = true; // 現在どちらが表示されているか

    void Start()
    {
        // 初期状態：Canvas Aだけ表示
        ShowCanvasA();
    }

    void Update()
    {
        // Spaceキーで切り替え
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchCanvas();
        }
    }

    // Canvas切り替え
    private void SwitchCanvas()
    {
        if (isCanvasAActive)
        {
            ShowCanvasB();
        }
        else
        {
            ShowCanvasA();
        }
    }

    // Canvas A を表示、B を非表示
    private void ShowCanvasA()
    {
        canvasA.enabled = true;
        canvasB.enabled = false;
        isCanvasAActive = true;
    }

    // Canvas B を表示、A を非表示
    private void ShowCanvasB()
    {
        canvasA.enabled = false;
        canvasB.enabled = true;
        isCanvasAActive = false;
    }
}
