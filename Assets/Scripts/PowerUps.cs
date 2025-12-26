using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField]    private float _speed = 3.0f;
    [SerializeField]    private float _speedToPlayer = 10.0f; //Speed of Power Up if pulled to Player
    [SerializeField]    private int _powerUpID; // 0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Ammo, 4 = Health, 5 = Nuke, 6 = Speed Debuff, 7 = Missiles
    [SerializeField]    private AudioClip _clip;
    [SerializeField]    private AudioClip _pwrUpShot;
    [SerializeField]    private Transform _player;
    private bool _isDestroyed = false;
    private bool _toPlayer = false;

    void Start()
    {
        //Looks for Player in the scene based on the Player Tag
        if (_player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.transform;
        }

    }
    // Update is called once per frame
    void Update()
    {
        Movement(); 

        //Pulls Power Up to Player while holding down C Key
        if (Input.GetKeyDown(KeyCode.C))
        {
            _toPlayer = true;
        }

        //Returns to normal movement when C Key is let go
        if (Input.GetKeyUp(KeyCode.C))
        {
            _toPlayer = false;
        }
    }

    private void Movement()
    {
        if (_isDestroyed == true)
        {
            _speed = 0f;        //This stops the Power Up object from 
                                //traveling further when it is Destroyed
        }

        if (_toPlayer == true)
        {
            float dist = Vector3.Distance(transform.position, _player.position);
            Vector3 dir = (_player.position - transform.position).normalized;
            transform.Translate(dir * _speedToPlayer * Time.deltaTime, Space.World);
        }
        else if (_toPlayer == false)
        {
            // Normal PowerUp movement
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        //Destroy PowerUp if it drops off screen
        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
        }
    }


    public void FlyToPlayerOn()
    {
        _toPlayer = true;
    }

    public void FlyToPlayerOff()
    {
        _toPlayer = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Enable PowerUp with Null Check
            Player _player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position, 5.0f);

            if (_player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        _player.TripleShotPowerUp();
                        break;
                    case 1:
                        _player.SpeedPowerUp();                      
                        break;
                    case 2:
                        _player.ShieldPowerUp();
                        break ;
                    case 3:
                        _player.SpeedDebuff();
                        break;
                    case 4:
                        _player.HealthPowerUp();    
                        break;
                    case 5:
                        _player.NukePowerUp();
                        break;
                    case 6:
                        _player.AmmoPowerUp();
                        break;
                    case 7:
                        _player.MissilePowerUp();
                        break;
                    default:
                        Debug.Log("Default");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "EnemyLaser" || other.tag == "Missile")
        {
            AudioSource.PlayClipAtPoint(_pwrUpShot, transform.position, 5.0f);

            if (_isDestroyed)
            {
                return;
            }

            //Destroy Laser
            Destroy(other.gameObject);

            _isDestroyed = true;                   
            Destroy(this.gameObject);
        }
    }
  
}
