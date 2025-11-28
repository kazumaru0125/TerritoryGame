using UnityEngine;
using System.Linq;
using Photon.Pun;

public class NearestGuidePhoton : MonoBehaviourPun
    {
    [Header("タグ設定")]
    //public string oniTag = "Oni";
    public string ofudaTag = "Ofuda";

    [Header("ガイドオブジェクト")]
 //   public Transform guideOni;
    public Transform guideOfuda;

    [Header("ガイド位置")]
    public float guideHeight = 1.8f;
    public float guideDistance = 1.0f;

    private Transform cachedTransform;

    void Start()
        {
        cachedTransform = transform;

        // 他プレイヤーのガイドは非表示
        if (!photonView.IsMine)
            {
          //  if (guideOni) guideOni.gameObject.SetActive(false);
            if (guideOfuda) guideOfuda.gameObject.SetActive(false);
            }
        }

    void Update()
        {
     
        if (!photonView.IsMine) return;

       // UpdateGuide(oniTag, guideOni);
        UpdateGuide(ofudaTag, guideOfuda);
        }

    void UpdateGuide(string tag, Transform guide)
        {
        if (guide == null) return;

        GameObject nearest = FindNearestTarget(tag);

        if (nearest == null)
            {
            guide.gameObject.SetActive(false);
            return;
            }

        guide.gameObject.SetActive(true);

        // ガイド座標 = プレイヤー頭上
        Vector3 basePos = cachedTransform.position + Vector3.up * guideHeight;
        guide.position = basePos;

        // ガイドが対象を向く
        guide.LookAt(nearest.transform.position);

        // 少し前へ
        guide.position += guide.forward * guideDistance;
        }

    GameObject FindNearestTarget(string tag)
        {
        var targets = GameObject.FindGameObjectsWithTag(tag);
        if (targets.Length == 0) return null;

        return targets
            .OrderBy(t => Vector3.Distance(cachedTransform.position, t.transform.position))
            .FirstOrDefault();
        }
    }
