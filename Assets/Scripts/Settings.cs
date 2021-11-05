using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public Slider MusicSlider;
    public Slider SoundSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<SoundManagerDemo>().FindSliders();
    }
}
