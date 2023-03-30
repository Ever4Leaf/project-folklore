using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource bgmSource;


    [Header("Audio Clip")]
    public AudioClip background;

    private void Start() 
    {
        bgmSource.clip = background;
        bgmSource.Play();
    }
}
    

