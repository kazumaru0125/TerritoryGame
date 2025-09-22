using UnityEngine;
using Photon.Pun;
public class AuraChangeScript : MonoBehaviour
    {
    [SerializeField] private ParticleSystem human; // Inspectorに割り当て
    [SerializeField] private ParticleSystem oni;   // Inspectorに割り当て

    private PlayerRole playerRole;

    void Start()
        {
        // 同じオブジェクトに PlayerRole がある前提
        playerRole = GetComponent<PlayerRole>();
        }

    void Update()
        {
        if (playerRole == null) return;

        if (playerRole.CurrentRole == "Human")
            {
            if (!human.isPlaying) human.Play();
            if (oni.isPlaying) oni.Stop();
            }
        else if (playerRole.CurrentRole == "Oni")
            {
            if (!oni.isPlaying) oni.Play();
            if (human.isPlaying) human.Stop();
            }
        else
            {
            // どちらでもない場合は全部止める
            if (human.isPlaying) human.Stop();
            if (oni.isPlaying) oni.Stop();
            }
        }
    }
