using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] AudioSource[] bgmSource;
    [SerializeField] AudioSource[] sfxSource;

    //[Header("Audio Clip")]
    //public AudioClip[] background;
    //public AudioClip[] sfx;

    public static AudioManager instance;

    public const string VOLUME_KEY = "volume";
    public const string BGM_KEY = "bgmVolume";
    public const string SFX_KEY = "sfxVolume";


    private void Awake() 
    {
        
    }

    private void Start() 
    {
        if (instance == null)
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

    private void LoadVolume() //volume saved in VolumeSetting script
    {
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        audioMixer.SetFloat(VolumeSettings.MIXER_VOLUME, Mathf.Log10(volume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_BGM, Mathf.Log10(bgmVolume) * 20);
        audioMixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }

    public void PlaySFX(int soundToPlay)
    {
        
        if (soundToPlay < sfxSource.Length)
        {
                sfxSource[soundToPlay].Play();
        }
        
    }

    public void PlayBGM(int musicToPlay)
    {
        if(!bgmSource[musicToPlay].isPlaying)
        {
            StopBGM();

            if(musicToPlay < bgmSource.Length)
            {
                bgmSource[musicToPlay].Play();
            }
        }
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgmSource.Length; i++)
        {
            bgmSource[i].Stop();
        }
    }


}
    

