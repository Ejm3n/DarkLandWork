using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;
[System.Serializable]
public class Bullets
{
    public int Count;
    public GameObject prefab;
}

public class PlayerWeapon : MonoBehaviour
{
    private float nextFireTime;
    [SerializeField ] float delay;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform firePoint;
    [SerializeField] List<Bullets> bullets;
    [SerializeField] List<GameObject> bulletsOnScene;
    public int ammo;
    Health health;
    private void Awake()
    {       
        health = GetComponent<Health>();
        for (int i = 0; i < bullets.Count; i++)
        {
            for (int j = 0; j < bullets[i].Count; j++)
            {
                GameObject enemyObj = Instantiate(bullets[i].prefab);
                bulletsOnScene.Add(enemyObj);
                enemyObj.SetActive(false);
            }
        }
    }
    void Update()
    {
        if(health.IsAlive)
        {
            if(Time.timeScale == 1)
                AimTowardMouse();
            if (ReadyToFire() && Input.GetKeyDown(KeyCode.Mouse0) && ammo > 0 && !(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                Fire();
            else if (ReadyToFire() && Input.GetKeyDown(KeyCode.Mouse0) && ammo <= 0 && !(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                SoundManagerDemo.Instance.NoAmmo();
            
        }
        
    }

    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hit,Mathf.Infinity,layerMask))
        {
            Vector3 destination = hit.point;
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.Normalize();

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    bool ReadyToFire() => Time.time >= nextFireTime;
    void Fire()
    {
        SoundManagerDemo.Instance.AkShot();
        nextFireTime = Time.time + delay;
        Shoot();
        //BulletShot shot = Instantiate(bulletPrefab, firePoint.position, transform.rotation);

        //shot.Launch(transform.forward);
    }
    public void Shoot()
    {
        ammo--;
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
            bulletsOnScene[bulletsOnScene.Count-1].GetComponent<BulletShot>().Launch(transform.forward);
        }

    }
    public void AddAmmo(int count)
    {
        ammo += count;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AmmoBonus")
        {
            AddAmmo(other.GetComponent<Bonus>().countToAdd);
            other.gameObject.SetActive(false);
            SoundManagerDemo.Instance.PickUpAmmo();
        }
    }
}
