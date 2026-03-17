using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionChecklistUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bagLabel;
    [SerializeField] private TextMeshProUGUI chopstickLabel;
    [SerializeField] private TextMeshProUGUI hoeLabel;
    [SerializeField] private Image bagTick;
    [SerializeField] private Image chopstickTick;
    [SerializeField] private Image hoeTick;

    private void Awake()
    {
        if (panelRoot == null)
        {
            panelRoot = gameObject;
        }

        SetTickVisible(bagTick, false);
        SetTickVisible(chopstickTick, false);
        SetTickVisible(hoeTick, false);
    }

    public void Initialize(string title, string bagText, string chopstickText, string hoeText)
    {
        if (titleText != null)
        {
            titleText.text = title;
        }

        if (bagLabel != null)
        {
            bagLabel.text = bagText;
        }

        if (chopstickLabel != null)
        {
            chopstickLabel.text = chopstickText;
        }

        if (hoeLabel != null)
        {
            hoeLabel.text = hoeText;
        }
    }

    public void Show()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
    }

    public void Hide()
    {
        SetTickVisible(bagTick, false);
        SetTickVisible(chopstickTick, false);
        SetTickVisible(hoeTick, false);

        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    public void UpdateChecklist(bool hasBag, bool hasChopstick, bool hasHoe)
    {
        SetTickVisible(bagTick, hasBag);
        SetTickVisible(chopstickTick, hasChopstick);
        SetTickVisible(hoeTick, hasHoe);
    }

    private void SetTickVisible(Image tick, bool isVisible)
    {
        if (tick != null)
        {
            tick.gameObject.SetActive(isVisible);
        }
    }
}
