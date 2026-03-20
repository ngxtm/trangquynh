using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI component for selecting AI difficulty
/// </summary>
public class DifficultySelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown difficultyDropdown;
    [SerializeField] private Toggle aiEnabledToggle;
    [SerializeField] private TMP_Dropdown aiPlayerDropdown;

    private const string PREF_DIFFICULTY = "AIDifficulty";
    private const string PREF_AI_ENABLED = "AIEnabled";
    private const string PREF_AI_PLAYER = "AIPlayer";

    private void Start()
    {
        LoadSettings();
        SetupListeners();
    }

    private void LoadSettings()
    {
        // Load difficulty
        int difficulty = PlayerPrefs.GetInt(PREF_DIFFICULTY, 1); // Default: Medium
        if (difficultyDropdown != null)
            difficultyDropdown.value = difficulty;

        // Load AI enabled
        bool aiEnabled = PlayerPrefs.GetInt(PREF_AI_ENABLED, 0) == 1;
        if (aiEnabledToggle != null)
            aiEnabledToggle.isOn = aiEnabled;

        // Load AI player
        int aiPlayer = PlayerPrefs.GetInt(PREF_AI_PLAYER, 1); // Default: P2
        if (aiPlayerDropdown != null)
            aiPlayerDropdown.value = aiPlayer;
    }

    private void SetupListeners()
    {
        if (difficultyDropdown != null)
            difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);

        if (aiEnabledToggle != null)
            aiEnabledToggle.onValueChanged.AddListener(OnAIEnabledChanged);

        if (aiPlayerDropdown != null)
            aiPlayerDropdown.onValueChanged.AddListener(OnAIPlayerChanged);
    }

    private void OnDifficultyChanged(int value)
    {
        PlayerPrefs.SetInt(PREF_DIFFICULTY, value);
        PlayerPrefs.Save();

        if (AIManager.Instance != null)
            AIManager.Instance.SetAIDifficulty((AIDifficulty)value);

        Debug.Log($"Difficulty changed to: {(AIDifficulty)value}");
    }

    private void OnAIEnabledChanged(bool enabled)
    {
        PlayerPrefs.SetInt(PREF_AI_ENABLED, enabled ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"AI {(enabled ? "enabled" : "disabled")}");
    }

    private void OnAIPlayerChanged(int value)
    {
        PlayerPrefs.SetInt(PREF_AI_PLAYER, value);
        PlayerPrefs.Save();

        Debug.Log($"AI player set to: {(value == 0 ? "P1" : "P2")}");
    }

    public static AIDifficulty GetSavedDifficulty()
    {
        return (AIDifficulty)PlayerPrefs.GetInt(PREF_DIFFICULTY, 1);
    }

    public static bool IsAIEnabled()
    {
        return PlayerPrefs.GetInt(PREF_AI_ENABLED, 0) == 1;
    }

    public static PlayerTurn GetAIPlayer()
    {
        return PlayerPrefs.GetInt(PREF_AI_PLAYER, 1) == 0 ? PlayerTurn.P1 : PlayerTurn.P2;
    }
}
