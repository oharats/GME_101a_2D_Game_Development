using UnityEngine;

public class Player_Missile : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 8.0f;

    [Header("Homing")]
    [SerializeField]
    private float _homingRange = 5.0f;
    [SerializeField]
    private float _turnSpeed = 300f;
    private Transform _enemy;


    // Update is called once per frame
    void Update()
    {
        if (_enemy == null)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null) _enemy = enemy.transform;
        }

       MoveUp();
    }

    //Player Laser
    private void MoveUp()
    {
        if (_enemy != null)
        {
            float dist = Vector3.Distance(transform.position, _enemy.position);

            if (dist < _homingRange)
            {
                Vector3 dir = (_enemy.position - transform.position).normalized;

                // Target rotation
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

                // Smooth rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,_turnSpeed * Time.deltaTime);

                transform.Translate(dir * _missileSpeed * Time.deltaTime, Space.World);
                Debug.DrawRay(transform.position, dir, Color.green);
            }
            else
            {
                transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);
            }
        }
        else
        {
            //Laser motion
            transform.Translate(Vector3.up * _missileSpeed * Time.deltaTime);
        }

        //Destroy laser game object once it exits the screen
        if (transform.position.y >= 7.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _homingRange);

    }


}
