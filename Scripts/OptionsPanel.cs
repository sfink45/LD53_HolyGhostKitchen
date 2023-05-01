using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _fxSlider;

    private void Awake()
    {
        _musicSlider.value = AudioManager.Instance.MusicVolume;
        _fxSlider.value = AudioManager.Instance.FXVolume;
    }
}
