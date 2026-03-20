using UnityEngine;
using System.IO;

public class LogFileErrorBuilded : MonoBehaviour
{
   string logPath;

    void Awake()
    {
        // Chỉ đăng ký log khi KHÔNG chạy trong Editor
        if (!Application.isEditor)
        {
            logPath = Path.Combine(Application.persistentDataPath, "error_log.txt");
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }
            Application.logMessageReceived += HandleLog;
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
       // if (type == LogType.Error || type == LogType.Exception)
       // {
            string logEntry = $"{System.DateTime.Now:yyyy-MM-dd HH:mm:ss} [{type}] {logString}\n{stackTrace}\n";
            File.AppendAllText(logPath, logEntry);
       // }
    }

    void OnDestroy()
    {
        if (!Application.isEditor)
            Application.logMessageReceived -= HandleLog;
    }
}
