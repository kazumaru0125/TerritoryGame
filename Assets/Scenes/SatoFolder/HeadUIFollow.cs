using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
    {
    [SerializeField] private TMP_Text nameText;

    //[Header("頭の真上に置くオフセット")]
    // Vector3 offset = new Vector3(0, 2.0f, 0);

    private Transform target;     // 追従するプレイヤー本体
    private Camera mainCamera;

    private void Start()
        {
        mainCamera = Camera.main;

        // 親のプレイヤーをターゲットにする
        target = transform.parent;

        // Photonから名前取得
        if (photonView.Owner != null)
            nameText.text = photonView.Owner.NickName;
        }

    //private void LateUpdate()
    //    {
    //    if (mainCamera == null || target == null) return;

    //    // --- ① 位置をキャラの頭の真上に固定 ---
    //    transform.position = target.position + offset;

    //    // --- ② カメラの方向を向かせる（反転なし） ---
    //    Vector3 lookDir = mainCamera.transform.position - transform.position;
    //    lookDir.y = 0;

    //    if (lookDir.sqrMagnitude > 0.001f)
    //        transform.rotation = Quaternion.LookRotation(lookDir);
    //    }
    }
