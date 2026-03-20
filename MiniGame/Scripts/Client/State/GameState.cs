using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameState : MonoBehaviour, GameStateMachine
{
    [Header("serialize")]
    public GameObject UIObject;
    Main MainScript;
    void Awake()
    {
        Main.LoadState(this, 0, false);
        UIObject = GameObject.Find("Game");
        var allObj = GameObject.Find("All");
        if (allObj != null)
        {
            var settingTransform = allObj.transform.Find("SettingUI");
            if (settingTransform != null)
                _settingUI = settingTransform.GetComponent<SettingUI>();
        }
    }
    public IEnumerator Load(Main _MainScript)
    {
        MainScript = _MainScript;
        yield return null;
    }
    public void UpdateState()
    {
        if (_gameManager != null)
            _gameManager.UpdateManager();
    }
    public void LateUpdateState() { }
    public void FixedUpdateState() { }
    public void StartInput(Interaction.Data _Data) { }
    public void UpdateInputMove(Interaction.Data _Data) { }
    public void EndInput(Interaction.Data _Data) { }
    public void UpdateInputStationary(Interaction.Data _Data) { }
    public void DoChangeState(System.Type _Type, ChangeStateEffect.EffectType _EffectType, float _TimeChange)
        => MainScript.StartChangeState(MainScript.GetState(_Type), _EffectType, _TimeChange);
    public void Enable()
    {
        gameObject.SetActive(true);
        if (UIObject != null)
            UIObject.SetActive(true);
        SoundManager.Instance.PlayMusic(Config.Music.MUSIC_2);
    }
    public void Disable()
    {
        if (gameObject != null)
            gameObject.SetActive(false);
        if (UIObject != null)
            UIObject.SetActive(false);
    }
    public void ShutDown() { }
    public void StartChangeState(GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() == GetType())
            Enable();
    }
    public void UpdateChangeState(GameStateMachine _NextState, GameStateMachine _LastState, float _Time) { }
    public void SetChangeState(GameStateMachine _NextState, GameStateMachine _LastState) { }
    public void EndChangeState(GameStateMachine _NextState, GameStateMachine _LastState)
    {
        if (_NextState.GetType() != GetType())
        {
            Disable();
        }
    }

    public void UpdateUIPosition(Vector3 localPosition)
    {
        UIObject.transform.localPosition = localPosition;
    }
    public void UpdateUISortingOder()
    {
        //UIObject.transform.SetAsLastSibling();
    }

    GameManager _gameManager;
    PauseUI _pauseUI;
    SettingUI _settingUI;

    public IEnumerator Init()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = 1 / 50f;
        Application.runInBackground = true;
        ButtonSettingClick.instance.Init(CallbackClickPause);
        _gameManager = UIObject.GetComponent<GameManager>();
        _pauseUI = UIObject.transform.Find("PauseUI")?.GetComponent<PauseUI>();
        _gameManager.Initialize(this);
        _pauseUI.Init(CallbackContinue, CallbackPlayAgain, CallbackBacktoMenu, CallbackOpenSetting);
        //_settingUI.Init();
        
        // Wait a frame for initialization
        yield return new WaitForEndOfFrame();
        
        // Disable LoadingState UI first
        if (LoadingState.instance != null)
        {
            LoadingState.instance.Disable();
            Debug.Log("✅ LoadingState disabled from GameState.Init()");
        }
        
        // Ensure splash screen is hidden
        if (SlashScreenControl.instance != null)
        {
            SlashScreenControl.instance.Hide();
            Debug.Log("✅ SlashScreen hidden from GameState.Init()");
        }
        
        // Enable GameState UI
        Enable();
        Debug.Log("✅ GameState enabled from Init()");
    }

    public void CallbackClickPause() => _pauseUI.Show();

    public void CallbackContinue()
    {
        print("CallbackContinue");
    }

    public void CallbackPlayAgain()
    {
        _gameManager.ResetGame();
    }

    public void CallbackBacktoMenu()
    {
        print("CallbackBacktoMenu");
        DoChangeState(typeof(LoadingState), ChangeStateEffect.EffectType.None, 0);
    }

    public void CallbackOpenSetting()
    {
        print("CallbackOpenSetting");
        _settingUI.Show();

    }

    public static bool IsPointerOverUIObject()
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

}
