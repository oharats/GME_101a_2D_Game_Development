using UnityEngine;

public class EnemyMovementA : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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

        // Enemy movement downward at set speed upon spawning
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Respawn at random x location after falling off screen
        if (transform.position.y < -5.3)
        {
            float _randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(_randomX, 7, 0);
        }
    }
   
}
