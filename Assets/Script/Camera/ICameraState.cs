using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState
    {
    void EnterState(PlayerCameraController camera);
    void UpdateState(PlayerCameraController camera);
    void ExitState(PlayerCameraController camera);
    }
