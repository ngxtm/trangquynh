using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoOptimize : EditorWindow
{
    [MenuItem("Tools/Auto Optimize Game")]
    public static void OptimizeGame()
    {
        Debug.Log("🚀 Starting Auto Optimization...");
        
        int improvements = 0;
        
        improvements += OptimizeTextures();
        improvements += OptimizeAudio();
        improvements += OptimizePlayerSettings();
        
        Debug.Log($"✅ Optimization complete! {improvements} improvements made.");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private static int OptimizeTextures()
    {
        Debug.Log("📸 Optimizing textures...");
        int count = 0;
        
        string[] texturePaths = Directory.GetFiles("Assets/Art", "*.png", SearchOption.AllDirectories);
        
        foreach (string path in texturePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) continue;
            
            bool changed = false;
            
            // Android settings
            TextureImporterPlatformSettings androidSettings = importer.GetPlatformTextureSettings("Android");
            if (!androidSettings.overridden || androidSettings.format != TextureImporterFormat.ASTC_6x6)
            {
                androidSettings.overridden = true;
                androidSettings.maxTextureSize = 1024;
                androidSettings.format = TextureImporterFormat.ASTC_6x6;
                androidSettings.compressionQuality = 50;
                importer.SetPlatformTextureSettings(androidSettings);
                changed = true;
            }
            
            // General settings
            if (importer.maxTextureSize > 2048)
            {
                importer.maxTextureSize = 2048;
                changed = true;
            }
            
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }
            
            if (changed)
            {
                importer.SaveAndReimport();
                count++;
                Debug.Log($"  ✓ Optimized: {Path.GetFileName(path)}");
            }
        }
        
        Debug.Log($"  📸 Optimized {count} textures");
        return count;
    }
    
    private static int OptimizeAudio()
    {
        Debug.Log("🔊 Optimizing audio...");
        int count = 0;
        
        string[] audioPaths = Directory.GetFiles("Assets/Audio", "*.wav", SearchOption.AllDirectories);
        
        foreach (string path in audioPaths)
        {
            AudioImporter importer = AssetImporter.GetAtPath(path) as AudioImporter;
            if (importer == null) continue;
            
            bool changed = false;
            
            AudioImporterSampleSettings settings = importer.defaultSampleSettings;
            
            if (settings.loadType != AudioClipLoadType.CompressedInMemory)
            {
                settings.loadType = AudioClipLoadType.CompressedInMemory;
                changed = true;
            }
            
            if (settings.compressionFormat != AudioCompressionFormat.Vorbis)
            {
                settings.compressionFormat = AudioCompressionFormat.Vorbis;
                settings.quality = 0.7f;
                changed = true;
            }
            
            if (changed)
            {
                importer.defaultSampleSettings = settings;
                importer.SaveAndReimport();
                count++;
                Debug.Log($"  ✓ Optimized: {Path.GetFileName(path)}");
            }
        }
        
        Debug.Log($"  🔊 Optimized {count} audio files");
        return count;
    }
    
    private static int OptimizePlayerSettings()
    {
        Debug.Log("⚙️ Optimizing player settings...");
        int count = 0;
        
        // Stripping level
        if (PlayerSettings.stripEngineCode == false)
        {
            PlayerSettings.stripEngineCode = true;
            count++;
        }
        
        // Managed stripping level
        if (PlayerSettings.GetManagedStrippingLevel(BuildTargetGroup.Android) != ManagedStrippingLevel.High)
        {
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.High);
            count++;
        }
        
        // IL2CPP optimization
        if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.Mono2x)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            count++;
        }
        
        // API level
        if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel24)
        {
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            count++;
        }
        
        Debug.Log($"  ⚙️ Applied {count} player settings");
        return count;
    }
}
