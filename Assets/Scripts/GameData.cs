using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    #region Constants
    public const string VERTICAL_AXIS = "Vertical";
    public const string HORIZONTAL_AXIS = "Horizontal";
    #endregion
    public static GameData Instance;

    public int CurrentScore;
    public int CurrentRound;
    // два массива 8 0 0 0 - текущая ситуация и массив 0 3 3 2 - цель
    public int HighScore;
    Spawner spawner;
    [SerializeField] int[] currentEnemyTypes = new int[4] {8,0,0,0 };
    [SerializeField] int[] maxEnemyTypes = new int[4] { 0, 3, 3, 2 };
    [SerializeField] int scoreToNewPhase = 50;
    [SerializeField] int scoreStep = 50;
    [SerializeField] float timeToBonusSpawns;
    private void Awake()
    {
        LoadData();
        StartCoroutine(SpawnBonuses());
        spawner = GetComponent<Spawner>();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if(CurrentScore>=scoreToNewPhase)
        {
            ChangeEnemyArray();
            scoreToNewPhase += scoreStep;
            spawner.EnableEnemiesOnScene(currentEnemyTypes);
        }
    }
    private void ChangeEnemyArray()
    {
        if(currentEnemyTypes!= maxEnemyTypes)
        {
            for (int i = 0; i < currentEnemyTypes.Length; i++)
            {
                if (currentEnemyTypes[i] > maxEnemyTypes[i])
                {
                    if (i + 1 < currentEnemyTypes.Length)
                    {
                        currentEnemyTypes[i + 1]++;
                        currentEnemyTypes[i]--;
                    }

                }
            }
        }          
    }
    IEnumerator SpawnBonuses()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeToBonusSpawns);
            spawner.EnableAmmoOnScene(1);
            spawner.EnableHpsOnScene(1);
        }
        

    }
    public void AddScore(int score)
    {
        CurrentScore += score;
    }
    public void SaveData()
    {
        if (CurrentScore > HighScore)
        {
            PlayerPrefs.SetInt("Score", CurrentScore);
        }
    }
    public void LoadData()
    {
        HighScore = PlayerPrefs.GetInt("Score");
    }
}
