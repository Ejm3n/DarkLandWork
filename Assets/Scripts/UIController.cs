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

    public void PressButtonSound()
    {
        SoundManagerDemo.Instance.ButtonPressSound();
    }

    public void OnPauseClick()
    {
        Time.timeScale = 0;
        ChangeStates(_gameCanvas, _pauseCanvas);
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        ChangeStates(_pauseCanvas, _gameCanvas);
    }

    public void OnRestart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        LevelLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenConfirmMenu()
    {
        ChangeStates(_pauseCanvas, _confirmCanvas);

    }

    public void BackToPauseMenu()
    {
        ChangeStates(_confirmCanvas, _pauseCanvas);
        ChangeStates(_settingsCanvas, _pauseCanvas);
    }

    public void ToMainMenu()
    {
        _gameData.SaveData();
        LevelLoader.Instance.LoadScene(0);
    }

    public void ToSettings()
    {
        ChangeStates(_pauseCanvas, _settingsCanvas);
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
