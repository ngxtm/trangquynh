using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AchievementSetup : EditorWindow
{
    [MenuItem("Tools/Setup Achievement System")]
    public static void SetupAchievementSystem()
    {
        Debug.Log("🏆 Setting up Achievement System...");

        // Create AchievementManager
        GameObject managerObj = GameObject.Find("AchievementManager");
        if (managerObj == null)
        {
            managerObj = new GameObject("AchievementManager");
            managerObj.AddComponent<AchievementManager>();
            Debug.Log("✓ Created AchievementManager");
        }

        // Find Canvas
        Canvas[] canvases = Object.FindObjectsOfType<Canvas>();
        Canvas canvas = canvases.Length > 0 ? canvases[0] : null;
        
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found! Please open the game scene");
            return;
        }

        // Create Achievement Popup
        CreateAchievementPopup(canvas);

        Debug.Log("✅ Achievement System setup complete!");
    }

    private static void CreateAchievementPopup(Canvas canvas)
    {
        // Create popup panel
        GameObject popup = new GameObject("AchievementPopup");
        popup.transform.SetParent(canvas.transform, false);

        Image popupImage = popup.AddComponent<Image>();
        popupImage.color = new Color(0, 0, 0, 0.8f);

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchorMin = new Vector2(0.5f, 1f);
        popupRect.anchorMax = new Vector2(0.5f, 1f);
        popupRect.pivot = new Vector2(0.5f, 1f);
        popupRect.anchoredPosition = new Vector2(0, -10);
        popupRect.sizeDelta = new Vector2(400, 120);

        // Create title text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(popup.transform, false);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "Achievement Unlocked!";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 24;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.84f, 0f); // Gold

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.6f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.sizeDelta = Vector2.zero;

        // Create description text
        GameObject descObj = new GameObject("DescriptionText");
        descObj.transform.SetParent(popup.transform, false);

        Text descText = descObj.AddComponent<Text>();
        descText.text = "Achievement description";
        descText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        descText.fontSize = 18;
        descText.alignment = TextAnchor.MiddleCenter;
        descText.color = Color.white;

        RectTransform descRect = descObj.GetComponent<RectTransform>();
        descRect.anchorMin = new Vector2(0, 0.3f);
        descRect.anchorMax = new Vector2(1, 0.6f);
        descRect.sizeDelta = Vector2.zero;

        // Create reward text
        GameObject rewardObj = new GameObject("RewardText");
        rewardObj.transform.SetParent(popup.transform, false);

        Text rewardText = rewardObj.AddComponent<Text>();
        rewardText.text = "Reward: 100 coins";
        rewardText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        rewardText.fontSize = 16;
        rewardText.alignment = TextAnchor.MiddleCenter;
        rewardText.color = new Color(0.5f, 1f, 0.5f); // Light green

        RectTransform rewardRect = rewardObj.GetComponent<RectTransform>();
        rewardRect.anchorMin = new Vector2(0, 0);
        rewardRect.anchorMax = new Vector2(1, 0.3f);
        rewardRect.sizeDelta = Vector2.zero;

        // Add AchievementPopup component
        AchievementPopup popupComp = popup.AddComponent<AchievementPopup>();
        
        // Set references using reflection
        var type = typeof(AchievementPopup);
        type.GetField("popupPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(popupComp, popup);
        type.GetField("titleText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(popupComp, titleText);
        type.GetField("descriptionText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(popupComp, descText);
        type.GetField("rewardText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(popupComp, rewardText);

        popup.SetActive(true);
        EditorUtility.SetDirty(popupComp);
        
        Debug.Log("✓ Created Achievement Popup");
    }
}
