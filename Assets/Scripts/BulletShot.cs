using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletShot : MonoBehaviour
{
    [SerializeField]private float _speed = 15f;
    public int Damage;

    private void OnEnable()
    {
       StartCoroutine( WaitAndTurnOff());
    }

    /// <summary>
    /// запуск пули
    /// </summary>
    /// <param name="dir"></param>
    public void Launch(Vector3 dir)
    {
        dir.Normalize();
        transform.up = dir;
        GetComponent<Rigidbody>().velocity = dir * _speed;
    }

    private IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);      
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
    }
}
