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
    [SerializeField] List<Enemy> enemies;
    public List<List<GameObject>> DeactivatedEnemiesOnScene = new List<List<GameObject>>();
    public List<List<GameObject>> ActiveEnemiesOnScene = new List<List<GameObject>>();
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] List<Bonuses> bonuses;
    [SerializeField] List<List<GameObject>> bonusesOnScene = new List<List<GameObject>>();

    [SerializeField] Transform[] BonusesSpawnPoints;
    int currentPoint = 0;
    private void Awake()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            DeactivatedEnemiesOnScene.Add(new List<GameObject>());
            ActiveEnemiesOnScene.Add(new List<GameObject>());
            for (int j = 0; j < enemies[i].Count; j++)
            {
                GameObject enemyObj = Instantiate(enemies[i].prefab);
                enemyObj.GetComponent<EnemyController>().TypeNum = i;
                DeactivatedEnemiesOnScene[i].Add(enemyObj);
                enemyObj.SetActive(false);
            }
        }
        for (int i = 0; i < bonuses.Count; i++)
        {
            bonusesOnScene.Add(new List<GameObject>());
            for (int j = 0; j < bonuses[i].Count; j++)
            {
                GameObject bonusObj = Instantiate(bonuses[i].prefab);
                bonusesOnScene[i].Add(bonusObj);
                bonusObj.SetActive(false);
            }
        }
        //EnableBonusesOnScene(2);
    }
    public void EnableEnemiesOnScene(int[] typesTOACtivate)
    {

        for (int i = 0; i < typesTOACtivate.Length; i++)
        {
            for (int j = 0; j < typesTOACtivate[i]; j++)
            {
                GameObject enemy = DeactivatedEnemiesOnScene[i][DeactivatedEnemiesOnScene[i].Count - 1];
                DeactivatedEnemiesOnScene[i].RemoveAt(DeactivatedEnemiesOnScene[i].Count - 1);
                enemy.transform.position = SpawnPoints[currentPoint].position;
                enemy.GetComponent<Health>().Revive();
                enemy.SetActive(true);

                ActiveEnemiesOnScene[i].Add(enemy);

                if (currentPoint < SpawnPoints.Length - 1)
                {
                    currentPoint++;
                }
                else
                {
                    currentPoint = 0;
                }
            }
        }
    }
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
    public void EnableHpsOnScene(int count)
    {

        if(CheckSummaryOfBonuses())
        {
            for (int j = 0; j < count; j++)
            {
                for(int i = 0;i<bonusesOnScene[0].Count;i++)
                {
                    if (!bonusesOnScene[0][i].activeInHierarchy)
                    {
                        bonusesOnScene[0][i].transform.position = BonusesSpawnPoints[currentPoint].position;
                        bonusesOnScene[0][i].SetActive(true);
                        break;
                    }

                    
                }
                if (currentPoint < BonusesSpawnPoints.Length - 1)
                {
                    currentPoint++;
                }
                else
                {
                    currentPoint = 0;
                }
            }
        }

    }
    public void EnableAmmoOnScene(int count)
    {
        if(CheckSummaryOfBonuses())
        {
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < bonusesOnScene[0].Count; i++)
                {
                    if (!bonusesOnScene[1][i].activeInHierarchy)
                    {
                        bonusesOnScene[1][i].transform.position = BonusesSpawnPoints[currentPoint].position;
                        bonusesOnScene[1][i].SetActive(true);
                        break;
                    }

                    
                }
                if (currentPoint < BonusesSpawnPoints.Length - 1)
                {
                    currentPoint++;
                }
                else
                {
                    currentPoint = 0;
                }
            }
        }
        
    }
    private bool CheckSummaryOfBonuses()
    {
        if (GameObject.FindGameObjectsWithTag("HpBonus").Length + GameObject.FindGameObjectsWithTag("AmmoBonus").Length >= SpawnPoints.Length - 1)
        {
            return false;
        }
        else
            return true;
    }
}