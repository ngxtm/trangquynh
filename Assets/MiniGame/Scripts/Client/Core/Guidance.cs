using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Guidance : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Button closeButton;
    private Button _btnBgClose;

    public void Init()
    {
        if (instructionPanel == null)
            instructionPanel = transform.Find("bg").gameObject;

        if (titleText == null)
            titleText = transform.Find("content/Title/Text").GetComponent<Text>();

        if (contentText == null)
            contentText = transform.Find("content/Scroll View/Viewport/Content/Text").GetComponent<Text>();

        if (closeButton == null)
            closeButton = transform.Find("content/Image/close").GetComponent<Button>();
        if (_btnBgClose == null)
        _btnBgClose = instructionPanel.GetComponent<Button>();

        closeButton.onClick.RemoveAllListeners();
        _btnBgClose.onClick.RemoveAllListeners();
        // Gắn sự kiện nút đóng
        closeButton.onClick.AddListener(Hide);
        _btnBgClose.onClick.AddListener(Hide);

        // Mặc định ẩn
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // Nếu cần cập nhật nội dung động
    public void SetContent(string title, string content)
    {
        titleText.text = title;
        contentText.text = content;
    }
}
