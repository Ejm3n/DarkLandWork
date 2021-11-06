using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private List<Bullets> _bullets;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<GameObject> _bulletsOnScene;

    private void Awake()
    {
        for (int i = 0; i < _bullets.Count; i++)
        {
            for (int j = 0; j < _bullets[i].Count; j++)
            {
                GameObject enemyBullet = Instantiate(_bullets[i].prefab);
                _bulletsOnScene.Add(enemyBullet);
                enemyBullet.SetActive(false);
            }
        }
    }

    /// <summary>
    /// находим свободную пулю, ставим её в позицию, запускаем
    /// </summary>
    public void Shoot()
    {        
        bool freeBullet = false;
        for (int i = 0; i < _bulletsOnScene.Count; i++)
        {
            if (!_bulletsOnScene[i].activeInHierarchy)
            {
                _bulletsOnScene[i].transform.position = _firePoint.position;
                _bulletsOnScene[i].transform.rotation = transform.rotation;
                _bulletsOnScene[i].SetActive(true);
                _bulletsOnScene[i].GetComponent<BulletShot>().Launch(transform.forward);
                freeBullet = true;
                break;
            }
        }
        if (!freeBullet)
        {
            _bulletsOnScene.Add(Instantiate(_bullets[0].prefab, _firePoint.position, transform.rotation));
            _bulletsOnScene[_bulletsOnScene.Count - 1].GetComponent<BulletShot>().Launch(transform.forward);
        }
    }

    void AttackHit()
    {
        Shoot();
    }
}
