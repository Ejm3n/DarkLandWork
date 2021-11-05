using DigitalRuby.SoundManagerNamespace;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private CanvasGroup _settings;
    [SerializeField] private CanvasGroup _checkBox;

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
        ChangeStates(_mainMenu, _settings);
    }

    public void CloseSettings()
    {
        ChangeStates(_settings, _mainMenu);
    }

    public void PressedExit()
    {
        ChangeStates(_mainMenu, _checkBox);
    }

    public void CloseCheckBox()
    {
        ChangeStates(_checkBox, _mainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PressButtonSound()
    {
        SoundManagerDemo.Instance.ButtonPressSound();
    }

    private void ChangeStates(CanvasGroup ToOff, CanvasGroup ToOn)
    {
        SetCanvasGroup(ToOff, false);
        SetCanvasGroup(ToOn, true);
    }

    private void SetCanvasGroup(CanvasGroup cg, bool what)
    {
        if (what)
            cg.alpha = 1;
        else
            cg.alpha = 0;
        cg.interactable = what;
        cg.blocksRaycasts = what;
    }
}
