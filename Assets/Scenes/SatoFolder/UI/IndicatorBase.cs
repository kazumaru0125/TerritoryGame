using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class IndicatorBase : MonoBehaviour
    {
    [SerializeField] private Transform player;         // 自分の Transform
    [SerializeField] private RectTransform indicator;  // UI 矢印
    [SerializeField] private float closeDistance = 5f; // 距離で色変化

    private Transform currentTarget;
    private Transform cameraTransform;
    private Image indicatorImage;

    void Start()
        {
        // MainCamera を取得
        Camera mainCam = Camera.main;
        if (mainCam != null)
            cameraTransform = mainCam.transform;
        else
            Debug.LogWarning("[Indicator] MainCamera が見つかりません！");

        // Image の取得
        indicatorImage = indicator.GetComponent<Image>();
        if (indicatorImage == null)
            Debug.LogWarning("[Indicator] Indicator に Image コンポーネントがありません！");
        }

    void Update()
        {
        if (cameraTransform == null || player == null || indicator == null) return;

        // Photon 上の全プレイヤー取得
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float minDist = float.MaxValue;
        Transform nearestOni = null;

        foreach (GameObject obj in players)
            {
            TestPlayerRoll role = obj.GetComponent<TestPlayerRoll>();
            if (role == null) continue;

            // 🔥 ここを Team ではなく "Role" で判定
            if (role.CurrentRole != "Oni") continue;

            // 自分自身は除外
            if (obj.transform == player) continue;

            float dist = Vector3.Distance(player.position, obj.transform.position);

            // Debug
            Debug.Log($"[Indicator] found Oni: {obj.name}, dist={dist}");

            if (dist < minDist)
                {
                minDist = dist;
                nearestOni = obj.transform;
                }
            }

        currentTarget = nearestOni;
        if (currentTarget == null)
            {
            Debug.Log("[Indicator] Oniが1人も見つかりません");
            return;
            }

        // プレイヤー → Oni の方向
        Vector3 direction = currentTarget.position - player.position;
        var rot = Quaternion.LookRotation(direction);

        // カメラ方向との差を Z 回転へ
        float angle = Quaternion.Angle(
            Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0),
            rot
        );

        indicator.localEulerAngles = new Vector3(0, 0, angle);

        // 距離で色変化
        if (indicatorImage != null)
            {
            if (minDist <= closeDistance)
                indicatorImage.color = Color.blue;   // 近い
            else
                indicatorImage.color = Color.red;    // 遠い
            }
        }
    }
