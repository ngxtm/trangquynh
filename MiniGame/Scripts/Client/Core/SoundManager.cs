using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sources & Settings")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public float defaultFadeTime = 1f;

    [Header("Audio Clips")]
    public List<AudioClip> bgmClips;  // 0: Loading, 1: Game
    public List<AudioClip> sfxClips;  // SFX list

    private Dictionary<string, AudioClip> _bgmMap;
    private Dictionary<string, AudioClip> _sfxMap;

    private Coroutine _currentFade;

    // PlayerPrefs keys
    private const string MasterVolKey     = "MasterVolume";
    private const string MusicVolKey      = "MusicVolume";
    private const string SFXVolKey        = "SFXVolume";
    private const string MusicEnabledKey  = "MusicEnabled";
    private const string SFXEnabledKey    = "SFXEnabled";

    // Volume values [0..1]
    private float _masterVol = 1f;
    private float _musicVol  = 1f;
    private float _sfxVol    = 1f;

    // Enable flags
    private bool _musicEnabled = true;
    private bool _sfxEnabled   = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build lookup tables
        _bgmMap = new Dictionary<string, AudioClip>();
        foreach (var clip in bgmClips)
            _bgmMap[clip.name] = clip;

        _sfxMap = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
            _sfxMap[clip.name] = clip;

        // Load saved volumes and enabled flags
        _masterVol    = PlayerPrefs.GetFloat(MasterVolKey,    1f);
        _musicVol     = PlayerPrefs.GetFloat(MusicVolKey,     1f);
        _sfxVol       = PlayerPrefs.GetFloat(SFXVolKey,       1f);
        _musicEnabled = PlayerPrefs.GetInt(MusicEnabledKey,   1) == 1;
        _sfxEnabled   = PlayerPrefs.GetInt(SFXEnabledKey,     1) == 1;

        ApplyVolumes();
    }

    #region BGM Controls

    /// <summary>Play BGM theo tên clip</summary>
    public void PlayMusic(string clipName, bool loop = true, float fadeTime = -1f)
    {
        if (!_musicEnabled) return;

        if (!_bgmMap.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"[SoundManager] BGM '{clipName}' không tìm thấy!");
            return;
        }
        if (_currentFade != null)
            StopCoroutine(_currentFade);

        _currentFade = StartCoroutine(FadeMusic(clip, loop, fadeTime > 0 ? fadeTime : defaultFadeTime));
    }

    public void StopMusic()
    {
        if (_currentFade != null)
            StopCoroutine(_currentFade);
        musicSource.Stop();
    }

    public void PauseMusic(bool pause)
    {
        if (pause) musicSource.Pause();
        else       musicSource.UnPause();
    }

    private IEnumerator FadeMusic(AudioClip newClip, bool loop, float duration)
    {
        float startVol = _musicEnabled ? (_musicVol * _masterVol) : 0f;
        // fade out
        for (float t = duration; t >= 0; t -= Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, startVol, t / duration);
            yield return null;
        }
        musicSource.Stop();

        if (!_musicEnabled) yield break;

        // đổi clip
        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.Play();

        // fade in
        for (float t = 0; t <= duration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, startVol, t / duration);
            yield return null;
        }
        musicSource.volume = startVol;
        _currentFade = null;
    }

    #endregion

    #region SFX Controls

    /// <summary>Play one-shot SFX theo tên clip.</summary>
    public void PlaySFX(string clipName, float volumeScale = 1f)
    {
        if (!_sfxEnabled) return;

        if (!_sfxMap.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"[SoundManager] SFX '{clipName}' không tìm thấy!");
            return;
        }
        sfxSource.PlayOneShot(clip, volumeScale * _sfxVol * _masterVol);
    }

    #endregion

    #region Volume & Enable Controls

    public void SetMasterVolume(float volume)
    {
        _masterVol = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MasterVolKey, _masterVol);
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVol = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(MusicVolKey, _musicVol);
        musicSource.volume = _musicEnabled ? (_musicVol * _masterVol) : 0f;
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVol = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFXVolKey, _sfxVol);
    }

    public void EnableMusic(bool enabled)
    {
        _musicEnabled = enabled;
        PlayerPrefs.SetInt(MusicEnabledKey, enabled ? 1 : 0);
        musicSource.volume = _musicEnabled ? (_musicVol * _masterVol) : 0f;
    }

    public void EnableSFX(bool enabled)
    {
        _sfxEnabled = enabled;
        PlayerPrefs.SetInt(SFXEnabledKey, enabled ? 1 : 0);
    }

    private void ApplyVolumes()
    {
        musicSource.volume = _musicEnabled ? (_musicVol * _masterVol) : 0f;
        // SFX volume applied on PlayOneShot if _sfxEnabled
    }
    
    public float GetMusicVolume() => _musicVol;
    public float GetSFXVolume() => _sfxVol;

    #endregion
}
