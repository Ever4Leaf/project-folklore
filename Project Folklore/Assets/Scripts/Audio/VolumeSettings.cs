using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    public const string MIXER_VOLUME = "volume";
    public const string MIXER_BGM = "bgm";
    public const string MIXER_SFX = "sfx";

    private void Awake() 
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume); 
    }

    private void Start() 
    {
        volumeSlider.value = PlayerPrefs.GetFloat(AudioManager.VOLUME_KEY, 1f);
        BGMSlider.value = PlayerPrefs.GetFloat(AudioManager.BGM_KEY, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }

    private void OnDisable() 
    {
        PlayerPrefs.SetFloat(AudioManager.VOLUME_KEY, volumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.BGM_KEY, BGMSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(MIXER_VOLUME, Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat(MIXER_BGM, Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);
    }
}
