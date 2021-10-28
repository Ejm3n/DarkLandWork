using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DigitalRuby.SoundManagerNamespace;
[RequireComponent (typeof (NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public int TypeNum;
    NavMeshAgent agent;
    Transform player;
    [SerializeField] float distanceToAttack = 2f;
    [SerializeField] float distanceToDoDamage = 4f;
    [SerializeField] int damage;
    [SerializeField] int scoreCost;
    bool alive = true;
    Animator anim;
    Health health;
    bool attacking = false;
    [HideInInspector] public bool AttackHitting = false;
    Spawner spawner;
    GameData gd;
    void Awake()
    {
        gd = FindObjectOfType<GameData>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerMovement>().transform;
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        spawner = FindObjectOfType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsAlive)
        {
            if (agent.enabled)
            {
                agent.SetDestination(player.position);
            }
            if (Vector3.Distance(transform.position, player.position) < distanceToAttack)
            {
                Attack();
            }
        }
        else if(alive)
        {
            Die();
        }
    }
    private void OnEnable()
    {
        agent.enabled = true;
        alive = true;
    }
    private void Die()
    {
        SoundManagerDemo.Instance.ZombieDeath(TypeNum);
        alive = false;
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        anim.SetTrigger("Died");
        gd.AddScore(scoreCost);
        StartCoroutine(WaitAndTurnOff());
        
    }
    IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        spawner.DisableEnemy(gameObject);
    }
    void Attack()
    {
        if(!attacking)
        {
            anim.SetTrigger("Attack");
            agent.enabled = false;
            attacking = true;
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
        if(alive)
            agent.enabled = true;
        attacking = false;
    }
    void AttackHit()
    {       
        if(Vector3.Distance(transform.position, player.position) < distanceToDoDamage)
            player.gameObject.GetComponent<Health>().TakeDamage(damage);
    }
}
