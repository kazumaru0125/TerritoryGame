using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
    {
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private string playerName;

    private Camera mainCamera;

    private void Start()
        {
        mainCamera = Camera.main;

        if (!photonView.IsMine) return;
        // 自分の名前をPhotonから取得して表示
        playerName = Photon.Pun.PhotonNetwork.LocalPlayer.NickName;
        nameText.text = playerName;
        }


    private void LateUpdate()
        {
        if (mainCamera != null)
            {
            // カメラの方向ベクトルを計算
            Vector3 direction = mainCamera.transform.position - transform.position;

            // Y軸のみ反映（上下の傾きを無視）
            direction.y = 0;

            // ゼロベクトル回避
            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }



    }
