using System.Collections;
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
    [SerializeField] int[] currentEnemyTypes = new int[4] { 8, 0, 0, 0 };
    [SerializeField] int[] maxEnemyTypes = new int[4] { 1, 2, 3, 2 };
    [SerializeField] int scoreToNewPhase = 50;
    [SerializeField] int scoreStep = 50;
    [SerializeField] float timeToBonusSpawns;
    bool canSpawn = true;
    int currentStage = 1;
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

        if (CurrentScore >= scoreToNewPhase && canSpawn)
        {
            int checkTotalCount = 0;
            for (int i = 0; i < spawner.ActiveEnemiesOnScene.Count; i++)
            {
                checkTotalCount += spawner.ActiveEnemiesOnScene[i].Count;
            }
            if (checkTotalCount < 16)
            {

                scoreToNewPhase += scoreStep;
                spawner.EnableEnemiesOnScene(currentEnemyTypes);
                currentEnemyTypes = ChangeEnemyArray(currentStage);
                currentStage++;
                canSpawn = false;
                StartCoroutine(DelayBetweenWaves());
            }
            
        }
    }
    IEnumerator DelayBetweenWaves()
    {
        yield return new WaitForSeconds(7f);
        canSpawn = true;

    }
    private int[] ChangeEnemyArray(int stage)
    {
        //if (currentEnemyTypes != maxEnemyTypes)
        //{
        //    if (currentEnemyTypes[currentAdding] != maxEnemyTypes[currentAdding])
        //    {
        //        BalanceEnemyTypes();
        //    }
        //    else
        //    {
        //        currentAdding++;
        //        BalanceEnemyTypes();
        //    }
        //    for(int i = 0; i< currentEnemyTypes.Length;i++)
        //    {
        //        Debug.Log("curTypes(" + i+") = " + currentEnemyTypes[i]);
        //    }
        //}
        switch(stage)
        {
            case 0: return new int[] { 8, 0, 0, 0 };
            case 1: return new int[] { 7, 1, 0, 0 };
            case 2: return new int[] { 6, 2, 0, 0 };
            case 3: return new int[] { 5, 2, 1, 0 };
            case 4: return new int[] { 4, 2, 2, 0 };
            case 5: return new int[] { 3, 2, 3, 0 };
            case 6: return new int[] { 2, 2, 3, 1 };
            case 7: return new int[] { 1, 2, 3, 2 };
        }
        return new int[] { 1,2,3,2};
    }   

    IEnumerator SpawnBonuses()
    {
        while (true)
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
