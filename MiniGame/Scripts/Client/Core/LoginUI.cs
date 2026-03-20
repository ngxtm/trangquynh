using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    Button _btnPlay;
    public Button _btnGuidance;
    Guidance _guidance;
    
    public delegate void CallbackPlayNow(bool vsAI);
    public CallbackPlayNow _callbackPlayNow;
    
    public void Init(CallbackPlayNow callbackPlayNow)
    {   
        _guidance = transform.Find("bg/Panel/Instruction")?.GetComponent<Guidance>();
        _callbackPlayNow = callbackPlayNow;

        _btnPlay = transform.Find("bg/play")?.GetComponent<Button>();
        _btnGuidance = transform.Find("bg/guidance")?.GetComponent<Button>();

        _btnPlay?.onClick.RemoveAllListeners();
        _btnGuidance?.onClick.RemoveAllListeners();

        _btnPlay?.onClick.AddListener(() => StartGame(true));
        _btnGuidance?.onClick.AddListener(ShowGuidance);

        _guidance?.Init();
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        SlashScreenControl.instance?.Show(true, SlashScreenControl.instance.Sprites.Length - 1, 1);
    }

    void StartGame(bool vsAI)
    {
        _callbackPlayNow?.Invoke(vsAI);
        Hide();
    }

    public void ShowGuidance() => _guidance?.Show();

    public void Hide() => gameObject.SetActive(false);
}
