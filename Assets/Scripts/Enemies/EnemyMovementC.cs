using UnityEngine;

public class EnemyMovementC : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [Header("Ram Player")]
    [SerializeField]
    private float _homingRange = 4.0f;
    [SerializeField]
    private Transform _player;      //Assigned in the Inspector, but we also use a tag find


    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        bool _isDead = GetComponent<Enemy>().IsDead();

        if (_isDead == true)
        {
            _speed = 0f;        //This stops the Enemy object from colliding
                                //with the Player before it is Destroyed
        }

        if (_player != null && !_isDead)
        {
            float dist = Vector3.Distance(transform.position, _player.position);

            if (dist <= _homingRange)
            {
                Vector3 dir = (_player.position - transform.position).normalized;
                transform.Translate(dir * _speed * Time.deltaTime, Space.World);
                Debug.DrawRay(transform.position, dir, Color.green);

            }
            else
            {
                // Enemy normal movement downward at set speed upon spawning
                transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            // If no Player found or IsDead, maintain normal movement
            transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        }



        //Respawn at random x location after falling off screen
        if (transform.position.y < -5.3)
        {
            float _randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(_randomX, 7, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _homingRange);

    }

}
