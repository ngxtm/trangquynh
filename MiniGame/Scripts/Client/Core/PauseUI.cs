using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public delegate void ButtonCallback();

    [Header("Root Panel")]
    [SerializeField] private GameObject _rootPanel;

    [Header("Buttons")]
    [SerializeField] private Button _btnContinue;
    [SerializeField] private Button _btnPlayAgain;
    [SerializeField] private Button _btnBackToMenu;
    [SerializeField] private Button _btnSettings;
    [SerializeField] private Button _btnClose;  // nút X góc
    [SerializeField] private Button _btnBgClose;

    // Callbacks
    private ButtonCallback _onContinue;
    private ButtonCallback _onPlayAgain;
    private ButtonCallback _onBackToMenu;
    private ButtonCallback _onSettings;

    /// <summary>
    /// Init callback cho từng nút
    /// </summary>
    public void Init(
        ButtonCallback onContinue,
        ButtonCallback onPlayAgain,
        ButtonCallback onBackToMenu,
        ButtonCallback onSettings = null
    )
    {
        _onContinue = onContinue;
        _onPlayAgain = onPlayAgain;
        _onBackToMenu = onBackToMenu;
        _onSettings = onSettings;

        if (_rootPanel == null)
            _rootPanel = transform/* .Find("Content")? */.gameObject;
        if (_btnContinue == null)
            _btnBgClose = transform.GetComponent<Button>();
        if (_btnContinue == null)
                _btnContinue = transform.Find("Content/Continue")?.GetComponent<Button>();
        if (_btnPlayAgain == null)
            _btnPlayAgain = transform.Find("Content/Play Again")?.GetComponent<Button>();
        if (_btnBackToMenu == null)
            _btnBackToMenu = transform.Find("Content/Back to menu")?.GetComponent<Button>();
        if (_btnSettings == null)
            _btnSettings = transform.Find("Content/Settings")?.GetComponent<Button>();
        if (_btnClose == null)
            _btnClose = transform.Find("Content/Image/close")?.GetComponent<Button>();

        _btnBgClose.onClick.RemoveAllListeners();
        _btnContinue.onClick.RemoveAllListeners();
        _btnPlayAgain.onClick.RemoveAllListeners();
        _btnBackToMenu.onClick.RemoveAllListeners();
        _btnClose.onClick.RemoveAllListeners();
        _btnSettings.onClick.RemoveAllListeners();

        _btnContinue.onClick.AddListener(HandleContinue);
        _btnPlayAgain.onClick.AddListener(HandlePlayAgain);
        _btnBackToMenu.onClick.AddListener(HandleBackToMenu);
        _btnSettings.onClick.AddListener(HandleSettings);
        _btnClose.onClick.AddListener(HandleClose);
        _btnBgClose.onClick.AddListener(HandleClose);

        Hide();
    }

    /// <summary>Hiện panel và pause game</summary>
    public void Show()
    {
        _rootPanel.SetActive(true);
        Time.timeScale = 0f;
        SoundManager.Instance.PauseMusic(true);
    }

    /// <summary>Ẩn panel và resume game</summary>
    public void Hide()
    {
        _rootPanel.SetActive(false);
        Time.timeScale = 1f;
        SoundManager.Instance.PauseMusic(false);
    }

    #region Internal Handlers
    private void HandleContinue()
    {
        Hide();
        _onContinue?.Invoke();
    }

    private void HandlePlayAgain()
    {
        Hide();
        _onPlayAgain?.Invoke();
    }

    private void HandleBackToMenu()
    {
        Hide();
        _onBackToMenu?.Invoke();
    }

    private void HandleSettings() => _onSettings?.Invoke();

    private void HandleClose() => HandleContinue();
    #endregion
}
