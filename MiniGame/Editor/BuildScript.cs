using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class BuildScript
{
    [MenuItem("Build/Build Android APK")]
    public static void BuildAndroid()
    {
        BuildAndroidAPK();
    }

    public static void BuildAndroidAPK()
    {
        try
        {
            // Đảm bảo platform được set đúng
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                Debug.Log("Switching to Android platform...");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }
            
            // Tạo thư mục Build nếu chưa có
            string buildPath = Path.Combine(Application.dataPath, "..", "Build");
            if (!Directory.Exists(buildPath))
            {
                Directory.CreateDirectory(buildPath);
                Debug.Log("Created Build directory: " + buildPath);
            }

            // Đường dẫn output APK
            string apkPath = Path.Combine(buildPath, "game-o-quan.apk");
            
            // Lấy tất cả scenes
            string[] scenes = GetScenesInBuild();
            
            if (scenes == null || scenes.Length == 0)
            {
                Debug.LogError("No scenes found in Build Settings! Add scenes first.");
                return;
            }

            Debug.Log($"Building {scenes.Length} scene(s):");
            foreach (var scene in scenes)
            {
                Debug.Log("  - " + scene);
            }
            
            // Build options
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = apkPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            Debug.Log("Starting build...");
            Debug.Log("Output: " + apkPath);
            
            // Thực hiện build
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log("✅ Build successful!");
                Debug.Log("📦 APK: " + apkPath);
                Debug.Log("📊 Size: " + (report.summary.totalSize / 1024 / 1024) + " MB");
            }
            else
            {
                Debug.LogError("❌ Build failed: " + report.summary.result);
                if (report.summary.totalErrors > 0)
                {
                    Debug.LogError($"Total errors: {report.summary.totalErrors}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Build exception: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }
    
    private static string[] GetScenesInBuild()
    {
        var enabledScenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
            
        if (enabledScenes.Length == 0)
        {
            // Fallback: tìm scene trong Assets/Scenes
            string scenesPath = Path.Combine(Application.dataPath, "Scenes");
            if (Directory.Exists(scenesPath))
            {
                var allScenes = Directory.GetFiles(scenesPath, "*.unity", SearchOption.AllDirectories);
                
                if (allScenes.Length > 0)
                {
                    Debug.LogWarning("No scenes in Build Settings, using first scene found: " + allScenes[0]);
                    return new string[] { allScenes[0].Replace(Application.dataPath, "Assets") };
                }
            }
        }
        
        return enabledScenes;
    }
}
