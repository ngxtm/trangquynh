using UnityEngine;
using System.Collections;

public class ChangeStateEffect : MonoBehaviour, GameStateMachine {

    // varible here //
    Main MainScript;

    int ChangeStateTime;
    int CurrentTime;
    float MaxTransparency;

    public GameObject UIObject;
    GameStateMachine nextState;
    GameStateMachine lastState;
    public AnimationCurve Graphics;

    public enum EffectType
    {
        FromLeft,
        FromRight,
        FromUp,
        FromDown,
        None
    }

    EffectType CurrentEffect;
    // varible here //





    // init functions (call your own function only, don't code anything here) //
    void Awake()
    {
        Main.LoadState(this, 0, false);
    }

    public IEnumerator Load (Main _MainScript)
    {
        MainScript = _MainScript;
        yield return null;
    }
    // init functions (call your own function only, don't code anything here) //





    // functions run while do any change state //
    public void StartChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {
        
    }

    public void UpdateChangeState (GameStateMachine _NextState, GameStateMachine _LastState, float _Time)
    {
       
    }

    public void SetChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {

    }

    public void EndChangeState (GameStateMachine _NextState, GameStateMachine _LastState)
    {

    }
    // functions run while do any change state //





    // functions run while this state is available (call your own function only, don't code anything here) //
    public void UpdateState ()
    {
        UpdateChangeState();
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
    public void ShutDown()
    {

    }

    //Don't change this//
    public void Enable ()
    {
        gameObject.SetActive(true);
    }

    public void Disable ()
    {
        gameObject.SetActive(false);
    }
    // functions run while this state is available (call your own function only, don't code anything here) //

    public void UpdateUIPosition (Vector3 localPosition)
    {
        //UIObject.transform.localPosition = localPosition;
    }

    public void UpdateUISortingOder ()
    {
        //UIObject.transform.SetAsLastSibling();
    }

    public void SetStartChangeState (EffectType effectType, float time, GameStateMachine _NextState, GameStateMachine _LastState)
    {
        ChangeStateTime = (int)(time * Main.FrameRate);
        CurrentTime = ChangeStateTime;
        CurrentEffect = effectType;
        nextState = _NextState;
        lastState = _LastState;
        switch (CurrentEffect)
        {
            case EffectType.FromDown:
                _NextState.UpdateUIPosition (new Vector3 (0, -Main.ScreenHeight, 0));
                break;
            case EffectType.FromLeft:
                _NextState.UpdateUIPosition (new Vector3 (-Main.ScreenWidth, 0, 0));
                break;
            case EffectType.FromUp:
                _NextState.UpdateUIPosition (new Vector3 (0, Main.ScreenHeight, 0));
                break;
            case EffectType.FromRight:
                _NextState.UpdateUIPosition (new Vector3 (Main.ScreenWidth, 0, 0));
                break;
        }
    }

    void UpdateChangeState ()
    {
        if (CurrentTime > 0)
        {
            CurrentTime--;
            float f = (float)(CurrentTime)/(float)(ChangeStateTime);
            MainScript.UpdateChangeState(1-f);
            switch (CurrentEffect)
            {
                case EffectType.FromDown:
                    nextState.UpdateUIPosition (new Vector3 (0, -Main.ScreenHeight* Graphics.Evaluate (f), 0));
                    lastState.UpdateUIPosition (new Vector3 (0, Main.ScreenHeight* (1-Graphics.Evaluate (f)), 0));
                    break;
                case EffectType.FromLeft:
                    nextState.UpdateUIPosition (new Vector3 (-Main.ScreenWidth* Graphics.Evaluate (f), 0, 0));
                    lastState.UpdateUIPosition (new Vector3 (Main.ScreenWidth* (1-Graphics.Evaluate (f)), 0, 0));
                    break;
                case EffectType.FromUp:
                    nextState.UpdateUIPosition (new Vector3 (0, Main.ScreenHeight* Graphics.Evaluate (f), 0));
                    lastState.UpdateUIPosition (new Vector3 (0, -Main.ScreenHeight* (1-Graphics.Evaluate (f)), 0));
                    break;
                case EffectType.FromRight:
                    nextState.UpdateUIPosition (new Vector3 (Main.ScreenWidth* Graphics.Evaluate (f), 0, 0));
                    lastState.UpdateUIPosition (new Vector3 (-Main.ScreenWidth* (1-Graphics.Evaluate (f)), 0, 0));
                    break;
            }
        }
        else
        {
            MainScript.EndChangeState();
        }
    }
    // your own function here //
}
