using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon.Realtime;

public class ChangeSceneManager : MonoBehaviourPunCallbacks
    {
    public static ChangeSceneManager Instance;

    private void Awake()
        {
        if (Instance == null)
            {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            }
        else
            {
            Destroy(gameObject);
            }
        }

    public void GoToTitleScene(float delay = 0f)
        {
        if (PhotonNetwork.IsConnected)
            {
            StartCoroutine(DisconnectAndLoadTitle(delay));
            }
        else
            {
            SceneManager.LoadScene("TitleScene");
            }
        }

    private IEnumerator DisconnectAndLoadTitle(float delay)
        {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Disconnect();
        }

    public override void OnDisconnected(DisconnectCause cause)
        {
        Debug.Log("Photon disconnected: " + cause);
        SceneManager.LoadScene("TitleScene");
        }
    }
