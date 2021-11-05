using DigitalRuby.SoundManagerNamespace;
using UnityEngine;
public class Health : MonoBehaviour
{
    
    [SerializeField] private int _startHP;
    public int CurrentHP;
    public bool IsAlive = true;
    private EnemyController _enemyController;

    private void Awake()
    {
        if (GetComponent<EnemyController>() != null)
        {
            _enemyController = GetComponent<EnemyController>();
        }
        _startHP = CurrentHP;
    }

    public void TakeDamage(int dmg)
    {
        CurrentHP -= dmg;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            IsAlive = false;
        }
        if (_enemyController != null)
            _enemyController.TakeHit();
        else if (dmg > 0)
            SoundManagerDemo.Instance.PlayerHit();
    }
    
    public void Revive()
    {
        GetComponent<Collider>().enabled = true;
        CurrentHP = _startHP;
        IsAlive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HpBonus"))
        {
            if (CurrentHP != _startHP)
            {
                TakeDamage(-1);
                other.gameObject.SetActive(false);
                SoundManagerDemo.Instance.PickUpMeds();
            }
        }
        if (other.GetComponent<BulletShot>() != null)
        {
            TakeDamage(other.GetComponent<BulletShot>().Damage);
            other.gameObject.SetActive(false);
        }
    }
}
