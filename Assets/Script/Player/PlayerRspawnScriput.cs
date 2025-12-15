using UnityEngine;
using Photon.Pun;

public class PlayerRespawnScript : MonoBehaviourPunCallbacks
    {
    [SerializeField] private float fallLimitY = -5f;
    private bool isRespawning = false;

    private void Update()
        {
        if (!photonView.IsMine) return;

        Debug.Log($"[RespawnCheck] y = {transform.position.y}");

        if (transform.position.y < -5f)
            {
 
            RespawnAtRandomSpawnArea();
            }
        }


    private System.Collections.IEnumerator RespawnDelay()
        {
        isRespawning = true;


        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            }

        yield return null;

        RespawnAtRandomSpawnArea();

      
        yield return new WaitForSeconds(0.5f);
        isRespawning = false;
        }



    public void RespawnAtRandomSpawnArea()
        {

        GameObject[] spawnAreas = GameObject.FindGameObjectsWithTag("SpawnArea");
        if (spawnAreas.Length == 0)
            {
         ;
            return;
            }

        GameObject randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector3 areaPos = randomArea.transform.position;
        Vector3 newPosition = new Vector3(areaPos.x, areaPos.y + 1.0f, areaPos.z);

      
        transform.position = newPosition;

     
        photonView.RPC(nameof(RPC_SetRespawnPosition), RpcTarget.Others, newPosition);

        Debug.Log($"[������] {gameObject.name} �� {randomArea.name} �̏�Ɉړ����܂����B");


        }

    [PunRPC]
    void RPC_SetRespawnPosition(Vector3 newPosition)
        {
        transform.position = newPosition;
        Debug.Log($"[���N���C�A���g] {gameObject.name} �����X�|�[�����܂����B");
        }
    }
