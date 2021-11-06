using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed;
    private Animator _anim;
    private Health _health;
    private bool _dead = false;
    // Update is called once per frame
    private void Awake()
    {
        _health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_health.IsAlive && !_dead)
        {
            float horizontal = Input.GetAxis(GameData.HorizontalAxis);//вынести в общие переменные
            float vertical = Input.GetAxis(GameData.VerticalAxis);
            Vector3 movement = new Vector3(horizontal, 0f, vertical);

            if (movement.magnitude >= .5f)
            {
                Debug.Log("Do" + movement + " mag = " + movement.magnitude);
                movement.Normalize();
                Debug.Log("Posle =" + movement + " mag = " + movement.magnitude);
                movement *= Time.deltaTime * _speed;
                transform.Translate(movement, Space.World);
            }
            float velocityX = Vector3.Dot(movement.normalized, transform.right);
            float velocityZ = Vector3.Dot(movement.normalized, transform.forward);

            _anim.SetFloat("Horizontal", velocityX, 0.1f, Time.deltaTime);
            _anim.SetFloat("Vertical", velocityZ, 0.1f, Time.deltaTime);
        }
        else if (!_dead)
        {
            _anim.SetTrigger("Dead");
            _dead = true;
        }
    }
}
