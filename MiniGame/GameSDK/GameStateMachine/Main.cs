using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    public static List<GameStateMachine> AllState = new();
    public static List<int> AllOrderNumber = new();
    public static GameStateMachine CurrentState = null;
    public static GameStateMachine NextState = null;
    public static GameStateMachine LastState = null;
    public GameObject ChangeSceneBackGround;
    public Interaction _Interaction;
    public static int FrameRate = 30;
    public static int FixedFrameRate = 30;
    public static int ScreenWidth = 640;
    public static int ScreenHeight = 1136;
    ChangeStateEffect changeStateEffectState;
    public static void Reset ()
    {
        AllState.Clear();
        AllOrderNumber.Clear();
    }
    public static void LoadState (GameStateMachine _GameStateMachine, int _OrderNumber, bool IsFirstStartState)
    {
        AllState.Add(_GameStateMachine);
        AllOrderNumber.Add(_OrderNumber);
        if (IsFirstStartState)
        {
            NextState = _GameStateMachine;
        }
    }
	public void Start () {
        Application.targetFrameRate = FrameRate;
        ScreenHeight = 1136;
        ScreenWidth = (int)((float)Screen.width / (float)Screen.height * ScreenHeight);
        BubbleSort();
        StartCoroutine(Load());
	}
    IEnumerator Load ()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < AllState.Count; i++)
        {
            yield return AllState[i].Load(this);
        }
        for (int i = 0; i < AllState.Count; i++)
        {
            if (AllState[i] != NextState)
            {
                AllState[i].Disable();
            }
        }
        changeStateEffectState = GetState(typeof(ChangeStateEffect)) as ChangeStateEffect;
        changeStateEffectState.Enable(); 
        CurrentState = NextState;
        CurrentState.Enable();
    }
    public void Update () 
    {
        if (CurrentState != null)
        {
            UpdateInput();
            CurrentState.UpdateState();
        }
	}
    public void LateUpdate ()
    {
        if (CurrentState != null)
        {
            CurrentState.LateUpdateState();
        }
    }
    public void FixedUpdate ()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdateState();
        }
    }
    public void ShutDown()
    {
        for (int i = 0; i < AllState.Count; i++)
        {
            AllState[i].ShutDown();
        }
    }
    void BubbleSort() 
    {
        bool swapped = true;
        int j = 0;
        int tmp;
        GameStateMachine _tmp;
        while (swapped) {
            swapped = false;
            j++;
            for (int i = 0; i < AllOrderNumber.Count - j; i++) {                                       
                if (AllOrderNumber[i] > AllOrderNumber[i + 1]) {                          
                    tmp = AllOrderNumber[i];
                    _tmp = AllState[i];
                    int n = AllOrderNumber[i + 1];
                    AllOrderNumber[i] = n;
                    AllOrderNumber[i + 1] = tmp;
                    AllState[i] = AllState[i + 1];
                    AllState[i + 1] = _tmp;
                    swapped = true;
                }
            }                
        }
    }
    public void StartChangeState (GameStateMachine _NextState, ChangeStateEffect.EffectType _Effect, float _Time)
    {
        //ChangeSceneBackGround.transform.SetAsLastSibling();
        //ChangeSceneBackGround.SetActive(true);
        changeStateEffectState.SetStartChangeState (_Effect, _Time, _NextState, CurrentState);
        LastState = CurrentState;
        CurrentState = changeStateEffectState;
        for (int i = 0; i < AllState.Count; i++)
        {
            AllState[i].StartChangeState(_NextState, LastState);
            if (AllState[i] == _NextState)
            {     
                NextState = AllState[i];
                NextState.UpdateUISortingOder();
            }
        }
    }
    public void UpdateChangeState (float _Time)
    {
        for (int i = 0; i < AllState.Count; i++)
        {
            AllState[i].UpdateChangeState (NextState, LastState, _Time);
        }
    }
    public void SetChangeState ()
    {
        for (int i = 0; i < AllState.Count; i++)
        {
            AllState[i].SetChangeState (NextState, LastState);
        }
    }
    public void EndChangeState ()
    {
        //ChangeSceneBackGround.SetActive(false);
        for (int i = 0; i < AllState.Count; i++)
        {
            AllState[i].EndChangeState (NextState, LastState);
        }
        CurrentState = NextState;
    }
    public GameStateMachine GetNextState ()
    {
        return NextState;
    }
    public GameStateMachine GetCurrentState ()
    {
        return CurrentState;
    }
    public GameStateMachine GetLastState ()
    {
        return LastState;
    }
    public GameStateMachine GetState (System.Type _Type)
    {
        for (int i = 0; i < AllState.Count; i++)
        {
            if (AllState[i].GetType() == _Type)
            {
                return AllState[i];
            }
        }
        return null;
    }
    public static int TouchCount;
    void UpdateInput ()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            TouchCount = Input.touchCount;
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    CurrentState.StartInput(_Interaction.CheckInteraction(Input.GetTouch(i).position, Input.GetTouch(i).fingerId, TouchCount));
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary)
                {
                    CurrentState.UpdateInputMove(_Interaction.CheckInteraction(Input.GetTouch(i).position, Input.GetTouch(i).fingerId, TouchCount));
                }
       
                else if (Input.GetTouch(i).phase == TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Canceled)
                {
                    CurrentState.EndInput(_Interaction.CheckInteraction(Input.GetTouch(i).position, Input.GetTouch(i).fingerId, TouchCount));
                }
            }
        }
        else if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchCount = 1;
                CurrentState.StartInput(_Interaction.CheckInteraction(Input.mousePosition, -1, 2));
            }
            else if (Input.GetMouseButton(0))
            {
                TouchCount = 1;
                CurrentState.UpdateInputMove(_Interaction.CheckInteraction(Input.mousePosition, -1, 2));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CurrentState.EndInput(_Interaction.CheckInteraction(Input.mousePosition, -1, 2));
            }
        }
    }
}