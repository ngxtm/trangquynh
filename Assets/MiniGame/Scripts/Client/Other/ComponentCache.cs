using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper class to cache component references and avoid GetComponent calls
/// </summary>
public class ComponentCache : MonoBehaviour
{
    // Cached components
    private Transform _cachedTransform;
    private RectTransform _cachedRectTransform;
    private Image _cachedImage;
    private Text _cachedText;
    private Button _cachedButton;
    
    public new Transform transform
    {
        get
        {
            if (_cachedTransform == null)
                _cachedTransform = base.transform;
            return _cachedTransform;
        }
    }
    
    public RectTransform rectTransform
    {
        get
        {
            if (_cachedRectTransform == null)
                _cachedRectTransform = GetComponent<RectTransform>();
            return _cachedRectTransform;
        }
    }
    
    public Image image
    {
        get
        {
            if (_cachedImage == null)
                _cachedImage = GetComponent<Image>();
            return _cachedImage;
        }
    }
    
    public Text text
    {
        get
        {
            if (_cachedText == null)
                _cachedText = GetComponent<Text>();
            return _cachedText;
        }
    }
    
    public Button button
    {
        get
        {
            if (_cachedButton == null)
                _cachedButton = GetComponent<Button>();
            return _cachedButton;
        }
    }
}
