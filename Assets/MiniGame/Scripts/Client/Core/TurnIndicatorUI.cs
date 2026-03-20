using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI hiển thị trạng thái lượt chơi và AI thinking
/// </summary>
public class TurnIndicatorUI : MonoBehaviour
{
    public static TurnIndicatorUI Instance;
    
    private GameObject _banner;
    private Text _bannerText;
    private GameObject _thinkingPopup;
    private Image _bannerBg;
    private float _dotTimer;
    private int _dotCount;
    
    void Awake() => Instance = this;
    
    void Start() => CreateUI();
    
    void Update()
    {
        // Animate dots khi AI thinking
        if (_thinkingPopup != null && _thinkingPopup.activeSelf)
        {
            _dotTimer += Time.deltaTime;
            if (_dotTimer > 0.4f)
            {
                _dotTimer = 0;
                _dotCount = (_dotCount + 1) % 4;
                var txt = _thinkingPopup.GetComponentInChildren<Text>();
                if (txt != null) txt.text = "🤖 AI đang suy nghĩ" + new string('.', _dotCount + 1);
            }
        }
    }
    
    void CreateUI()
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;
        
        // === TURN BANNER (top) ===
        _banner = new GameObject("TurnBanner", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        _banner.transform.SetParent(canvas.transform, false);
        var bRect = _banner.GetComponent<RectTransform>();
        bRect.anchorMin = new Vector2(0, 1);
        bRect.anchorMax = new Vector2(1, 1);
        bRect.pivot = new Vector2(0.5f, 1);
        bRect.sizeDelta = new Vector2(0, 80);
        bRect.anchoredPosition = Vector2.zero;
        _bannerBg = _banner.GetComponent<Image>();
        _bannerBg.color = new Color(0.2f, 0.5f, 0.2f, 0.9f);
        
        var txtGO = new GameObject("Text", typeof(RectTransform), typeof(Text));
        txtGO.transform.SetParent(_banner.transform, false);
        var tRect = txtGO.GetComponent<RectTransform>();
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.offsetMin = tRect.offsetMax = Vector2.zero;
        _bannerText = txtGO.GetComponent<Text>();
        _bannerText.text = "👤 LƯỢT CỦA BẠN";
        _bannerText.fontSize = 32;
        _bannerText.color = Color.white;
        _bannerText.alignment = TextAnchor.MiddleCenter;
        _bannerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // === AI THINKING POPUP (center) ===
        _thinkingPopup = new GameObject("AIThinking", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        _thinkingPopup.transform.SetParent(canvas.transform, false);
        var pRect = _thinkingPopup.GetComponent<RectTransform>();
        pRect.anchorMin = pRect.anchorMax = new Vector2(0.5f, 0.5f);
        pRect.sizeDelta = new Vector2(350, 100);
        _thinkingPopup.GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
        
        var pTxt = new GameObject("Text", typeof(RectTransform), typeof(Text));
        pTxt.transform.SetParent(_thinkingPopup.transform, false);
        var ptRect = pTxt.GetComponent<RectTransform>();
        ptRect.anchorMin = Vector2.zero;
        ptRect.anchorMax = Vector2.one;
        ptRect.offsetMin = ptRect.offsetMax = Vector2.zero;
        var pt = pTxt.GetComponent<Text>();
        pt.text = "🤖 AI đang suy nghĩ...";
        pt.fontSize = 28;
        pt.color = Color.white;
        pt.alignment = TextAnchor.MiddleCenter;
        pt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        _thinkingPopup.SetActive(false);
        _banner.SetActive(false);
    }
    
    public void ShowPlayerTurn()
    {
        if (_banner == null) return;
        _banner.SetActive(true);
        _bannerText.text = "👤 LƯỢT CỦA BẠN";
        _bannerBg.color = new Color(0.2f, 0.5f, 0.2f, 0.9f);
        HideThinking();
    }
    
    public void ShowAITurn()
    {
        if (_banner == null) return;
        _banner.SetActive(true);
        _bannerText.text = "🤖 LƯỢT CỦA AI";
        _bannerBg.color = new Color(0.6f, 0.3f, 0.2f, 0.9f);
    }
    
    public void ShowThinking()
    {
        _thinkingPopup?.SetActive(true);
        _dotCount = 0;
        _dotTimer = 0;
    }
    
    public void HideThinking()
    {
        _thinkingPopup?.SetActive(false);
    }
    
    public void Hide()
    {
        _banner?.SetActive(false);
        HideThinking();
    }
}
