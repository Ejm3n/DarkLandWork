using DigitalRuby.SoundManagerNamespace;
using UnityEngine;
public class Health : MonoBehaviour
{
    
    [SerializeField] private int _startHP;
     [SerializeField] private int currentHP;
    [SerializeField] private bool isAlive = true;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public int CurrentHP { get => currentHP; set => currentHP = value; }

    
    private EnemyController _enemyController;

    private void Awake()
    {
        if (GetComponent<EnemyController>() != null)
        {
            _enemyController = GetComponent<EnemyController>();
        }
        CurrentHP= _startHP  ;
    }

    /// <summary>
    /// получение урона
    /// </summary>
    /// <param name="dmg"></param>
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
    
    /// <summary>
    /// восстановить персонажа
    /// </summary>
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
