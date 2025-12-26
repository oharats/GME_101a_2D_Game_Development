using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _playerHandle;
    private int _pointValue = 10;

    private Animator _anim;
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;

    public bool _isDestroyed = false; //Flag to prevent additional points being added

    private int _shieldHP;
    [SerializeField]
    private GameObject _enemyShield;

    private void OnEnable()
    {
        _isDestroyed = false;
        _shieldHP = 0;

        if (_enemyShield != null)
        {
            _enemyShield.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Call to Player game object with NULL check
        _playerHandle = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        
     
        if (_playerHandle == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is NULL");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public bool IsDead()        //This method is used to call out from the Fire and Movement Scripts
    {
        return this._isDestroyed;
    }
    
    private void DestroyEnemy()
    {
        if (_isDestroyed)
        {
            return;
        }

        _isDestroyed = true;                    //Created this method to prevent AddScore() from running more than once
        _spawnManager.EnemyKilled();
        _anim.SetTrigger("OnEnemyDeath");       //"OnEnemyDeath" is the Parameter associated with 
        _audioSource.Play();                    //the Enemy_Destroyed animation in the Animator
        Destroy(gameObject, 2.0f);
    }

    public void TriggerEnemyShield(bool state)
    {
        _shieldHP = state ? 1 : 0;

        if (_enemyShield != null)
        {
            _enemyShield.SetActive(state);
        }

        _enemyShield.GetComponent<SpriteRenderer>().color = Color.red;
            
        
        Debug.Log($"{name} | Shield SET → HP:{_shieldHP} Visual:{_enemyShield.activeSelf}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed)
        {
           return;
        }

       
        if (other.tag == "Player")
        {
            //Verify logic/visual sync at impact time
            Debug.Assert((_shieldHP > 0) == _enemyShield.activeSelf, $"{name} | Shield DESYNC! HP:{_shieldHP} Visual:{_enemyShield.activeSelf}");

            //Damage Player with Null Check and Add _pointValue of Enemy to Score
            Player _player = other.GetComponent<Player>();
            if (_player == null) return;

            // Shield absorbs the collision
            if (_shieldHP > 0)
            {
                _shieldHP = 0;
                _enemyShield.SetActive(false);
                return;
            }

            //No Shield -> apply damamge
            _player.Damage();
            _player.AddScore(_pointValue);
            DestroyEnemy();

        }
        else if (other.tag == "Laser")
        {
            //Verify logic/visual sync at impact time
            Debug.Assert((_shieldHP > 0) == _enemyShield.activeSelf,$"{name} | Shield DESYNC! HP:{_shieldHP} Visual:{_enemyShield.activeSelf}");

            //Destroy Laser
            Destroy(other.gameObject);

            Debug.Log($"{name} | HIT → HP:{_shieldHP} Visual:{_enemyShield.activeSelf}");

            if (_shieldHP > 0)
            {
                _shieldHP = 0;
                _enemyShield.SetActive(false);
                return;
            }

            //Call out to Player and Add _pointValue of Enemy to Score
            _playerHandle.AddScore(_pointValue);

            DestroyEnemy();

        }
        else if (other.tag == "Nuke")
        {
            //Call out to Player and Add _pointValue of Enemy to Score
            _playerHandle.AddScore(_pointValue);

            //Destroy Laser
            Destroy(other.gameObject, 4.0f);
            DestroyEnemy();
        }
    }
}
