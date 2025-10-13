using UnityEngine;

public class MoveingState : IPlayerState
{


    //開始
    public void EnterState(PlayerController player)
    {
        Debug.Log("クラウチングスタート");
    }
    //更新処理
    public void UpdateState(PlayerController player)
    {
        Debug.Log("移動中");
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveY) * player.moveSpeed * Time.deltaTime;
        player.transform.position += move;
    }
    //終了
    public void ExitState(PlayerController player)
    {
        Debug.Log("うー");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
