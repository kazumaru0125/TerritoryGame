using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;


public class PlayerCameraController : MonoBehaviour
    {
    private ICameraState currentState;

    public Transform target; // 追従対象（プレイヤー）
    public Vector3 followOffset = new Vector3(0, 2, -5);
    public float smoothSpeed = 10f;

    private void Start()
        {
        // 初期状態を設定（追従カメラ）
        ChangeState(new FollowCameraState());
        }

    private void Update()
        {
        if (currentState != null)
            {
            currentState.UpdateState(this);
            }
        }

    public void ChangeState(ICameraState newState)
        {
        if (currentState != null)
            {
            currentState.ExitState(this);
            }

        currentState = newState;
        currentState.EnterState(this);
        }
    }
