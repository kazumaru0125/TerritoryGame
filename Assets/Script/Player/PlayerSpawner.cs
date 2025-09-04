using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
    void Start()
        {
        if (PhotonNetwork.InRoom) // Ç∑Ç≈Ç…ÉãÅ[ÉÄÇ…Ç¢ÇÈèÍçá
            {
            SpawnPlayer();
            }
        }

    void SpawnPlayer()
        {
        Vector3 pos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f);
        PhotonNetwork.Instantiate("akai", pos, Quaternion.identity);
        }
    }
