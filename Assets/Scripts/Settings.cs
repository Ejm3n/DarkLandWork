using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public Slider musicSlider;
    public Slider soundSlider;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        FindObjectOfType<SoundManagerDemo>().FindSliders();
    }
}
