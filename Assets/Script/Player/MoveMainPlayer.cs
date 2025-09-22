using UnityEngine;

public class MoveMainPlayer : MonoBehaviour
    {
    [Header("追従する子オブジェクト")]
    public Transform child1;
    public Transform child2;

    [Header("どちらを参照するか")]
    public bool useChild1 = true;

    void Update()
        {
        // 追従する子を決める
        Transform targetChild = useChild1 ? child1 : child2;

        if (targetChild != null)
            {
            // ワールド座標で親に追従
            transform.position = targetChild.position;
            transform.rotation = targetChild.rotation;
            }
        }
    }
