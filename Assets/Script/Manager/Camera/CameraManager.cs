using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviourPun
    {
    private PlayerCameraFollow cameraFollow;

    private void Start()
        {
        // Camera.main が null の可能性があるので安全に取得
        var mainCam = Camera.main;
        if (mainCam != null)
            {
            cameraFollow = mainCam.GetComponent<PlayerCameraFollow>();
            if (cameraFollow == null)
                Debug.LogError("PlayerCameraFollow が Camera にアタッチされていません。");
            }
        else
            {
            Debug.LogError("MainCamera が見つかりません。");
            }
        }

    // プレイヤーを切り替えた時に呼ぶ
    public void SetFollowTarget(Transform newTarget)
        {
        if (cameraFollow != null)
            {
            cameraFollow.SetTarget(newTarget);
            }
        }

    // プレイヤー破棄時にカメラ追従をクリア
    public void ClearFollowTarget()
        {
        if (cameraFollow != null)
            {
            cameraFollow.SetTarget(null);
            }
        }

    // 自分の操作しているプレイヤーがアクティブになった時に呼ばれる
    public static void TryRegisterCamera(PlayerCameraController player)
        {
        // player と photonView が null でないかチェック
        if (player != null && player.photonView != null && player.photonView.IsMine)
            {
            var mainCam = Camera.main;
            if (mainCam != null)
                {
                var manager = mainCam.GetComponent<CameraManager>();
                if (manager != null)
                    manager.SetFollowTarget(player.transform);
                }
            }
        }
    }
