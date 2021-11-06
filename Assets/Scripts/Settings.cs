using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    [SerializeField] private Slider _musicSlider;
    [SerializeField]private Slider _soundSlider;

    public Slider MusicSlider { get => _musicSlider; set => _musicSlider = value; }
    public Slider SoundSlider { get => _soundSlider; set => _soundSlider = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<SoundManagerDemo>().FindSliders();
    }
}
