using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string VOLUME_PREF_KEY = "MasterVolumePref";

    void Awake()
    {
        if (volumeSlider == null)
        {
            volumeSlider = FindFirstObjectByType<Slider>(FindObjectsInactive.Include);
        }
    }

    void Start()
    {
        if (volumeSlider == null)
        {
            Debug.LogError($"{nameof(VolumeManager)} on {name} could not find a Slider reference.", this);
            return;
        }

        if (audioMixer == null)
        {
            Debug.LogError($"{nameof(VolumeManager)} on {name} is missing an AudioMixer reference.", this);
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 0.75f);

        volumeSlider.SetValueWithoutNotify(savedVolume);
        SetVolume(savedVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    public void SetVolume(float sliderValue)
    {
        float volume = Mathf.Clamp(sliderValue, 0.0001f, 1f);

        float dbVolume = Mathf.Log10(volume) * 20;

        bool result = audioMixer.SetFloat("Audio", dbVolume);

        if (!result) Debug.LogError("Không tìm thấy tham số mang tên 'Audio' trong Mixer!");

        PlayerPrefs.SetFloat(VOLUME_PREF_KEY, sliderValue);
    }
}