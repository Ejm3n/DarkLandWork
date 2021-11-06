using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    [SerializeField] private Slider musicSlider;
    [SerializeField]private Slider soundSlider;

    public Slider MusicSlider { get => musicSlider; set => musicSlider = value; }
    public Slider SoundSlider { get => soundSlider; set => soundSlider = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<SoundManagerDemo>().FindSliders();
    }
}
