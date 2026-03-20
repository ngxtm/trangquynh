using UnityEngine;
using System.Collections;

public interface GameStateMachine {

	IEnumerator Load (Main _MainScript);

    void StartChangeState (GameStateMachine _NextState, GameStateMachine _LastState);
    void UpdateChangeState (GameStateMachine _NextState, GameStateMachine _LastState, float _Time);
    void SetChangeState (GameStateMachine _NextState, GameStateMachine _LastState);
    void EndChangeState (GameStateMachine _NextState, GameStateMachine _LastState);

	void UpdateState ();
    void LateUpdateState ();
    void FixedUpdateState ();
	
    void StartInput (Interaction.Data _Data);
    void UpdateInputMove (Interaction.Data _Data);
    void UpdateInputStationary (Interaction.Data _Data);
    void EndInput (Interaction.Data _Data);

    void Enable();
    void Disable ();
    void ShutDown();

    void UpdateUIPosition (Vector3 localPosition);
    void UpdateUISortingOder();
}
