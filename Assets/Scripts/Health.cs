using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;
public class Health : MonoBehaviour
{
    public int CurrentHP;
    public bool IsAlive = true;
    [SerializeField] private int startHP;
    EnemyController ec;
    private void Awake()
    {
       if(GetComponent<EnemyController>()!=null)
        {
            ec = GetComponent<EnemyController>();
        }
        startHP = CurrentHP;
    }
    public void TakeDamage(int dmg)
    {
        CurrentHP -= dmg;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            IsAlive = false;
        }
        if (ec != null)
            ec.TakeHit();
        else if (dmg>0)
            SoundManagerDemo.Instance.PlayerHit();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HpBonus")
        {
            if(CurrentHP != startHP)
            {
                TakeDamage(-1);
                other.gameObject.SetActive(false);
                SoundManagerDemo.Instance.PickUpMeds();
            }           
        }
        if(other.GetComponent<BulletShot>()!=null)
        {
            TakeDamage(other.GetComponent<BulletShot>().Damage);
            other.gameObject.SetActive(false);
        }        
    }
    public void Revive()
    {
        GetComponent<Collider>().enabled = true;
        CurrentHP = startHP;
        IsAlive = true;
    }
}
