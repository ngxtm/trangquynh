using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TutorialSetup : EditorWindow
{
    [MenuItem("Tools/Setup Tutorial UI")]
    public static void SetupTutorialUI()
    {
        // Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in scene!");
            return;
        }

        // Create Tutorial Panel
        GameObject tutorialPanel = new GameObject("TutorialPanel");
        tutorialPanel.transform.SetParent(canvas.transform, false);
        
        Image panelImage = tutorialPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = tutorialPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        // Create Content Panel
        GameObject content = new GameObject("Content");
        content.transform.SetParent(tutorialPanel.transform, false);
        
        Image contentImage = content.AddComponent<Image>();
        contentImage.color = Color.white;
        
        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.pivot = new Vector2(0.5f, 0.5f);
        contentRect.sizeDelta = new Vector2(600, 400);

        // Create Tutorial Text
        GameObject textObj = new GameObject("TutorialText");
        textObj.transform.SetParent(content.transform, false);
        
        Text text = textObj.AddComponent<Text>();
        text.text = "Tutorial text here...";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 28;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.3f);
        textRect.anchorMax = new Vector2(0.9f, 0.9f);
        textRect.sizeDelta = Vector2.zero;

        // Create Next Button
        GameObject nextBtn = CreateButton("NextButton", "Tiếp theo →", content.transform);
        RectTransform nextRect = nextBtn.GetComponent<RectTransform>();
        nextRect.anchorMin = new Vector2(0.7f, 0.05f);
        nextRect.anchorMax = new Vector2(0.95f, 0.2f);
        nextRect.sizeDelta = Vector2.zero;

        // Create Skip Button
        GameObject skipBtn = CreateButton("SkipButton", "Bỏ qua", content.transform);
        RectTransform skipRect = skipBtn.GetComponent<RectTransform>();
        skipRect.anchorMin = new Vector2(0.05f, 0.05f);
        skipRect.anchorMax = new Vector2(0.25f, 0.2f);
        skipRect.sizeDelta = Vector2.zero;

        // Create Highlight Overlay
        GameObject overlay = new GameObject("HighlightOverlay");
        overlay.transform.SetParent(tutorialPanel.transform, false);
        
        Image overlayImage = overlay.AddComponent<Image>();
        overlayImage.color = new Color(0, 0, 0, 0.6f);
        
        RectTransform overlayRect = overlay.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;
        overlay.SetActive(false);

        // Create TutorialManager GameObject
        GameObject managerObj = new GameObject("TutorialManager");
        TutorialManager manager = managerObj.AddComponent<TutorialManager>();
        
        // Use reflection to set private fields
        var type = typeof(TutorialManager);
        type.GetField("tutorialPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, tutorialPanel);
        type.GetField("tutorialText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, text);
        type.GetField("highlightOverlay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, overlay);
        type.GetField("nextButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, nextBtn.GetComponent<Button>());
        type.GetField("skipButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(manager, skipBtn.GetComponent<Button>());

        tutorialPanel.SetActive(false);

        Debug.Log("✅ Tutorial UI setup complete!");
        EditorUtility.SetDirty(manager);
        Selection.activeGameObject = managerObj;
    }

    private static GameObject CreateButton(string name, string text, Transform parent)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        
        Image btnImage = btnObj.AddComponent<Image>();
        btnImage.color = new Color(0.2f, 0.6f, 1f);
        
        Button btn = btnObj.AddComponent<Button>();
        
        GameObject btnText = new GameObject("Text");
        btnText.transform.SetParent(btnObj.transform, false);
        
        Text txt = btnText.AddComponent<Text>();
        txt.text = text;
        txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.fontSize = 20;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = Color.white;
        
        RectTransform txtRect = btnText.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.sizeDelta = Vector2.zero;
        
        return btnObj;
    }
}
