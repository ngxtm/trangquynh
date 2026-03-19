using UnityEngine;

[CreateAssetMenu(fileName = "GeminiConfig", menuName = "Game/Gemini Config")]
public class GeminiConfig : ScriptableObject
{
    public string apiKey = "";
    public string model = "gemini-1.5-flash";
    public int timeoutSeconds = 10;
    public float temperature = 0.7f;
}
