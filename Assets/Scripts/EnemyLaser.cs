using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    [SerializeField]
    private float _enemyLaserSpeed = 8.0f;

    Vector3 _direction = new Vector3 (0, -1f, 0);

    // Update is called once per frame
    void Update()
    {
        //Enemy Laser motion
        transform.Translate(_direction * _enemyLaserSpeed * Time.deltaTime);

        //Destroy Enemy Laser if it falls off screen
        if (transform.position.y >= -7.0f)
        {
            if (transform.parent != null)   //What is the difference between "!= null" and "== true" in if statements?
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.transform.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }

            Destroy(this.gameObject);
        }
    }
}
