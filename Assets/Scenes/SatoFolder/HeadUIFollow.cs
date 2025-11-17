using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameDisplay : MonoBehaviourPun
    {
    [SerializeField] private TMP_Text nameText;

    private Camera mainCamera;

    private void Start()
        {
        mainCamera = Camera.main;

        // PhotonView‚ÌOwnerî•ñ‚©‚ç–¼‘O‚ðŽæ“¾
        if (photonView.Owner != null)
            {
            nameText.text = photonView.Owner.NickName;
            }
        }

    private void LateUpdate()
        {
        if (mainCamera != null)
            {
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
