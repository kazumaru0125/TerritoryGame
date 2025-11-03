using UnityEngine;
using Photon.Pun;

/// <summary>
/// 親（Player）に付けるスクリプト。
/// 子のHuman/Oniを切り替えつつ、Photonの同期対象を動的に変更する。
/// </summary>
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PhotonAnimatorView))]
public class PlayerPhotonController : MonoBehaviourPun
    {
    [Header("子オブジェクト参照")]
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject oni;

    private PhotonTransformView transformView;
    private PhotonAnimatorView animatorView;

    private Transform currentTransform;
    private Animator currentAnimator;

    void Awake()
        {
        transformView = GetComponent<PhotonTransformView>();
        animatorView = GetComponent<PhotonAnimatorView>();
        }

    void Start()
        {
        // 初期状態（Humanを有効にする）
        SwitchToHuman();
        }

    /// <summary>
    /// Humanを有効化し、Oniを無効化する
    /// </summary>
    public void SwitchToHuman()
        {
        if (human == null || oni == null) return;

        human.SetActive(true);
        oni.SetActive(false);

        currentTransform = human.transform;
        currentAnimator = human.GetComponent<Animator>();

        ApplySyncTargets();
        }

    /// <summary>
    /// Oniを有効化し、Humanを無効化する
    /// </summary>
    public void SwitchToOni()
        {
        if (human == null || oni == null) return;

        human.SetActive(false);
        oni.SetActive(true);

        currentTransform = oni.transform;
        currentAnimator = oni.GetComponent<Animator>();

        ApplySyncTargets();
        }

    /// <summary>
    /// Photon同期対象を更新する（Transform / Animator）
    /// </summary>
    private void ApplySyncTargets()
        {
        if (transformView != null)
            {
            // Transform同期対象を変更
            transformView.m_SynchronizePosition = true;
            transformView.m_SynchronizeRotation = true;
            transformView.m_SynchronizeScale = false;

            // Transformの対象を直接差し替える
            transformView.transform.SetParent(currentTransform.parent);
            transformView.transform.localPosition = currentTransform.localPosition;
            transformView.transform.localRotation = currentTransform.localRotation;
            }

        if (animatorView != null && currentAnimator != null)
            {
            // Animatorを切り替える
          //  animatorView.SetAnimator(currentAnimator);
            }
        }

    /// <summary>
    /// 入力や動作の処理はローカルプレイヤーのみ実行
    /// </summary>
    void Update()
        {
        if (!photonView.IsMine) return;

        // デバッグ用切り替え
        if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            SwitchToHuman();
            }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            SwitchToOni();
            }
        }
    }
