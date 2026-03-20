using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Shows popup when achievement is unlocked
/// </summary>
public class AchievementPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text rewardText;
    [SerializeField] private float displayDuration = 3f;

    private void Start()
    {
        if (popupPanel) popupPanel.SetActive(false);

        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.OnAchievementUnlocked += ShowPopup;
        }
    }

    private void OnDestroy()
    {
        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.OnAchievementUnlocked -= ShowPopup;
        }
    }

    private void ShowPopup(AchievementData achievement)
    {
        if (titleText)
            titleText.text = achievement.title;

        if (descriptionText)
            descriptionText.text = achievement.description;

        if (rewardText)
        {
            var sb = StringBuilderCache.Acquire(50);
            sb.Append("Reward: ");
            if (achievement.rewardCoins > 0)
            {
                sb.Append(achievement.rewardCoins);
                sb.Append(" coins");
            }
            if (achievement.rewardGems > 0)
            {
                if (achievement.rewardCoins > 0) sb.Append(" + ");
                sb.Append(achievement.rewardGems);
                sb.Append(" gems");
            }
            rewardText.text = StringBuilderCache.GetStringAndRelease(sb);
        }

        if (popupPanel)
        {
            popupPanel.SetActive(true);
            StartCoroutine(HideAfterDelay());
        }

        SoundManager.Instance?.PlaySFX(Config.SFX.START_GAME);
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (popupPanel)
            popupPanel.SetActive(false);
    }
}
