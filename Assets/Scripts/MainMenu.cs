using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DigitalRuby.SoundManagerNamespace;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    [SerializeField] CanvasGroup mainMenu;
    [SerializeField] CanvasGroup settings;
    [SerializeField] CanvasGroup checkBox;
    public Slider musicSlider;
    public Slider soundSlider;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void StartGame()
    {
        LevelLoader.Instance.LoadScene(1);
    }
    public void OpenSettings()
    {
        ChangeStates(mainMenu, settings);
    }
    public void CloseSettings()
    {
        ChangeStates(settings, mainMenu);
    }
    public void PressedExit()
    {
        ChangeStates(mainMenu, checkBox);
    }
    public void CloseCheckBox()
    {
        ChangeStates(checkBox, mainMenu);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PressButtonSound()
    {
        SoundManagerDemo.Instance.ButtonPressSound();
    }
    void ChangeStates(CanvasGroup ToOff, CanvasGroup ToOn)
    {
        SetCanvasGroup(ToOff, false);
        SetCanvasGroup(ToOn, true);
    }
    void SetCanvasGroup(CanvasGroup cg, bool what)
    {
        if (what)
            cg.alpha = 1;
        else
            cg.alpha = 0;
        cg.interactable = what;
        cg.blocksRaycasts = what;
    }
}
