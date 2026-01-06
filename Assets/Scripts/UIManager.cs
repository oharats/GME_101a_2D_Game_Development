using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevel;
    [SerializeField]
    private Text _thrustTimerText;
    [SerializeField]
    private Text _AmmoCountText;
    [SerializeField]
    private Text _outOfAmmoText;
    [SerializeField]
    private Text _victoryText;
    [SerializeField]
    private Text _mainMenuText;

    private float _thrustCD;
    private bool _isOutOfAmmo = false;

    private Player _playerHandle;
    private GameManager _gameManager;
    private Boss _boss;

    [SerializeField]
    private Image _livesDisplay;

    [SerializeField]
    private Sprite[] _livesSprite;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartLevel.gameObject.SetActive(false);
        _outOfAmmoText.gameObject.SetActive(false);
        _victoryText.gameObject.SetActive(false); 
        _mainMenuText.gameObject.SetActive(false);
        _thrustTimerText.text = "Thrusters Ready!";

        //Calls to other Game Objects with NULL checks
        _playerHandle = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        
        if (_playerHandle == null)
        {
            Debug.LogError("Player is NULL");
        }

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    private void Update()
    {
        //Thrust Cooldown Timer updates to UI
        if (_thrustCD > 0)
        {
            _thrustCD -= Time.deltaTime;
            ThrusterTimer(_thrustCD);
        }
        else
        {
            _thrustTimerText.text = "Thrusters Ready!";
            _thrustCD = 0;
        }
    }


    //Update UI score value with _score int passed from Player
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmo(int ammo)
    {
        _AmmoCountText.text = "Ammo: " + ammo;

        if (ammo < 1)
        {
            StartCoroutine(OutOfAmmo());
        }
    }

    public void UpdateOutOfAmmo()
    {
        _isOutOfAmmo = false;
        StopCoroutine(OutOfAmmo());
    }

    public void UpdateThrusterCD(float _coolDown)
    {
        _thrustCD = _coolDown;
    }

    //Thruster Timer - On Screen
    public void ThrusterTimer (float thrustCD)
    {
        decimal _uiThrustValue = Mathf.RoundToInt(thrustCD);
        _thrustTimerText.text = "Thrusters ready in: " + _uiThrustValue; // Update the display
    }
   
    public void UpdateLives(int currentLives)
    {
        if (currentLives <0 || currentLives >= _livesSprite.Length)
        {
            return;
        }
        
        _livesDisplay.sprite = _livesSprite[currentLives];

        if (currentLives == 0)
        {
            RestartSequence();
        }
    }

    public void RestartSequence()
    {
        StartCoroutine(GameOverText());

        //Displays 'Restart" text on screen
        _restartLevel.gameObject.SetActive(true);
        
    }

    //Flashes 'Game Over' On screen
    IEnumerator GameOverText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void BossRegister()
    {
        _boss = GameObject.Find("FinalBoss(Clone)").GetComponent<Boss>();
        if (_boss == null)
        {
            Debug.LogError("Final Boss is NULL!");
        }
    }

    public void Victory()
    {
        StartCoroutine(VictoryText());

        //Displays "Main Menu" text on screen
        _mainMenuText.gameObject.SetActive(true);
    }

    IEnumerator VictoryText()
    {
        while (true)
        {
            _victoryText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _victoryText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator OutOfAmmo()
    {
        _isOutOfAmmo = true;

        while (_isOutOfAmmo)
        {
            _outOfAmmoText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _outOfAmmoText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }
    }


}
