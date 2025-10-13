using Photon.Pun.Demo.SlotRacer;
using UnityEngine;

public interface IPlayerState
{
    //開始
    void EnterState(PlayerController player);
    //更新処理
    void UpdateState(PlayerController player);
    //終了
    void ExitState(PlayerController player);
}
