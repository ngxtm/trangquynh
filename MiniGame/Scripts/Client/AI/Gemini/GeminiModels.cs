using System;

[Serializable]
public class GeminiRequest
{
    public GeminiContent[] contents;
    public GeminiGenConfig generationConfig;
}

[Serializable]
public class GeminiContent
{
    public string role;
    public GeminiPart[] parts;
}

[Serializable]
public class GeminiPart
{
    public string text;
}

[Serializable]
public class GeminiGenConfig
{
    public float temperature;
    public int maxOutputTokens;
}

[Serializable]
public class GeminiResponse
{
    public GeminiCandidate[] candidates;
}

[Serializable]
public class GeminiCandidate
{
    public GeminiContent content;
}

[Serializable]
public class GeminiMoveResult
{
    public int cellIndex;
    public int direction;
}
