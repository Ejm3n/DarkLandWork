using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _distanceToAttack = 2f;
    [SerializeField] private float _distanceToDoDamage = 4f;
    [SerializeField] private int _damage;
    [SerializeField] private int _scoreCost;
    [HideInInspector] public bool AttackHitting = false;
    public int TypeNum;
    private NavMeshAgent _agent;
    private Transform _player;   
    private bool _alive = true;
    private Animator _anim;
    private Health _health;
    private bool _attacking = false;   
    private Spawner _spawner;
    private GameData _gameData;
    void Awake()
    {
        _gameData = FindObjectOfType<GameData>();
        _agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<PlayerMovement>().transform;
        _anim = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _spawner = FindObjectOfType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_health.IsAlive)
        {
            if (_agent.enabled)
            {
                _agent.SetDestination(_player.position);
            }
            if (Vector3.Distance(transform.position, _player.position) < _distanceToAttack)
            {
                Attack();
            }
        }
        else if (_alive)
        {
            Die();
        }
    }
    private void OnEnable()
    {
        _agent.enabled = true;
        _alive = true;
    }
    private void Die()
    {
        SoundManagerDemo.Instance.ZombieDeath(TypeNum);
        _alive = false;
        GetComponent<Collider>().enabled = false;
        _agent.enabled = false;
        _anim.SetTrigger("Died");
        _gameData.AddScore(_scoreCost);
        StartCoroutine(WaitAndTurnOff());

    }
    IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        _spawner.DisableEnemy(gameObject);
    }
    void Attack()
    {
        if (!_attacking)
        {
            _anim.SetTrigger("Attack");
            _agent.enabled = false;
            _attacking = true;
            SoundManagerDemo.Instance.ZombieAttack(TypeNum);
        }

    }
    public void TakeHit()
    {
        SoundManagerDemo.Instance.ZombieGetHit(TypeNum);
    }
    //animation event
    void AttackComplete()
    {
        if (_alive)
            _agent.enabled = true;
        _attacking = false;
    }
    void AttackHit()
    {
        if (Vector3.Distance(transform.position, _player.position) < _distanceToDoDamage)
            _player.gameObject.GetComponent<Health>().TakeDamage(_damage);
    }
}
