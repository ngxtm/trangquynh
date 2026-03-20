using UnityEngine;
using System.Collections;

public class ExampleState : MonoBehaviour, GameStateMachine {

    Main MainScript;
    public GameObject UIObject;

    void Awake()
    {
        Main.LoadState(this, 0, false);
    }
    public IEnumerator Load (Main _MainScript)
    {
        MainScript = _MainScript;
        yield return null;
    }
    public void UpdateState ()
    {
        
    }
    public void LateUpdateState ()
    {
        
    }
    public void FixedUpdateState ()
    {
        
    }
    public void StartInput (Interaction.Data _Data)
    {
        
    }
    public void UpdateInputMove (Interaction.Data _Data)
    {

    }
    public void UpdateInputStationary (Interaction.Data _Data)
    {

    }
    public void EndInput (Interaction.Data _Data)
    {
        
    }
    public void DoChangeState (System.Type _Type, ChangeStateEffect.EffectType _EffectType, float _TimeChange)
    {
        MainScript.StartChangeState(MainScript.GetState (_Type), _EffectType, _TimeChange);
    }
    public void ShutDown ()
    {

    }
    public void Enable ()
    {
        gameObject.SetActive(true);
        if(UIObject != null)
            UIObject.SetActive(true);
    }
    public void Disable ()
    {
        gameObject.SetActive(false);
        if(UIObject != null)
            UIObject.SetActive(false);
    }
    public void StartChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() == GetType())
        {
            Enable();
        }
    }
    public void UpdateChangeState (GameStateMachine _NextState, GameStateMachine _LastState, float _Time)
    {

    }
    public void SetChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {

    }
    public void EndChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() != GetType())
        {
            Disable();
        }
    }
    public void UpdateUIPosition (Vector3 localPosition)
    {
        UIObject.transform.localPosition = localPosition;
    }
    public void UpdateUISortingOder ()
    {
        UIObject.transform.SetAsLastSibling();
    }
}
