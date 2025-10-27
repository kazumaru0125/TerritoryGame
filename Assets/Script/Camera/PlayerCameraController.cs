using UnityEngine;
using Photon.Pun;

public class PlayerCameraController : MonoBehaviourPun
    {
    void Start()
        {
        if (photonView.IsMine)
            {
            // 自分のプレイヤーをカメラに登録
            Camera.main.GetComponent<PlayerCameraFollow>().SetTarget(transform);
            }
        }
    private void OnEnable()
        {
        // 自分のキャラクターがアクティブ化されたらカメラを登録
        if (photonView.IsMine)
            {
            Debug.Log($"[PlayerCameraController] {gameObject.name} がアクティブになりました。");
            CameraManager.TryRegisterCamera(this);
            }
        }
    }
