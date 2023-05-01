using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource, _fxSource;
    [SerializeField] private AudioClip[] _fxClips;

    public float MusicVolume { get => _musicSource.volume; }

    public float FXVolume { get => _fxSource.volume; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PlaySound(string name)
    {
        var clips = _fxClips.Where(c => c.name.StartsWith(name)).ToList();
        PlaySound(clips[Random.Range(0, clips.Count)]);
    }

    public void PlaySound(AudioClip clip)
    {
        //if(!_fxSource.isPlaying)
        _fxSource.pitch = Random.Range(.95f, 1f);
        _fxSource.PlayOneShot(clip);
    }

    public void MusicVolumeChanged(Slider slider)
    {
        _musicSource.volume = slider.value;
    }

    public void FXVolumeChanged(Slider slider)
    {
        _fxSource.volume = slider.value;
        PlaySound(_fxSource.clip);
    }
}
