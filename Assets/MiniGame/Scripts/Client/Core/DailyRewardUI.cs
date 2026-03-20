using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI for daily reward popup
/// </summary>
public class DailyRewardUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public Transform rewardContainer;
    public GameObject rewardItemPrefab;
    public Button claimButton;
    public Button closeButton;
    public TextMeshProUGUI titleText;

    private List<GameObject> rewardItems = new List<GameObject>();

    private void Start()
    {
        if (claimButton != null)
            claimButton.onClick.AddListener(OnClaimClicked);
        
        if (closeButton != null)
            closeButton.onClick.AddListener(Hide);

        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.Instance.OnRewardClaimed += OnRewardClaimed;
        }

        // Delay check to ensure manager is ready
        Invoke(nameof(CheckAndShow), 0.5f);
    }

    private void CheckAndShow()
    {
        if (DailyRewardManager.Instance != null && DailyRewardManager.Instance.CanClaimToday())
        {
            Show();
            Debug.Log("🎁 Daily Reward popup shown!");
        }
        else
        {
            Hide();
            Debug.Log("⏭️ Daily Reward already claimed today");
        }
    }

    public void Show()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            popupPanel.transform.SetAsLastSibling(); // Ensure on top
        }

        RefreshUI();
    }

    public void Hide()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void RefreshUI()
    {
        if (DailyRewardManager.Instance == null) return;

        // Clear old items
        foreach (var item in rewardItems)
        {
            if (item != null)
                Destroy(item);
        }
        rewardItems.Clear();

        // Create reward items
        var rewards = DailyRewardManager.Instance.GetAllRewards();
        int currentStreak = DailyRewardManager.Instance.GetCurrentStreak();

        for (int i = 0; i < rewards.Count; i++)
        {
            CreateRewardItem(rewards[i], i == currentStreak);
        }

        // Update claim button
        bool canClaim = DailyRewardManager.Instance.CanClaimToday();
        if (claimButton != null)
        {
            claimButton.interactable = canClaim;
            var btnText = claimButton.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = canClaim ? "CLAIM" : "CLAIMED";
            }
        }

        // Update title
        if (titleText != null)
        {
            titleText.text = $"Daily Rewards - Day {currentStreak + 1}";
        }
    }

    private void CreateRewardItem(DailyRewardData reward, bool isCurrent)
    {
        if (rewardItemPrefab == null || rewardContainer == null) return;

        GameObject item = Instantiate(rewardItemPrefab, rewardContainer);
        rewardItems.Add(item);

        // Set day number
        var dayText = item.transform.Find("DayText")?.GetComponent<TextMeshProUGUI>();
        if (dayText != null)
            dayText.text = $"Day {reward.day}";

        // Set reward text
        var rewardText = item.transform.Find("RewardText")?.GetComponent<TextMeshProUGUI>();
        if (rewardText != null)
            rewardText.text = reward.GetRewardText();

        // Highlight current day
        var bg = item.GetComponent<Image>();
        if (bg != null)
        {
            bg.color = isCurrent ? new Color(1f, 0.9f, 0.5f, 1f) : Color.white;
        }

        // Show claimed checkmark
        var checkmark = item.transform.Find("Checkmark")?.gameObject;
        if (checkmark != null)
            checkmark.SetActive(reward.claimed);
    }

    private void OnClaimClicked()
    {
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.Instance.ClaimReward();
        }
    }

    private void OnRewardClaimed(DailyRewardData reward)
    {
        RefreshUI();
        
        // Show celebration effect
        Debug.Log($"🎉 Claimed: {reward.GetRewardText()}");
        
        // Auto close after 2 seconds
        Invoke(nameof(Hide), 2f);
    }

    private void OnDestroy()
    {
        if (DailyRewardManager.Instance != null)
        {
            DailyRewardManager.Instance.OnRewardClaimed -= OnRewardClaimed;
        }
    }
}
