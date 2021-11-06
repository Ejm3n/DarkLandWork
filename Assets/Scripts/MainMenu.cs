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

    /// <summary>
    /// переход на игровую сцену
    /// </summary>
    public void StartGame()
    {
        LevelLoader.Instance.LoadScene(1);
    }

    /// <summary>
    /// смена канвасов на настройки
    /// </summary>
    public void OpenSettings()
    {
        ChangeStates(_mainMenu, _settings);
    }

    /// <summary>
    /// смена канвасов из настроек обратно в меню
    /// </summary>
    public void CloseSettings()
    {
        ChangeStates(_settings, _mainMenu);
    }

    /// <summary>
    /// смена канвасов на окно выбора перед выходом из игры
    /// </summary>
    public void PressedExit()
    {
        ChangeStates(_mainMenu, _checkBox);
    }

    /// <summary>
    /// смена канвасов из диалогового окна обратно в главное меню
    /// </summary>
    public void CloseCheckBox()
    {
        ChangeStates(_checkBox, _mainMenu);
    }

    /// <summary>
    /// выход из игры
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// проигрывание звука нажатия кнопки
    /// </summary>
    public void PressButtonSound()
    {
        SoundManagerDemo.Instance.ButtonPressSound();
    }

    /// <summary>
    /// смена канвасов
    /// </summary>
    /// <param name="ToOff"></param>
    /// <param name="ToOn"></param>
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
