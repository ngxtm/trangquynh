using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    // UI references
    private Text  _titleText;
    private Button _btnClose;
    private Toggle _sfxToggle;
    private Toggle _musicToggle;
    private Button _btnBgClose;
    private Button _btnReplayTutorial;
    
    // AI Difficulty (tạo runtime)
    private GameObject _difficultyPanel;
    private Text _difficultyText;
    private int _difficultyIndex = 3;
    private readonly string[] _difficultyNames = { "Dễ", "Trung bình", "Khó", "AI Gemini" };
    private readonly AIDifficulty[] _difficulties = { AIDifficulty.Easy, AIDifficulty.Medium, AIDifficulty.Hard, AIDifficulty.Gemini };

    public void Init()
    {
        _titleText = transform.Find("Content/Tittle")?.GetComponent<Text>();
        if (_titleText != null) _titleText.text = "Cài đặt";

        _btnBgClose = transform.GetComponent<Button>();
        _btnClose = transform.Find("Content/Image/close")?.GetComponent<Button>();
        _btnClose?.onClick.RemoveAllListeners();
        _btnClose?.onClick.AddListener(Hide);
        _btnBgClose?.onClick.RemoveAllListeners();
        _btnBgClose?.onClick.AddListener(Hide);

        var sfxT = transform.Find("Content/SFX/Toggle");
        if (sfxT != null)
        {
            _sfxToggle = sfxT.GetComponent<Toggle>();
            _sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
            _sfxToggle.onValueChanged.RemoveAllListeners();
            _sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        }

        var musicT = transform.Find("Content/Music/Toggle");
        if (musicT != null)
        {
            _musicToggle = musicT.GetComponent<Toggle>();
            _musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
            _musicToggle.onValueChanged.RemoveAllListeners();
            _musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        var tutorialBtn = transform.Find("Content/ReplayTutorial");
        if (tutorialBtn != null)
        {
            _btnReplayTutorial = tutorialBtn.GetComponent<Button>();
            _btnReplayTutorial.onClick.RemoveAllListeners();
            _btnReplayTutorial.onClick.AddListener(OnReplayTutorial);
        }
        
        CreateDifficultySelector();
        LoadDifficulty();
    }
    
    void CreateDifficultySelector()
    {
        var content = transform.Find("Content");
        if (content == null) return;
        
        _difficultyPanel = new GameObject("AIDifficulty", typeof(RectTransform));
        _difficultyPanel.transform.SetParent(content, false);
        var rect = _difficultyPanel.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(400, 100);
        rect.anchoredPosition = new Vector2(0, -80);
        
        // Background
        var bg = _difficultyPanel.AddComponent<Image>();
        bg.color = new Color(0.2f, 0.25f, 0.35f, 0.8f);
        
        // Label with icon
        var label = new GameObject("Label", typeof(RectTransform), typeof(Text));
        label.transform.SetParent(_difficultyPanel.transform, false);
        var lRect = label.GetComponent<RectTransform>();
        lRect.anchorMin = new Vector2(0, 1);
        lRect.anchorMax = new Vector2(1, 1);
        lRect.pivot = new Vector2(0.5f, 1);
        lRect.sizeDelta = new Vector2(0, 35);
        lRect.anchoredPosition = new Vector2(0, -5);
        var lText = label.GetComponent<Text>();
        lText.text = "🤖 ĐỘ KHÓ AI";
        lText.fontSize = 20;
        lText.color = Color.white;
        lText.alignment = TextAnchor.MiddleCenter;
        lText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // Row with buttons - larger touch targets
        var row = new GameObject("Row", typeof(RectTransform));
        row.transform.SetParent(_difficultyPanel.transform, false);
        var rRect = row.GetComponent<RectTransform>();
        rRect.anchorMin = rRect.anchorMax = new Vector2(0.5f, 0.5f);
        rRect.sizeDelta = new Vector2(380, 50);
        rRect.anchoredPosition = new Vector2(0, -15);
        
        // Prev button - larger
        CreateNavButton(row.transform, "◀", -160, () => ChangeDifficulty(-1));
        
        // Difficulty text - larger, colored by difficulty
        var txtBg = new GameObject("DiffBg", typeof(RectTransform), typeof(Image));
        txtBg.transform.SetParent(row.transform, false);
        var bgRect = txtBg.GetComponent<RectTransform>();
        bgRect.anchorMin = bgRect.anchorMax = new Vector2(0.5f, 0.5f);
        bgRect.sizeDelta = new Vector2(200, 45);
        bgRect.anchoredPosition = Vector2.zero;
        txtBg.GetComponent<Image>().color = new Color(0.15f, 0.2f, 0.3f);
        
        var txtGO = new GameObject("DiffText", typeof(RectTransform), typeof(Text));
        txtGO.transform.SetParent(txtBg.transform, false);
        var tRect = txtGO.GetComponent<RectTransform>();
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.offsetMin = tRect.offsetMax = Vector2.zero;
        _difficultyText = txtGO.GetComponent<Text>();
        _difficultyText.text = _difficultyNames[_difficultyIndex];
        _difficultyText.fontSize = 24;
        _difficultyText.color = GetDifficultyColor(_difficultyIndex);
        _difficultyText.alignment = TextAnchor.MiddleCenter;
        _difficultyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // Next button - larger
        CreateNavButton(row.transform, "▶", 160, () => ChangeDifficulty(1));
    }
    
    Color GetDifficultyColor(int index)
    {
        return index switch
        {
            0 => new Color(0.4f, 0.8f, 0.4f),  // Easy - Green
            1 => new Color(1f, 0.8f, 0.2f),    // Medium - Yellow
            2 => new Color(1f, 0.4f, 0.3f),    // Hard - Red
            _ => new Color(0.5f, 0.7f, 1f)     // Gemini - Blue
        };
    }
    
    void CreateNavButton(Transform parent, string text, float x, UnityEngine.Events.UnityAction action)
    {
        var btn = new GameObject("Btn", typeof(RectTransform), typeof(Image), typeof(Button));
        btn.transform.SetParent(parent, false);
        var r = btn.GetComponent<RectTransform>();
        r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
        r.sizeDelta = new Vector2(55, 45);
        r.anchoredPosition = new Vector2(x, 0);
        btn.GetComponent<Image>().color = new Color(0.3f, 0.5f, 0.35f);
        btn.GetComponent<Button>().onClick.AddListener(action);
        
        var txt = new GameObject("T", typeof(RectTransform), typeof(Text));
        txt.transform.SetParent(btn.transform, false);
        var tr = txt.GetComponent<RectTransform>();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = tr.offsetMax = Vector2.zero;
        var t = txt.GetComponent<Text>();
        t.text = text;
        t.fontSize = 26;
        t.color = Color.white;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void ChangeDifficulty(int delta)
    {
        _difficultyIndex = (_difficultyIndex + delta + _difficulties.Length) % _difficulties.Length;
        if (_difficultyText != null)
        {
            _difficultyText.text = _difficultyNames[_difficultyIndex];
            _difficultyText.color = GetDifficultyColor(_difficultyIndex);
        }
        SaveDifficulty();
        AIManager.Instance?.SetAIDifficulty(_difficulties[_difficultyIndex]);
        SoundManager.Instance?.PlaySFX(Config.SFX.CLICK);
    }
    
    void LoadDifficulty()
    {
        _difficultyIndex = PlayerPrefs.GetInt("AIDifficulty", 3);
        if (_difficultyIndex >= _difficulties.Length) _difficultyIndex = 3;
        if (_difficultyText != null) _difficultyText.text = _difficultyNames[_difficultyIndex];
    }
    
    void SaveDifficulty()
    {
        PlayerPrefs.SetInt("AIDifficulty", _difficultyIndex);
        PlayerPrefs.Save();
    }
    
    public AIDifficulty GetSelectedDifficulty() => _difficulties[_difficultyIndex];

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnSfxToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
        SoundManager.Instance?.EnableSFX(isOn);
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("MusicEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
        SoundManager.Instance?.EnableMusic(isOn);
    }

    private void OnReplayTutorial()
    {
        Hide();
        TutorialManager.Instance?.ResetTutorial();
        TutorialManager.Instance?.StartTutorial(null);
    }
}
