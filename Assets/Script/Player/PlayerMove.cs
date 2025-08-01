using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPun
    {
    [SerializeField] GameObject allow = default;
    [SerializeField] float moveSpeed = 3.0f; // à⁄ìÆë¨ìx

    void Start()
        {
        if (photonView.IsMine)
            {
            allow.gameObject.SetActive(true);
            }
        }

    void Update()
        {
        if (!photonView.IsMine) return;

        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            {
            move += Vector3.forward;
            }
        if (Input.GetKey(KeyCode.S))
            {
            move += Vector3.back;
            }
        if (Input.GetKey(KeyCode.A))
            {
            move += Vector3.left;
            }
        if (Input.GetKey(KeyCode.D))
            {
            move += Vector3.right;
            }

        // ê≥ãKâªÇµÇƒë¨ìxàÍíËÇ…
        if (move != Vector3.zero)
            {
            transform.position += move.normalized * moveSpeed * Time.deltaTime;
            }
        }
    }
