using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] List<Bullets> bullets;
    [SerializeField] Transform firePoint;
    [SerializeField] List<GameObject> bulletsOnScene;
    EnemyController ec;

    void Awake()
    {
        ec = GetComponent<EnemyController>();
        for (int i = 0; i < bullets.Count; i++)
        {
            for (int j = 0; j < bullets[i].Count; j++)
            {
                GameObject enemyBullet = Instantiate(bullets[i].prefab);
                bulletsOnScene.Add(enemyBullet);
                enemyBullet.SetActive(false);
            }
        }
    }

    public void Shoot()
    {        
        bool freeBullet = false;
        for (int i = 0; i < bulletsOnScene.Count; i++)
        {
            if (!bulletsOnScene[i].activeInHierarchy)
            {
                bulletsOnScene[i].transform.position = firePoint.position;
                bulletsOnScene[i].transform.rotation = transform.rotation;
                bulletsOnScene[i].SetActive(true);
                bulletsOnScene[i].GetComponent<BulletShot>().Launch(transform.forward);
                freeBullet = true;
                break;
            }
        }
        if (!freeBullet)
        {
            bulletsOnScene.Add(Instantiate(bullets[0].prefab, firePoint.position, transform.rotation));
            bulletsOnScene[bulletsOnScene.Count - 1].GetComponent<BulletShot>().Launch(transform.forward);
        }
    }
    void AttackHit()
    {
        Shoot();
    }
}
