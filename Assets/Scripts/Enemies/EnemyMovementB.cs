using UnityEngine;


public class EnemyMovementB : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _freq = 4;  //Frequency
    [SerializeField]
    private float _amp = 2; //Amplitude

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

        //Oscillates the x value creating a zig-zag motion
        float _posX = (Mathf.Sin(Time.time * _freq)) * _amp;    
        Vector3 _direction = new Vector3(_posX, -1, 0);
        
        // Enemy movement downward at set speed upon spawning
        transform.Translate(_direction * _speed * Time.deltaTime);


        //Respawn at random x location after falling off screen
        if (transform.position.y < -5.3)
        {
            float _randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(_randomX, 7, 0);
        }
    }

}
