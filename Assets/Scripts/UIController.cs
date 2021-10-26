using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] Text ammoCount;
    [SerializeField] Text hpText;
    [SerializeField] CanvasGroup gameCanvas;
    [SerializeField] CanvasGroup pauseCanvas;
    [SerializeField] CanvasGroup confirmCanvas;
    [SerializeField] CanvasGroup deathCanvas;
    [SerializeField] Text currentScore;
    [SerializeField] Animator[] heartAnims;
    [SerializeField] Text bestScore;
    GameData gd;
    PlayerWeapon pw;
    Health playerHealth;
    private void Awake()
    {

        gd = FindObjectOfType<GameData>();
        pw = FindObjectOfType<PlayerWeapon>();
        playerHealth= pw.gameObject.GetComponent<Health>();
        Time.timeScale = 1;
        ChangeStates(pauseCanvas,gameCanvas);
        ChangeStates(confirmCanvas, gameCanvas);
        ChangeStates(deathCanvas, gameCanvas);
        bestScore.text = gd.HighScore.ToString();
    }
    // Update is called once per frame
    void Update()
    {

        currentScore.text = gd.CurrentScore.ToString();
        ammoCount.text = pw.ammo.ToString();

        SetHearts(playerHealth.CurrentHP);
        if (playerHealth.CurrentHP <= 0)
        {
            gd.SaveData();
            ChangeStates(gameCanvas, deathCanvas);
            Time.timeScale = 0;
        }
            
    }
    public void OnPauseClick()
    {
        Time.timeScale = 0;
        ChangeStates(gameCanvas, pauseCanvas);
    }
    public void OnResume()
    {
        Time.timeScale = 1;
        ChangeStates(pauseCanvas, gameCanvas);
    }
    public void OnRestart()
    {
        LevelLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);        
    }
    public void OpenConfirmMenu()
    {
        ChangeStates(pauseCanvas, confirmCanvas);

    }
    public void BackToPauseMenu()
    {
        ChangeStates( confirmCanvas, pauseCanvas);
    }
    public void ToMainMenu()
    {
        LevelLoader.Instance.LoadScene(0);
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
    void SetHearts(int hp)
    {
        for(int i = 0;i<hp;i++)
        {
            heartAnims[i].SetBool("IsFull", true);
        }
        for(int j = heartAnims.Length-1;j>=hp;j--)
        {
            heartAnims[j].SetBool("IsFull", false);
        }
    }
}
