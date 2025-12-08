using UnityEngine;

public class RandomSkybox : MonoBehaviour
    {
    [Header("候補となるSkyboxマテリアル")]
    public Material[] skyboxes;

    void Start()
        {
        SetRandomSkybox();
        }

    public void SetRandomSkybox()
        {
        if (skyboxes == null || skyboxes.Length == 0)
            {
            Debug.LogWarning("Skybox が設定されていません");
            return;
            }

        // ランダムに選択
        int index = Random.Range(0, skyboxes.Length);

        // Skybox に適用
        RenderSettings.skybox = skyboxes[index];

        // 反映（重要）
        DynamicGI.UpdateEnvironment();
        }
    }
