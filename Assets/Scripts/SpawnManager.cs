using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyType;
    private int _enemyID;
    [SerializeField]
    private GameObject _enemyContainer; //Container to keep Enemy spawns from cluttering the Edit UI
    [SerializeField]
    private GameObject _bossPrefab;
    [SerializeField]
    private GameObject[] _powerUps;  //Array containing the Power Up Prefabs, spawned based on ID# generated below
    private int _PowerUpID;

    private float _spawnDelay = 3f;
    private bool _stopSpawningEnemies = false;
    [SerializeField]
    private bool _stopSpawningPowerUps = false;

    [SerializeField]
    private int[] _enemiesPerWave = { 2, 2, 3, 3, 4, 4, 5, 5, 6 };
    private int _enemiesAlive;
    [SerializeField]
    private float _timeBetweenSpawns = 2f;
    [SerializeField]
    private float _timeBetweenWaves = 8f;
    
    private GameManager _gameManager;
    private UIManager _uiManager;
        
    private void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_gameManager == null )
        {
            Debug.LogError("Game Manager is NULL");
        }

        if (_uiManager == null )
        {
            Debug.LogError("UI Manager is NULL");
        }
    }

    public void Update()
    {
       

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    //Enemy Spawn Generator with Wave functionality
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        Debug.Log("Delay Over");

        while (_stopSpawningEnemies == false)
        {
            // Loop through each wave
            for (int _waveIndex = 0; _waveIndex < _enemiesPerWave.Length; _waveIndex++)
            {
                int _enemiesToSpawn = _enemiesPerWave[_waveIndex];

                Debug.Log("Starting Wave " + (_waveIndex + 1));

                // Spawn random enemies for this wave
                for (int i = 0; i < _enemiesToSpawn; i++)
                {

                    int _randomEnemy = Random.Range(0, 100);

                    if (_randomEnemy < 60)
                    {
                        _enemyID = 0;       //Enemy B (Zig-Zag)
                    }
                    else if (_randomEnemy >= 60 && _randomEnemy < 70)
                    {
                        _enemyID = 1;       //Enemy (Homing)
                    }
                    else if (_randomEnemy >= 70 && _randomEnemy < 90)
                    {
                        _enemyID = 2;       //Enemy C (Missile)
                    }
                    else
                    {
                        _enemyID = 3;       //Enemy D (Dodger)
                    }

                    Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                    GameObject _newEnemy = Instantiate(_enemyType[_enemyID], _posToSpawn, Quaternion.identity);
                    _newEnemy.transform.parent = _enemyContainer.transform;
                    _enemiesAlive++;

                    //Allows enemies to spawn a shield 30% of the time
                    Enemy _enemy = _newEnemy.gameObject.GetComponent<Enemy>();
                    int _enemyShieldActivate = Random.Range(0, 100);
                    if (_enemyShieldActivate < 30)
                    {
                        _enemy.TriggerEnemyShield(true);
                    }

                    yield return new WaitForSeconds(_timeBetweenSpawns);

                }

                yield return new WaitUntil(() => _enemiesAlive == 0);

                Debug.Log("Wave " + (_waveIndex + 1) + " completed!");

                // Wait before next wave
                yield return new WaitForSeconds(_timeBetweenWaves);
            }

            _stopSpawningEnemies = true;
            Debug.Log("All waves completed!");
        }

        Debug.Log("Boss Wave!");
        Vector3 _bossSpawnPos = new Vector3(0, 5, 0);
        GameObject _boss = Instantiate(_bossPrefab, _bossSpawnPos, Quaternion.identity);
        _uiManager.BossRegister();
    }

    public void EnemyKilled()  //Decremented from Enemy script Kill() method
    {
        _enemiesAlive--;
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        //Spawn Power Up every 3-7 seconds after a 3 sec delay
        yield return new WaitForSeconds(_spawnDelay);           //This is only a delay at the beginning of the game

        while (_stopSpawningPowerUps == false)
        {
            int _randomPwrUP = Random.Range(0, 100);

            if (_randomPwrUP <45)
            {
                _PowerUpID = Random.Range(0, 4);    //Triple Shot, Speed, Shield, Speed Debuff
            }
            else if (_randomPwrUP >= 45 &&  _randomPwrUP < 55)
            {
                _PowerUpID = 4;     //Health
            }
            else if (_randomPwrUP >= 55 && _randomPwrUP < 60)
            {
                _PowerUpID = 5;     //Nuke
            }
            else if (_randomPwrUP >= 60 && _randomPwrUP < 70)
            {
                _PowerUpID = 7;     //Missiles
            }
            else if (_randomPwrUP >= 70)
            {
                _PowerUpID = 6;     //Ammo
            }

            Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject _newPowerUp = Instantiate(_powerUps[_PowerUpID], _posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }

    public void OnPlayerDeath() 
    { 
        _stopSpawningPowerUps = true;
        _stopSpawningEnemies = true;
        _gameManager.GameOver();
    }

    public void OnVictory()
    {
        _stopSpawningPowerUps = true;
    }
}


/*
 * SPAWN MANAGER (BOTH Versions)
if (_nukeCanSpawn == true) 
{
Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
int _randomPowerUp = Random.Range(0, 6);
GameObject _newPowerUp = Instantiate(_powerUps[_randomPowerUp], _posToSpawn, Quaternion.identity);
yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
{
else
{
Vector3 _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
int _randomPowerUp = Random.Range(0, 5);
GameObject _newPowerUp = Instantiate(_powerUps[_randomPowerUp], _posToSpawn, Quaternion.identity);
yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
}

SPAWN MANAGER V1
IEnumerator NukeSpawnCD()
{
    yield return new WaitForSeconds(5.0f);
    _nukeCanSpawn = true;
}

public void NukeSpawnInitiateCD ()
{
	_nukeCanSpawn = false;
	StartCoroutine(NukeSpawnCD());
}

----------
PLAYER V1
public void NukePowerUp() 
{
	_isNukeActive = true
	_spawnManager.NukeSpawnInitiateCD();
}

--------------------------------------------------------------------------------
SPAWN MANAGER V2
if (_randomPowerUp == 6)
{
 	_nukeCanSpawn = false;
StartCoroutine(NukeSpawnCD());
}

IEnumerator NukeSpawnCD()
{
    yield return new WaitForSeconds(5.0f);
    _nukeCanSpawn = true;
}
-------
PLAYER V2
public void NukePowerUp() 
{
	_isNukeActive = true
}

*/
