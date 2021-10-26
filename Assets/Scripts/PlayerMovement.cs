using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed;
    Animator anim;
    Health health;
    bool dead = false;
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
            movement *= Time.deltaTime * speed;

            transform.Translate(movement, Space.World);
            anim.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            anim.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        }
        else if(!dead)
        {
            anim.SetTrigger("Dead");
            dead = true;
        }
    }
}
