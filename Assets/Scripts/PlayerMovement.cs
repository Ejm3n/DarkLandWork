using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;
    private  Animator anim;
    private Health health;
    private bool dead = false;
    // Update is called once per frame
    private void Awake()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (health.IsAlive && !dead)
        {
            float horizontal = Input.GetAxis(GameData.HORIZONTAL_AXIS);//вынести в общие переменные
            float vertical = Input.GetAxis(GameData.VERTICAL_AXIS);
            Vector3 movement = new Vector3(horizontal, 0f, vertical);
            
            if(movement.magnitude>=.5f)
            {
                Debug.Log("Do" + movement + " mag = " + movement.magnitude);
                movement.Normalize();
                Debug.Log("Posle =" + movement + " mag = " + movement.magnitude);
                movement *= Time.deltaTime * speed;
                transform.Translate(movement, Space.World);
            }
            float velocityX = Vector3.Dot(movement.normalized, transform.right);
            float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
            
            anim.SetFloat("Horizontal", velocityX, 0.1f, Time.deltaTime);
            anim.SetFloat("Vertical", velocityZ, 0.1f, Time.deltaTime);
        }
        else if(!dead)
        {
            anim.SetTrigger("Dead");
            dead = true;
        }
    }
}
