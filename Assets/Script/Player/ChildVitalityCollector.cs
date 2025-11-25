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
        if (other.CompareTag("vitality"))
            {
            lanternStatus ls = other.GetComponent<lanternStatus>();
            if (ls == null) return;

            // ★ CurrentVitality が 0 なら加算不可
            if (ls.CurrentVitality <= 0)
                return;

            if (Input.GetKey("joystick button 1"))
                {
                addTimer += Time.deltaTime;

                //    if (addTimer >= 1f)
                //        {
                //        addTimer = 0f;

                //        if (role.CurrentRole == "Oni") return;

                //        int addValue = ls.GetRecoveryVitality();

                //        // ランタン残量を減らす
                //        ls.AddVitality(-addValue);

                //        // プレイヤーに加算
                //        parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, addValue);
                //        }
                //    }
                //else
                //    {
                //    addTimer = 0f;
                //    }

                if (addTimer >= 1f)
                    {
                    addTimer = 0f;

                    if (role.CurrentRole == "Oni") return;

                    int addValue = ls.GetRecoveryVitality();

                    PhotonView lsView = ls.GetComponent<PhotonView>();
                    if (lsView != null)
                        lsView.RPC("RpcConsumeVitality", RpcTarget.MasterClient, addValue);

                    parentView.RPC("AddScoreRPC", RpcTarget.All, role.CurrentTeam, addValue);
                    }

                }
            }


            }
    }
    
