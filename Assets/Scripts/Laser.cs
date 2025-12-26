using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _laserSpeed = 8.0f;
    private bool _isEnemyLaser = false;
    private Vector3 _fireDirection;
    

     // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    //Player Laser
    private void MoveUp()
    {
        //Laser motion
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

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

    //Enemy Laser
    private void MoveDown()
    {
        //Laser motion
        transform.Translate(_fireDirection * _laserSpeed * Time.deltaTime);

        //Destroy laser game object once it exits the screen
        if (transform.position.y <= -7.0f || transform.position.y >= 7.1f)
        {
            if (transform.parent != null)   
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaserBehind ()
    {
        _isEnemyLaser = true;
        gameObject.tag = "EnemyLaser";      //Change the Tag on the Laser game object so Enemies do not kill each other
        _fireDirection = Vector3.up;      //Change movement direction to fire backward
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
        gameObject.tag = "EnemyLaser";      //Change the Tag on the Laser game object so Enemies do not kill each other
        _fireDirection = Vector3.down;
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player _player = other.GetComponent<Player>();
            
            if (_player != null)
            {
                _player.Damage();
            }
        }
    }
}

