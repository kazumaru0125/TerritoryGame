using UnityEngine;
using Photon.Pun;

public class CharacterChange : MonoBehaviourPun
    {
    [SerializeField] private GameObject[] models; // Inspectorでモデルを登録
    private int index = 0;
    private PlayerCameraFollow cameraFollow;

    //void Start()
    //    {
    //    ShowModel(index); // 最初のモデルを表示

    //    // 自分のプレイヤーならカメラを登録
    //    if (photonView.IsMine)
    //        {
    //        cameraFollow = Camera.main.GetComponent<PlayerCameraFollow>();
    //        UpdateCameraTarget();
    //        }
    //    }

    void Start()
        {
        if (models == null || models.Length == 0)
            {
            Debug.LogError($"{name} の CharacterChange にモデルが登録されていません！");
            return;
            }

        ShowModel(index); // 最初のモデルを表示

        // 自分のプレイヤーならカメラを登録
        if (photonView.IsMine)
            {
            cameraFollow = Camera.main.GetComponent<PlayerCameraFollow>();
            UpdateCameraTarget();
            }
        }


    void Update()
        {
        if (!photonView.IsMine) return; // 自分のプレイヤーだけ処理
        if (models == null || models.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.Z))
            {
            index = (index + 1) % models.Length;
            ShowModel(index);
            UpdateCameraTarget();
            }

        if (Input.GetKeyDown(KeyCode.X))
            {
            index = (index - 1 + models.Length) % models.Length;
            ShowModel(index);
            UpdateCameraTarget();
            }
        }

    public void ShowModel(int targetIndex)
        {
        GameObject current = null;
        foreach (var m in models)
            {
            if (m.activeSelf)
                {
                current = m;
                break;
                }
            }

        Vector3 currentPos = current != null ? current.transform.position : Vector3.zero;
        Quaternion currentRot = current != null ? current.transform.rotation : Quaternion.identity;

        for (int i = 0; i < models.Length; i++)
            {
            models[i].SetActive(i == targetIndex);
            }

        models[targetIndex].transform.position = currentPos;
        models[targetIndex].transform.rotation = currentRot;

        index = targetIndex;
        }

    private void UpdateCameraTarget()
        {
        if (cameraFollow != null)
            {
            cameraFollow.SetTarget(models[index].transform);
            }
        }

    public void SetAsOni()
        {
        ShowModel(0);
        if (photonView.IsMine) UpdateCameraTarget();
        }

    public void SetAsNigeru()
        {
        ShowModel(1);
        if (photonView.IsMine) UpdateCameraTarget();
        }
    }
