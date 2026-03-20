using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays player currency (coins and gems)
/// </summary>
public class CurrencyDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text gemsText;

    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged += UpdateCoins;
            CurrencyManager.Instance.OnGemsChanged += UpdateGems;
            UpdateCoins(CurrencyManager.Instance.GetCoins());
            UpdateGems(CurrencyManager.Instance.GetGems());
        }
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCoinsChanged -= UpdateCoins;
            CurrencyManager.Instance.OnGemsChanged -= UpdateGems;
        }
    }

    private void UpdateCoins(int amount)
    {
        if (coinsText)
        {
            var sb = StringBuilderCache.Acquire(10);
            sb.Append(amount);
            coinsText.text = StringBuilderCache.GetStringAndRelease(sb);
        }
    }

    private void UpdateGems(int amount)
    {
        if (gemsText)
        {
            var sb = StringBuilderCache.Acquire(10);
            sb.Append(amount);
            gemsText.text = StringBuilderCache.GetStringAndRelease(sb);
        }
    }
}
