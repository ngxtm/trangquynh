using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class LevelSystemSetup : EditorWindow
{
    [MenuItem("Tools/Setup Level System UI")]
    public static void SetupLevelSystemUI()
    {
        Debug.Log("🚀 Setting up Level System UI...");

        // Find Canvas in active scene
        Canvas[] canvases = Object.FindObjectsOfType<Canvas>();
        Canvas canvas = null;
        
        if (canvases.Length > 0)
        {
            canvas = canvases[0];
            Debug.Log($"Found Canvas: {canvas.name}");
        }
        
        if (canvas == null)
        {
            Debug.LogError("❌ No Canvas found! Please open the game scene (SampleScene)");
            return;
        }

        // Create Managers
        CreateManagers();

        // Create Level Display
        CreateLevelDisplay(canvas);

        // Create Currency Display
        CreateCurrencyDisplay(canvas);

        Debug.Log("✅ Level System UI setup complete!");
    }

    private static void CreateManagers()
    {
        // Create LevelSystem
        GameObject levelSystemObj = GameObject.Find("LevelSystem");
        if (levelSystemObj == null)
        {
            levelSystemObj = new GameObject("LevelSystem");
            LevelSystem levelSystem = levelSystemObj.AddComponent<LevelSystem>();
            Debug.Log("✓ Created LevelSystem");
        }

        // Create CurrencyManager
        GameObject currencyManagerObj = GameObject.Find("CurrencyManager");
        if (currencyManagerObj == null)
        {
            currencyManagerObj = new GameObject("CurrencyManager");
            CurrencyManager currencyManager = currencyManagerObj.AddComponent<CurrencyManager>();
            Debug.Log("✓ Created CurrencyManager");
        }
    }

    private static void CreateLevelDisplay(Canvas canvas)
    {
        // Create Level Display Panel
        GameObject levelDisplay = new GameObject("LevelDisplay");
        levelDisplay.transform.SetParent(canvas.transform, false);

        Image panelImage = levelDisplay.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.6f);

        RectTransform panelRect = levelDisplay.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(0, 1);
        panelRect.pivot = new Vector2(0, 1);
        panelRect.anchoredPosition = new Vector2(10, -10);
        panelRect.sizeDelta = new Vector2(200, 80);

        // Create Level Text
        GameObject levelTextObj = new GameObject("LevelText");
        levelTextObj.transform.SetParent(levelDisplay.transform, false);

        Text levelText = levelTextObj.AddComponent<Text>();
        levelText.text = "Lv.1";
        levelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        levelText.fontSize = 28;
        levelText.fontStyle = FontStyle.Bold;
        levelText.alignment = TextAnchor.MiddleCenter;
        levelText.color = Color.white;

        RectTransform levelTextRect = levelTextObj.GetComponent<RectTransform>();
        levelTextRect.anchorMin = new Vector2(0, 0.6f);
        levelTextRect.anchorMax = new Vector2(1, 1);
        levelTextRect.sizeDelta = Vector2.zero;

        // Create XP Bar Background
        GameObject xpBarBg = new GameObject("XPBarBG");
        xpBarBg.transform.SetParent(levelDisplay.transform, false);

        Image xpBarBgImage = xpBarBg.AddComponent<Image>();
        xpBarBgImage.color = new Color(0.2f, 0.2f, 0.2f);

        RectTransform xpBarBgRect = xpBarBg.GetComponent<RectTransform>();
        xpBarBgRect.anchorMin = new Vector2(0.1f, 0.3f);
        xpBarBgRect.anchorMax = new Vector2(0.9f, 0.5f);
        xpBarBgRect.sizeDelta = Vector2.zero;

        // Create XP Bar
        GameObject xpBarObj = new GameObject("XPBar");
        xpBarObj.transform.SetParent(xpBarBg.transform, false);

        Image xpBar = xpBarObj.AddComponent<Image>();
        xpBar.color = new Color(1f, 0.84f, 0f); // Gold
        xpBar.type = Image.Type.Filled;
        xpBar.fillMethod = Image.FillMethod.Horizontal;
        xpBar.fillAmount = 0.3f;

        RectTransform xpBarRect = xpBarObj.GetComponent<RectTransform>();
        xpBarRect.anchorMin = Vector2.zero;
        xpBarRect.anchorMax = Vector2.one;
        xpBarRect.sizeDelta = Vector2.zero;

        // Create XP Text
        GameObject xpTextObj = new GameObject("XPText");
        xpTextObj.transform.SetParent(levelDisplay.transform, false);

        Text xpText = xpTextObj.AddComponent<Text>();
        xpText.text = "0/100";
        xpText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        xpText.fontSize = 16;
        xpText.alignment = TextAnchor.MiddleCenter;
        xpText.color = Color.white;

        RectTransform xpTextRect = xpTextObj.GetComponent<RectTransform>();
        xpTextRect.anchorMin = new Vector2(0, 0);
        xpTextRect.anchorMax = new Vector2(1, 0.3f);
        xpTextRect.sizeDelta = Vector2.zero;

        // Add LevelDisplay component
        LevelDisplay levelDisplayComp = levelDisplay.AddComponent<LevelDisplay>();
        
        // Set references using reflection
        var type = typeof(LevelDisplay);
        type.GetField("levelText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(levelDisplayComp, levelText);
        type.GetField("xpText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(levelDisplayComp, xpText);
        type.GetField("xpBar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(levelDisplayComp, xpBar);

        EditorUtility.SetDirty(levelDisplayComp);
        Debug.Log("✓ Created Level Display");
    }

    private static void CreateCurrencyDisplay(Canvas canvas)
    {
        // Create Currency Display Panel
        GameObject currencyDisplay = new GameObject("CurrencyDisplay");
        currencyDisplay.transform.SetParent(canvas.transform, false);

        Image panelImage = currencyDisplay.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.6f);

        RectTransform panelRect = currencyDisplay.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(-10, -10);
        panelRect.sizeDelta = new Vector2(150, 80);

        // Create Coins Text
        GameObject coinsTextObj = new GameObject("CoinsText");
        coinsTextObj.transform.SetParent(currencyDisplay.transform, false);

        Text coinsText = coinsTextObj.AddComponent<Text>();
        coinsText.text = "💰 0";
        coinsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        coinsText.fontSize = 22;
        coinsText.fontStyle = FontStyle.Bold;
        coinsText.alignment = TextAnchor.MiddleCenter;
        coinsText.color = new Color(1f, 0.84f, 0f); // Gold

        RectTransform coinsTextRect = coinsTextObj.GetComponent<RectTransform>();
        coinsTextRect.anchorMin = new Vector2(0, 0.5f);
        coinsTextRect.anchorMax = new Vector2(1, 1);
        coinsTextRect.sizeDelta = Vector2.zero;

        // Create Gems Text
        GameObject gemsTextObj = new GameObject("GemsText");
        gemsTextObj.transform.SetParent(currencyDisplay.transform, false);

        Text gemsText = gemsTextObj.AddComponent<Text>();
        gemsText.text = "💎 0";
        gemsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        gemsText.fontSize = 22;
        gemsText.fontStyle = FontStyle.Bold;
        gemsText.alignment = TextAnchor.MiddleCenter;
        gemsText.color = new Color(0f, 1f, 1f); // Cyan

        RectTransform gemsTextRect = gemsTextObj.GetComponent<RectTransform>();
        gemsTextRect.anchorMin = new Vector2(0, 0);
        gemsTextRect.anchorMax = new Vector2(1, 0.5f);
        gemsTextRect.sizeDelta = Vector2.zero;

        // Add CurrencyDisplay component
        CurrencyDisplay currencyDisplayComp = currencyDisplay.AddComponent<CurrencyDisplay>();
        
        // Set references using reflection
        var type = typeof(CurrencyDisplay);
        type.GetField("coinsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(currencyDisplayComp, coinsText);
        type.GetField("gemsText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(currencyDisplayComp, gemsText);

        EditorUtility.SetDirty(currencyDisplayComp);
        Debug.Log("✓ Created Currency Display");
    }
}
