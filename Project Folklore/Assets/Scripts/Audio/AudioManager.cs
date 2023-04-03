using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource walkSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip walk;

    public static AudioManager instance;

    public const string VOLUME_KEY = "volume";
    public const string BGM_KEY = "bgmVolume";
    public const string SFX_KEY = "sfxVolume";


    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();
    }

    private void Start() 
    {
        bgmSource.clip = background;
        bgmSource.Play();
    }

    private void LoadVolume() //volume saved in VolumeSetting script
    {
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(VolumeSettings.MIXER_VOLUME, Mathf.Log10(volume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_BGM, Mathf.Log10(bgmVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }

    public void ContohSFX()
    {
        walkSource.clip = walk;
        walkSource.PlayOneShot(walk);
    }

    
}
    

