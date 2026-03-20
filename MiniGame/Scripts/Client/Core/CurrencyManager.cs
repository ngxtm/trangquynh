using UnityEngine;
using System;

/// <summary>
/// Manages in-game currencies (Coins and Gems)
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private int coins = 0;
    private int gems = 0;

    public event Action<int> OnCoinsChanged;
    public event Action<int> OnGemsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadCurrency();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
        SaveCurrency();
        Debug.Log($"💰 +{amount} coins! Total: {coins}");
    }

    public bool SpendCoins(int amount)
    {
        if (coins < amount)
        {
            Debug.Log("❌ Not enough coins!");
            return false;
        }

        coins -= amount;
        OnCoinsChanged?.Invoke(coins);
        SaveCurrency();
        Debug.Log($"💸 -{amount} coins. Remaining: {coins}");
        return true;
    }

    public void AddGems(int amount)
    {
        gems += amount;
        OnGemsChanged?.Invoke(gems);
        SaveCurrency();
        Debug.Log($"💎 +{amount} gems! Total: {gems}");
    }

    public bool SpendGems(int amount)
    {
        if (gems < amount)
        {
            Debug.Log("❌ Not enough gems!");
            return false;
        }

        gems -= amount;
        OnGemsChanged?.Invoke(gems);
        SaveCurrency();
        Debug.Log($"💎 -{amount} gems. Remaining: {gems}");
        return true;
    }

    public int GetCoins() => coins;
    public int GetGems() => gems;

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("PlayerCoins", coins);
        PlayerPrefs.SetInt("PlayerGems", gems);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        coins = PlayerPrefs.GetInt("PlayerCoins", 0);
        gems = PlayerPrefs.GetInt("PlayerGems", 0);
        OnCoinsChanged?.Invoke(coins);
        OnGemsChanged?.Invoke(gems);
    }

    public void ResetCurrency()
    {
        coins = 0;
        gems = 0;
        SaveCurrency();
    }
}
