using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private int _lives = 3;
    private int _damageID;
    private int _shieldDmgID = 3;
    [SerializeField]
    private int _ammoCount = 0;
    [SerializeField]
    private int _ammoMissiles = 0;

    [SerializeField]
    private float _speed = 4.5f;
    [SerializeField]
    private float _speedMultiplier = 2.5f;
    private float _thrustPower = 1.0f;
    [SerializeField]
    private float _speedThrustPower = 2.0f;
    [SerializeField]
    private float _speedThrustDuration = 2.0f;
    [SerializeField]
    private float _speedThrustCD = 6.0f;
    private float _canThrust = -1;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1;
    [SerializeField]
    private float _nukeCD = 5.0f;
    [SerializeField]
    private float _nukePosY = 2.5f;

    private Vector3 _lazerOffset = new Vector3(0, 1.1f, 0);
    private Vector3 _missileOffset = new Vector3(0, 1.6f, 0);

    private bool _haveAmmo = false;
    private bool _haveMissiles = false;
    private bool _isTripleShotActive = false;
    private bool _isSpeedPowerUpActive = false;
    private bool _isShieldPowerUpActive = false;
    private bool _isDmgRight = false;
    private bool _isDmgLeft = false;
    private bool _isNukeActive = false;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotLaserPrefab;
    [SerializeField]
    private GameObject _nukePrefab;
    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private GameObject _playerShield;
    [SerializeField]
    private GameObject _damageRightEngine;
    [SerializeField]
    private GameObject _damageLeftEngine;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _thrusterReadyLight;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private CameraShake _mainCamera;

    float _horizontalInput;
    float _verticalInput;
    Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        //Assign player a new postion (0,0,0) @ start
        transform.position = new Vector3( 0, 0, 0);

        _haveAmmo = true;
        _ammoCount = 15;

        //Set Engine Damage anim to false
        _damageLeftEngine.SetActive(false);
        _damageRightEngine.SetActive(false);
        _thruster.SetActive(false); 

        //Calls to other Game Objects and Null checks
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _mainCamera = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_audioSource == null )
        {
            Debug.LogError("Audio Source on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        MovementCode();

        //Fire with Space bar, must have Ammo and be off Cool Down
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _haveAmmo) 
        {
            FireLaser();
        }

        //Fire Missile with X key, must have ammo and be off cool down
        if (Input.GetKeyDown(KeyCode.X) && Time.time > _canFire && _haveMissiles)
        {
            FireMissile();
        }

        //Thrust burst with Shift Key Down
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _canThrust)
        {
            ThrustersOn();
        }

        //Thrust burst off when Shift Key is released
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ThrustersOff();
        }

        
        //Visual UI indicator for Thrusters Ready
        if (Time.time < _canThrust)
        {
            _thrusterReadyLight.GetComponent<Renderer>().material.color = Color.red;
        }
        if (Time.time > _canThrust)
        {
            _thrusterReadyLight.GetComponent<Renderer>().material.color = Color.green;
        }

    }

    //Movement code and screen boundary conditions
    void MovementCode()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _direction = new Vector3(_horizontalInput, _verticalInput, 0);

        //Movement vector with input controls INCLUDING Speed Boost Power Up
        if (_isSpeedPowerUpActive == true)
        {
            transform.Translate(_direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            //Normal speed movement
            transform.Translate(_direction * _speed * _thrustPower * Time.deltaTime);
        }

        
        //Boundary conditions for screen x position
        if (transform.position.x >= 10.2f)
        {
            transform.position = new Vector3(-10.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -10.2f)
        {
            transform.position = new Vector3(10.2f, transform.position.y, 0);
        }

        //Boundary conditions for screen y position using a Math Clamp for y
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.9f, 5.9f), 0);
    }

    //Set Thruster velocity, activate sprite and begin cooldown routine
    void ThrustersOn()
    {
        _thrustPower = _speedThrustPower;
        _thruster.SetActive(true);
        StartCoroutine(ThrustTimeOut());
    }

    //Thrust Burst cool down routine
    IEnumerator ThrustTimeOut ()
    {
        // Thrust duration in WaitForSeconds, then powers off and CD begins
        yield return new WaitForSeconds(_speedThrustDuration);
        _thrustPower = 1;
        _thruster.SetActive(false);
        //Cooldown begins
        _canThrust = Time.time + _speedThrustCD;
        UpdateThruster(_speedThrustCD);
    }

    //Turns thrusters off when Shift Key is released and turns off sprite
    void ThrustersOff ()
    {
        _thrustPower = 1;
        _thruster.SetActive(false);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isNukeActive == true)
        {
            Vector3 _posToSpawn = new Vector3(transform.position.x, _nukePosY, 0);
            Instantiate(_nukePrefab, _posToSpawn, Quaternion.identity);
            _ammoCount++;
            StartCoroutine(ShutNukeOff());
        }
        if (_isTripleShotActive == true && _isNukeActive != true)
        {           
            Instantiate(_tripleShotLaserPrefab, transform.position, Quaternion.identity);
            _ammoCount -= 3;
            AmmoCounter(_ammoCount);
        }
        else if (_isNukeActive != true)
        {            
            Instantiate(_laserPrefab, transform.position + _lazerOffset, Quaternion.identity);
            _ammoCount -= 1;
            AmmoCounter(_ammoCount);
        }

        //Latch for running out of ammo
        if (_ammoCount <= 0)
        {
            _ammoCount = 0;
            _haveAmmo = false;
        }

        _audioSource.Play();
    }

    private void FireMissile()
    {
        _canFire = Time.time + _fireRate;

        Instantiate(_missilePrefab, transform.position + _missileOffset, Quaternion.identity);
        _ammoMissiles -= 1;

        if (_ammoMissiles <= 0)
        {
            _ammoMissiles = 0;
            _haveMissiles = false;
        }

    }

    //Interface with UI Manager to track ammo
    public void AmmoCounter(int ammo)
    {
        _ammoCount = ammo;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    //Ammo Power Up and update Ammo Counter
    public void AmmoPowerUp ()
    {
        _ammoCount = 15;
        _haveAmmo = true;
        AmmoCounter(_ammoCount);
    }

    public void MissilePowerUp()
    {
        _ammoMissiles = 3;
        _haveMissiles = true;
    }
   
    //Triple Shot Power Up - Activate
    public void TripleShotPowerUp()
    {
        //Check to make sure there is enough ammo to fire a triple shot
        if (_ammoCount >= 3)
        {
            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDown());
        }
        else
        {
            return;
        }
    }

    //Triple Shot Power Up - Deactivate
    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    //Nuke Power Up
    public void NukePowerUp()
    {
        _isNukeActive = true;
    }

    IEnumerator ShutNukeOff()
    {
        yield return new WaitForSeconds(_nukeCD);
        _isNukeActive = false;
    }
   
    //Player Speed Power Up - Activate, turn ON Thruster anim
    public void SpeedPowerUp()
    {
        _isSpeedPowerUpActive = true;
        _thruster.SetActive(true);
        StartCoroutine(SpeedPowerUpOff());
    }

    //Player Speed Power Up - Deactivate, turn OFF Thruster anim
    IEnumerator SpeedPowerUpOff()
    {
        yield return new WaitForSeconds(5.0f);
        _thruster.SetActive(false); 
        _isSpeedPowerUpActive = false;
    }

    //Speed Debuff only has a % chance of negatively affecting player
    public void SpeedDebuff()
    {
        int _debuffSelect = Random.Range(0, 100);

        if (_debuffSelect <40)
        {
            _thrustPower = 0.5f;
            StartCoroutine(SpeedDebuffOff());
        }
        else
        {
            SpeedPowerUp();
        }
    }

    IEnumerator SpeedDebuffOff()
    {
        yield return new WaitForSeconds(4f);
        _thrustPower = 1.0f;
    }

    //Shield Power Up Activate (will deactivate automatically in Damage()
    public void ShieldPowerUp()
    {
        _isShieldPowerUpActive = true;
        _shieldDmgID = 3;
        _playerShield.gameObject.GetComponent<Renderer>().material.color = Color.white;
        _playerShield.SetActive(true); 
    }

    //Damage to Player, subtract one Life if Shields are not active
    public void Damage()
    {
        StartCoroutine(_mainCamera.Shake());

        //Shield Power Up Functionality = 3 Shields per Power Up
        if (_isShieldPowerUpActive == true)
        {
            _shieldDmgID -= 1;
            switch (_shieldDmgID)
            {
                case 0:
                    _isShieldPowerUpActive = false;
                    _playerShield.SetActive(false);
                    break;
                case 1:
                    _playerShield.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 2:
                    _playerShield.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
            }
        }
        else
        {
            _lives -= 1;
            _uiManager.UpdateLives(_lives);

            //Random Engine Damage Animation Assignment
            if (_isDmgRight == false && _isDmgLeft == false)
            {
                _damageID = Random.Range(0, 2);
                switch(_damageID)
                {
                    case 0:
                        DamageRightEngine();
                        break;
                    case 1:
                        DamageLeftEngine();
                        break;
                }
            }
            else if (_isDmgRight == false && _isDmgLeft == true)
            {
                DamageRightEngine();
            }
            else
            {
                DamageLeftEngine();
            }
           
            //Player Dead
            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    //Set Right Engine Damamge Animation
    private void DamageRightEngine()
    {
        _damageRightEngine.SetActive(true);
        _isDmgRight = true;
    }

    //Set Left Engine Damamge Animation
    private void DamageLeftEngine() 
    {
        _damageLeftEngine.SetActive(true);
        _isDmgLeft = true;
    }
    //Health Power Up
    public void HealthPowerUp()
    {
        if (_lives >= 3)
        {
            return;
        }
        else
        {
            _lives++;
            _uiManager.UpdateLives(_lives);

         //Logic to repair engines according to which are currently active
            if (_isDmgLeft == true && _isDmgRight == false)
            {
                _damageLeftEngine.SetActive(false);
                _isDmgLeft = false;
            }
            if (_isDmgLeft == false && _isDmgRight == true)
            {
                _damageRightEngine.SetActive(false);
                _isDmgRight = false;
            }
            if (_isDmgLeft == true && _isDmgRight == true)
            {
                _damageLeftEngine.SetActive(false);
                _isDmgLeft = false;
            }
        }
            
    }

    //Add _points value (_pointValue) passed from Enemy to _score variable and call UI Manger to UpdateScore()
    public void AddScore(int _points)
    {
        _score += _points;
        _uiManager.UpdateScore(_score);
    }

    public void UpdateThruster (float _coolDown)
    {
        _uiManager.UpdateThrusterCD(_coolDown);
    }
}




/*
 * [SereializeField]
 * private float _nukePosY = 2.5f;
 * [SereializeField]
 * private GameObject _nukePrefab;
 * private bool _isNukeActive = false;
 * 
 * if (_isNukeActive == true)
        {   
            Vector3 _posToSpawn = new Vector3(transform.position.x, _nukePosY, 0);
            Instantiate(_nukePrefab, _posToSpawn, Quaternion.identity);
            StartCoroutine(ShutNukeOff());
        }

IEnumerator ShutNukeOff()
{
    yield return new WaitForSeconds(5.0f);
    _isNukeActive = false;
}

public void NukePowerUp() 
{
	_isNukeActive = true
	_spawnManager.NukeSpawnInitiateCD();
}

 * */

