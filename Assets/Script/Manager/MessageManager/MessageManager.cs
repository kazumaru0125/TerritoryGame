using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviourPun
    {
    [SerializeField] TMP_InputField inputField = default;
[SerializeField] TMP_Text receiveText = default;

    void Start()
        {
        receiveText.text = "";
        }

    void SetReceiveText(string message)
        {
     
        receiveText.text = message;
        StartCoroutine(HideMessageAfterSeconds(15.0f));
        }

    IEnumerator HideMessageAfterSeconds(float time)
        {
        yield return new WaitForSeconds(time);
        receiveText.text = "";
        }

    public void SendText(string message)
        {
        Debug.Log("Received message: " + message); // ← 追加
        photonView.RPC(nameof(ReceiveText), RpcTarget.All, message);
        }

    public void SendInputText()
        {
        if (!string.IsNullOrWhiteSpace(inputField.text))
            {
            SendText(inputField.text);
            inputField.text = "";
            }
        }

    [PunRPC]
    void ReceiveText(string message)
        {
     
        SetReceiveText(message);
        }
    }
