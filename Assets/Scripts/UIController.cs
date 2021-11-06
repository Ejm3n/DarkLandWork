using DigitalRuby.SoundManagerNamespace;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _ammoCount;
    [SerializeField] private Text _hpText;
    [SerializeField] private CanvasGroup _gameCanvas;
    [SerializeField] private CanvasGroup _pauseCanvas;
    [SerializeField] private CanvasGroup _confirmCanvas;
    [SerializeField] private CanvasGroup _deathCanvas;
    [SerializeField] private CanvasGroup _settingsCanvas;
    [SerializeField] private Text _currentScore;
    [SerializeField] private Animator[] _heartAnims;
    [SerializeField] private Text _bestScore;
    [SerializeField] private Text _deathCurrentScore;
    [SerializeField] private Text _deathBestScore;
    [SerializeField] PostProcessVolume _postProcessVolume;
    private GameData _gameData;
    private PlayerWeapon _playerWeapon;
    private Health _playerHealth;

    private void Awake()
    {
        _gameData = FindObjectOfType<GameData>();
        _playerWeapon = FindObjectOfType<PlayerWeapon>();
        _playerHealth = _playerWeapon.gameObject.GetComponent<Health>();
        Time.timeScale = 1;
        ChangeStates(_pauseCanvas, _gameCanvas);
        ChangeStates(_confirmCanvas, _gameCanvas);
        ChangeStates(_deathCanvas, _gameCanvas);
        ChangeStates(_settingsCanvas, _gameCanvas);
        _bestScore.text = _gameData.HighScore.ToString();
    }

    private void Update()
    {

        _currentScore.text = _gameData.CurrentScore.ToString();
        _ammoCount.text = _playerWeapon.Ammo.ToString();
        SetHearts(_playerHealth.CurrentHP);

        if (_playerHealth.CurrentHP == 1)
        {
            ChangeVignette(true);
        }
        else if (_playerHealth.CurrentHP <= 0)
        {
            _gameData.SaveData();
            _deathCurrentScore.text = _currentScore.text;
            _deathBestScore.text = _bestScore.text;
            ChangeStates(_gameCanvas, _deathCanvas);
            Time.timeScale = 0;
        }
        else
        {
            ChangeVignette(false);
        }
    }

    /// <summary>
    /// проигрывание звука нажатия кнопки
    /// </summary>
    public void PressButtonSound()
    {
        SoundManagerDemo.Instance.ButtonPressSound();
    }

    /// <summary>
    /// нажатие на паузу, смена игрового канваса на паузу
    /// </summary>
    public void OnPauseClick()
    {
        Time.timeScale = 0;
        ChangeStates(_gameCanvas, _pauseCanvas);
    }

    /// <summary>
    /// возобновить игру, сменить канвас паузы на игровой   
    /// </summary>
    public void OnResume()
    {
        Time.timeScale = 1;
        ChangeStates(_pauseCanvas, _gameCanvas);
    }

    /// <summary>
    /// перезагрузить уровень
    /// </summary>
    public void OnRestart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        LevelLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// открыть канвас выбора, закрыть канвас паузы
    /// </summary>
    public void OpenConfirmMenu()
    {
        ChangeStates(_pauseCanvas, _confirmCanvas);

    }

    /// <summary>
    /// открыть канвас паузы, закрыть канвас выбора
    /// </summary>
    public void BackToPauseMenu()
    {
        ChangeStates(_confirmCanvas, _pauseCanvas);
        ChangeStates(_settingsCanvas, _pauseCanvas);
    }

    /// <summary>
    /// сохранить результат и выйти в меню
    /// </summary>
    public void ToMainMenu()
    {
        _gameData.SaveData();
        LevelLoader.Instance.LoadScene(0);
    }

    /// <summary>
    /// открыть настройки
    /// </summary>
    public void ToSettings()
    {
        ChangeStates(_pauseCanvas, _settingsCanvas);
    }

    /// <summary>
    /// сменить текущий канвас
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

    private void SetHearts(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            _heartAnims[i].SetBool("IsFull", true);
        }
        for (int j = _heartAnims.Length - 1; j >= hp; j--)
        {
            _heartAnims[j].SetBool("IsFull", false);
        }
    }

    private void ChangeVignette(bool what)
    {
        if (_postProcessVolume.profile.TryGetSettings<Vignette>(out var vignette))
            vignette.active = what;
    }
}
