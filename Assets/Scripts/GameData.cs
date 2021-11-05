using System.Collections;
using UnityEngine;
public class GameData : MonoBehaviour
{
    #region Constants
    public const string VerticalAxis = "Vertical";
    public const string HorizontalAxis = "Horizontal";
    #endregion
    public static GameData Instance;

    [SerializeField] private int[] _currentEnemyTypes = new int[4] { 8, 0, 0, 0 };
    [SerializeField] private int _scoreToNewPhase = 50;
    [SerializeField] private int _scoreStep = 50;
    [SerializeField] private float _timeToBonusSpawns;
    public int CurrentScore;
    public int CurrentRound;
    public int HighScore;
    private Spawner _spawner;
    private bool _canSpawn = true;
    private int _currentStage = 1;

    private void Awake()
    {
        LoadData();
        StartCoroutine(SpawnBonuses());
        _spawner = GetComponent<Spawner>();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {

        if (CurrentScore >= _scoreToNewPhase && _canSpawn)
        {
            int checkTotalCount = 0;
            for (int i = 0; i < _spawner.ActiveEnemiesOnScene.Count; i++)
            {
                checkTotalCount += _spawner.ActiveEnemiesOnScene[i].Count;
            }
            if (checkTotalCount < 16)
            {
                _scoreToNewPhase += _scoreStep;
                _spawner.EnableEnemiesOnScene(_currentEnemyTypes);
                _currentEnemyTypes = ChangeEnemyArray(_currentStage);
                _currentStage++;
                _canSpawn = false;
                StartCoroutine(DelayBetweenWaves());
            }
            
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

    /// <summary>
    /// –азрешает спавнить новую волну каждые 7 сек
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayBetweenWaves()
    {
        yield return new WaitForSeconds(7f);
        _canSpawn = true;
    }
    private int[] ChangeEnemyArray(int stage)
    {
        switch (stage)
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
        return new int[] { 1, 2, 3, 2 };
    }

    IEnumerator SpawnBonuses()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeToBonusSpawns);
            _spawner.EnableAmmoOnScene(1);
            _spawner.EnableHpsOnScene(1);
        }
    }
}
