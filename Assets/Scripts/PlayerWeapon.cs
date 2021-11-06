using DigitalRuby.SoundManagerNamespace;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Bullets
{
    public int Count;
    public GameObject prefab;
}

public class PlayerWeapon : MonoBehaviour
{

    [SerializeField] private float _delay;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private List<Bullets> _bullets;
    [SerializeField] private List<GameObject> _bulletsOnScene;
    public int Ammo { get; private set; }
    private float _nextFireTime;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        for (int i = 0; i < _bullets.Count; i++)
        {
            for (int j = 0; j < _bullets[i].Count; j++)
            {
                GameObject enemyObj = Instantiate(_bullets[i].prefab);
                _bulletsOnScene.Add(enemyObj);
                enemyObj.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (_health.IsAlive)
        {
            if (Time.timeScale == 1)
                AimTowardMouse();
            if (ReadyToFire() && Input.GetKeyDown(KeyCode.Mouse0) && Ammo > 0 && !(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                Fire();
            else if (ReadyToFire() && Input.GetKeyDown(KeyCode.Mouse0) && Ammo <= 0 && !(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                SoundManagerDemo.Instance.NoAmmo();
        }
    }

    /// <summary>
    /// находим свободную пулю, ставим её в позицию, запускаем
    /// </summary>
    public void Shoot()
    {
        Ammo--;
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

    /// <summary>
    /// добавить пуль
    /// </summary>
    /// <param name="count">количество</param>
    public void AddAmmo(int count)
    {
        Ammo += count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AmmoBonus")
        {
            AddAmmo(other.GetComponent<Bonus>().countToAdd);
            other.gameObject.SetActive(false);
            SoundManagerDemo.Instance.PickUpAmmo();
        }
    }

    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            Vector3 destination = hit.point;
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.Normalize();

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private bool ReadyToFire() => Time.time >= _nextFireTime;

    private void Fire()
    {
        SoundManagerDemo.Instance.AkShot();
        _nextFireTime = Time.time + _delay;
        Shoot();
    }
}
