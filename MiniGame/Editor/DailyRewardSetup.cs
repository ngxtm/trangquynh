using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class DailyRewardSetup : MonoBehaviour
{
    [MenuItem("LamGame/Setup Daily Rewards UI")]
    public static void SetupUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found!");
            return;
        }

        // Create Manager
        GameObject managerObj = new GameObject("DailyRewardManager");
        managerObj.AddComponent<DailyRewardManager>();

        // Create Popup Panel
        GameObject popup = new GameObject("DailyRewardPopup");
        popup.transform.SetParent(canvas.transform, false);
        popup.transform.SetAsLastSibling(); // Đặt lên trên cùng
        
        RectTransform popupRect = popup.AddComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.sizeDelta = Vector2.zero;
        
        Image popupBg = popup.AddComponent<Image>();
        popupBg.color = new Color(0, 0, 0, 0.8f);
        popupBg.raycastTarget = true; // Block clicks underneath
        
        // Add Canvas Group for better control
        CanvasGroup cg = popup.AddComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;

        // Create Content Panel
        GameObject content = new GameObject("ContentPanel");
        content.transform.SetParent(popup.transform, false);
        
        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.sizeDelta = new Vector2(600, 400);
        
        Image contentBg = content.AddComponent<Image>();
        contentBg.color = Color.white;

        // Title
        GameObject title = new GameObject("Title");
        title.transform.SetParent(content.transform, false);
        
        RectTransform titleRect = title.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0, -30);
        titleRect.sizeDelta = new Vector2(500, 50);
        
        TextMeshProUGUI titleText = title.AddComponent<TextMeshProUGUI>();
        titleText.text = "Daily Rewards";
        titleText.fontSize = 32;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.black;

        // Reward Container
        GameObject container = new GameObject("RewardContainer");
        container.transform.SetParent(content.transform, false);
        
        RectTransform containerRect = container.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.anchoredPosition = new Vector2(0, 20);
        containerRect.sizeDelta = new Vector2(550, 200);
        
        GridLayoutGroup grid = container.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(70, 80);
        grid.spacing = new Vector2(10, 10);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 7;

        // Claim Button
        GameObject claimBtn = new GameObject("ClaimButton");
        claimBtn.transform.SetParent(content.transform, false);
        
        RectTransform claimRect = claimBtn.AddComponent<RectTransform>();
        claimRect.anchorMin = new Vector2(0.5f, 0f);
        claimRect.anchorMax = new Vector2(0.5f, 0f);
        claimRect.anchoredPosition = new Vector2(0, 40);
        claimRect.sizeDelta = new Vector2(200, 50);
        
        Image claimBg = claimBtn.AddComponent<Image>();
        claimBg.color = new Color(0.2f, 0.8f, 0.2f);
        
        Button claimButton = claimBtn.AddComponent<Button>();
        
        GameObject claimText = new GameObject("Text");
        claimText.transform.SetParent(claimBtn.transform, false);
        
        RectTransform claimTextRect = claimText.AddComponent<RectTransform>();
        claimTextRect.anchorMin = Vector2.zero;
        claimTextRect.anchorMax = Vector2.one;
        claimTextRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI claimTMP = claimText.AddComponent<TextMeshProUGUI>();
        claimTMP.text = "CLAIM";
        claimTMP.fontSize = 24;
        claimTMP.alignment = TextAlignmentOptions.Center;
        claimTMP.color = Color.white;

        // Close Button
        GameObject closeBtn = new GameObject("CloseButton");
        closeBtn.transform.SetParent(content.transform, false);
        
        RectTransform closeRect = closeBtn.AddComponent<RectTransform>();
        closeRect.anchorMin = new Vector2(1f, 1f);
        closeRect.anchorMax = new Vector2(1f, 1f);
        closeRect.anchoredPosition = new Vector2(-20, -20);
        closeRect.sizeDelta = new Vector2(40, 40);
        
        Image closeBg = closeBtn.AddComponent<Image>();
        closeBg.color = new Color(0.8f, 0.2f, 0.2f);
        
        Button closeButton = closeBtn.AddComponent<Button>();
        
        GameObject closeText = new GameObject("Text");
        closeText.transform.SetParent(closeBtn.transform, false);
        
        RectTransform closeTextRect = closeText.AddComponent<RectTransform>();
        closeTextRect.anchorMin = Vector2.zero;
        closeTextRect.anchorMax = Vector2.one;
        closeTextRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI closeTMP = closeText.AddComponent<TextMeshProUGUI>();
        closeTMP.text = "X";
        closeTMP.fontSize = 24;
        closeTMP.alignment = TextAlignmentOptions.Center;
        closeTMP.color = Color.white;

        // Create Reward Item Prefab
        GameObject prefab = CreateRewardItemPrefab();

        // Setup DailyRewardUI
        DailyRewardUI ui = popup.AddComponent<DailyRewardUI>();
        ui.popupPanel = popup;
        ui.rewardContainer = container.transform;
        ui.rewardItemPrefab = prefab;
        ui.claimButton = claimButton;
        ui.closeButton = closeButton;
        ui.titleText = titleText;

        Debug.Log("✅ Daily Reward UI created!");
        Selection.activeGameObject = popup;
    }

    private static GameObject CreateRewardItemPrefab()
    {
        GameObject item = new GameObject("RewardItem");
        
        RectTransform rect = item.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(70, 80);
        
        Image bg = item.AddComponent<Image>();
        bg.color = new Color(0.9f, 0.9f, 0.9f);

        // Day Text
        GameObject dayObj = new GameObject("DayText");
        dayObj.transform.SetParent(item.transform, false);
        
        RectTransform dayRect = dayObj.AddComponent<RectTransform>();
        dayRect.anchorMin = new Vector2(0.5f, 1f);
        dayRect.anchorMax = new Vector2(0.5f, 1f);
        dayRect.anchoredPosition = new Vector2(0, -15);
        dayRect.sizeDelta = new Vector2(60, 20);
        
        TextMeshProUGUI dayText = dayObj.AddComponent<TextMeshProUGUI>();
        dayText.text = "Day 1";
        dayText.fontSize = 14;
        dayText.alignment = TextAlignmentOptions.Center;
        dayText.color = Color.black;

        // Reward Text
        GameObject rewardObj = new GameObject("RewardText");
        rewardObj.transform.SetParent(item.transform, false);
        
        RectTransform rewardRect = rewardObj.AddComponent<RectTransform>();
        rewardRect.anchorMin = new Vector2(0.5f, 0.5f);
        rewardRect.anchorMax = new Vector2(0.5f, 0.5f);
        rewardRect.anchoredPosition = new Vector2(0, -5);
        rewardRect.sizeDelta = new Vector2(60, 40);
        
        TextMeshProUGUI rewardText = rewardObj.AddComponent<TextMeshProUGUI>();
        rewardText.text = "50";
        rewardText.fontSize = 18;
        rewardText.alignment = TextAlignmentOptions.Center;
        rewardText.color = new Color(1f, 0.7f, 0f);

        // Checkmark
        GameObject check = new GameObject("Checkmark");
        check.transform.SetParent(item.transform, false);
        
        RectTransform checkRect = check.AddComponent<RectTransform>();
        checkRect.anchorMin = new Vector2(1f, 1f);
        checkRect.anchorMax = new Vector2(1f, 1f);
        checkRect.anchoredPosition = new Vector2(-5, -5);
        checkRect.sizeDelta = new Vector2(20, 20);
        
        TextMeshProUGUI checkText = check.AddComponent<TextMeshProUGUI>();
        checkText.text = "✓";
        checkText.fontSize = 16;
        checkText.alignment = TextAlignmentOptions.Center;
        checkText.color = Color.green;
        
        check.SetActive(false);

        return item;
    }
}
