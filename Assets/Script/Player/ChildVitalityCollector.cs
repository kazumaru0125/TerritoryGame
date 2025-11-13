using UnityEngine;
using Photon.Pun;

public class ChildVitalityCollector : MonoBehaviour
    {
    public int Vitality = 1; // この子が取ったときのスコア
    public int minusVitality = 1;
    private float addTimer = 0f; // 加算用タイマー

    private void OnCollisionEnter(Collision collision)
        {
        //if (collision.gameObject.CompareTag("vitality"))
        //    {
        //    TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
        //    if (role == null || string.IsNullOrEmpty(role.CurrentTeam)) return;

        //    // Oni は加算しない
        //    if (role.CurrentRole == "Oni") return;

        //    PhotonView parentView = role.photonView;
        //    if (parentView == null || !parentView.IsMine) return;

        //    // スコア加算 RPC
        //    parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, Vitality);

        //    // アイテム削除 RPC
        //    PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();
        //    if (targetView != null)
        //        {
        //        parentView.RPC("RequestDestroyRPC", RpcTarget.MasterClient, targetView.ViewID);
        //        }
        //    }
        }

    private void OnTriggerStay(Collider other)
        {
        TestPlayerRoll role = GetComponentInParent<TestPlayerRoll>();
        if (role == null) return;
        PhotonView parentView = role.photonView;
        if (parentView == null || !parentView.IsMine) return;

        // --- 減算アイテムに当たっているとき ---
        if (other.gameObject.CompareTag("AttackHitbox"))
            {
            parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, -minusVitality);
            }

        // --- vitalityエリアにいる場合 ---
        if (other.gameObject.CompareTag("vitality"))
            {
            // XBOX の B ボタン（Input の "joystick button 1"）を押している間のみ
            if (Input.GetKey("joystick button 1"))
                {
                addTimer += Time.deltaTime;

                if (addTimer >= 1f) // 1秒ごとに加算
                    {
                    addTimer = 0f;

                    // Oniは加算しない
                    if (role.CurrentRole == "Oni") return;

                    parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, Vitality);
                    }
                }
            else
                {
                addTimer = 0f; // 離したらリセット
                }
            }
        }
    }
