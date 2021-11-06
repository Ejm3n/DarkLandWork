using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public int Count;
    public GameObject prefab;
}

[System.Serializable]
public class Bonuses
{
    public int Count;
    public GameObject prefab;
}

public class Spawner : MonoBehaviour
{
    //ДВА двумерных массива и живых и мертвых инстансов
    [SerializeField]private List<Enemy> _enemies;   
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private List<Bonuses> _bonuses;
    [SerializeField] private List<List<GameObject>> _bonusesOnScene = new List<List<GameObject>>();
    [SerializeField] private Transform[] _bonusesSpawnPoints;
    public List<List<GameObject>> DeactivatedEnemiesOnScene = new List<List<GameObject>>();
    public List<List<GameObject>> ActiveEnemiesOnScene = new List<List<GameObject>>();
    private int _currentBonusPoint = 0;
    private int _currentEnemySpawnPoint = 0;

    private void Awake()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            DeactivatedEnemiesOnScene.Add(new List<GameObject>());
            ActiveEnemiesOnScene.Add(new List<GameObject>());
            for (int j = 0; j < _enemies[i].Count; j++)
            {
                GameObject enemyObj = Instantiate(_enemies[i].prefab);
                enemyObj.GetComponent<EnemyController>().TypeNum = i;
                DeactivatedEnemiesOnScene[i].Add(enemyObj);
                enemyObj.SetActive(false);
            }
        }
        for (int i = 0; i < _bonuses.Count; i++)
        {
            _bonusesOnScene.Add(new List<GameObject>());
            for (int j = 0; j < _bonuses[i].Count; j++)
            {
                GameObject bonusObj = Instantiate(_bonuses[i].prefab);
                _bonusesOnScene[i].Add(bonusObj);
                bonusObj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// включить зомби, каких и сколько включать передается в массиве
    /// </summary>
    /// <param name="typesTOACtivate"></param>
    public void EnableEnemiesOnScene(int[] typesTOACtivate)
    {
        for (int i = 0; i < typesTOACtivate.Length; i++)
        {
            for (int j = 0; j < typesTOACtivate[i]; j++)
            {
                GameObject enemy = DeactivatedEnemiesOnScene[i][DeactivatedEnemiesOnScene[i].Count - 1];
                DeactivatedEnemiesOnScene[i].RemoveAt(DeactivatedEnemiesOnScene[i].Count - 1);
                enemy.transform.position = _spawnPoints[_currentEnemySpawnPoint].position;
                enemy.GetComponent<Health>().Revive();
                enemy.SetActive(true);

                ActiveEnemiesOnScene[i].Add(enemy);

                if (_currentEnemySpawnPoint < _spawnPoints.Length - 1)
                {
                    _currentEnemySpawnPoint++;
                }
                else
                {
                    _currentEnemySpawnPoint = 0;
                }
            }
        }
    }

    /// <summary>
    /// при смерти врага выносим из двумерного массива активных и добавляем в деактивированных
    /// </summary>
    /// <param name="deadEnemy"></param>
    public void DisableEnemy(GameObject deadEnemy)
    {
        EnemyController ec = deadEnemy.GetComponent<EnemyController>();
        int type = ec.TypeNum;
        for (int i = 0; i < ActiveEnemiesOnScene[type].Count; i++)
        {
            if (!ActiveEnemiesOnScene[type][i].activeInHierarchy)
            {
                ActiveEnemiesOnScene[type].RemoveAt(i);
                break;
            }
        }
        DeactivatedEnemiesOnScene[type].Add(deadEnemy);
    }

    /// <summary>
    /// добавляем бонус на хп на сцену
    /// </summary>
    /// <param name="count"></param>
    public void EnableHpsOnScene(int count)
    {
        if(CheckSummaryOfBonuses())
        {
            for (int j = 0; j < count; j++)
            {
                for(int i = 0;i<_bonusesOnScene[0].Count;i++)
                {
                    if (!_bonusesOnScene[0][i].activeInHierarchy)
                    {
                        _bonusesOnScene[0][i].transform.position = _bonusesSpawnPoints[_currentBonusPoint].position;
                        _bonusesOnScene[0][i].SetActive(true);
                        break;
                    }                  
                }
                if (_currentBonusPoint < _bonusesSpawnPoints.Length - 1)
                {
                    _currentBonusPoint++;
                }
                else
                {
                    _currentBonusPoint = 0;
                }
            }
        }
    }

    /// <summary>
    /// добавляем бонус пуль на сцену
    /// </summary>
    /// <param name="count"></param>
    public void EnableAmmoOnScene(int count)
    {
        if(CheckSummaryOfBonuses())
        {
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < _bonusesOnScene[0].Count; i++)
                {
                    if (!_bonusesOnScene[1][i].activeInHierarchy)
                    {
                        _bonusesOnScene[1][i].transform.position = _bonusesSpawnPoints[_currentBonusPoint].position;
                        _bonusesOnScene[1][i].SetActive(true);
                        break;
                    }

                    
                }
                if (_currentBonusPoint < _bonusesSpawnPoints.Length - 1)
                {
                    _currentBonusPoint++;
                }
                else
                {
                    _currentBonusPoint = 0;
                }
            }
        }
    }

    private bool CheckSummaryOfBonuses()
    {
        if (GameObject.FindGameObjectsWithTag("HpBonus").Length + GameObject.FindGameObjectsWithTag("AmmoBonus").Length >= _spawnPoints.Length - 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}