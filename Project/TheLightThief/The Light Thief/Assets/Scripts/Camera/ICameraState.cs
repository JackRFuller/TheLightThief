using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState {

    void OnEnterState();

    void OnUpdateState();

    void OnExitState(ICameraState newState);
}
