using UnityEngine;

public class PlayerIdlingState : IPlayerState
{
    //開始
    public void EnterState(PlayerController player)
    {
        //Debug.Log("全裸待機");
    }
    //更新処理
    public void UpdateState(PlayerController player)
    {
        //Debug.Log("出勤");
    }
    //終了
    public void ExitState(PlayerController player)
    {
        //Debug.Log("解散");
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
