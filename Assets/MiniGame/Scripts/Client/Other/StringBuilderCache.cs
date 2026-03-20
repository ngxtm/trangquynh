using System.Text;

/// <summary>
/// Cache StringBuilder to reduce allocations
/// </summary>
public static class StringBuilderCache
{
    private const int MAX_BUILDER_SIZE = 360;
    
    [System.ThreadStatic]
    private static StringBuilder _cachedInstance;
    
    public static StringBuilder Acquire(int capacity = 16)
    {
        if (capacity <= MAX_BUILDER_SIZE)
        {
            StringBuilder sb = _cachedInstance;
            if (sb != null && capacity <= sb.Capacity)
            {
                _cachedInstance = null;
                sb.Clear();
                return sb;
            }
        }
        
        return new StringBuilder(capacity);
    }
    
    public static string GetStringAndRelease(StringBuilder sb)
    {
        string result = sb.ToString();
        Release(sb);
        return result;
    }
    
    public static void Release(StringBuilder sb)
    {
        if (sb.Capacity <= MAX_BUILDER_SIZE)
        {
            _cachedInstance = sb;
        }
    }
}
