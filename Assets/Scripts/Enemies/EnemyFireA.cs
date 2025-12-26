using UnityEngine;

public class EnemyFireA : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLazerPrefab;
    private float _fireRate = 3f;
    private float _canFire = -1;
    
    private Transform _pwrUPs;
    private bool _isPwrUpFront;
    private bool _isPwrUpAligned;
    private bool _fireAway = false;
    private float _rdyFire = -1;

    [Header("ShootPwrUp")]
    [SerializeField] private float _frontXWindow = 1.0f;
    [SerializeField] private float _frontRangeHeight = 10f;   // how far upward to visualize
    [SerializeField] private float _frontRangeStartOffset = 0.0f; // start above enemy if desired
    [SerializeField] private bool _showFrontFireGizmo = true;


    // Start is called before the first frame update
    void Start()
    {
        if (_pwrUPs == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("PowerUps");
            if (p != null) _pwrUPs = p.transform;
        }

        _fireAway = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool _isDead = GetComponent<Enemy>().IsDead();

        _isPwrUpFront = _pwrUPs != null && transform.position.y > _pwrUPs.transform.position.y;
        _isPwrUpAligned = _pwrUPs != null && Mathf.Abs(_pwrUPs.position.x - transform.position.x) <= _frontXWindow;

        if (Time.time > _canFire && _isDead == false)
        {
            EnemyFireLaser();
        }

        if (_isPwrUpAligned == true && _isPwrUpFront == true)
        {
            _fireAway = true;
        }
        else
        {
            _fireAway = false;
        }

        if (_fireAway == true && Time.time > _rdyFire)
        {
            ShootPowerUp();
        }
    }

    private void EnemyFireLaser()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        GameObject _enemyLaser = Instantiate(_enemyLazerPrefab, transform.position, Quaternion.identity);

        //Created and array for the 2 lasers fired each time an Enemy shoots.
        //Each "i" in the logic below is a single laser.
        //This allows for unlimited lasers to be fired and tracked
        Laser[] _lasers = _enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < _lasers.Length; i++)
        {
            _lasers[i].AssignEnemyLaser();
        }
    }

    private void ShootPowerUp ()
    {
        _fireRate = Random.Range(0.1f, 1f);
        _rdyFire = Time.time + _fireRate;
        GameObject _enemyLaser = Instantiate(_enemyLazerPrefab, transform.position, Quaternion.identity);
        Laser[] _lasers = _enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < _lasers.Length; i++)
        {
            _lasers[i].AssignEnemyLaser();
        }
    }

    void OnDrawGizmos()
    {
        if (!_showFrontFireGizmo) return;

        // Draw a vertical "strip" below the enemy representing the alignment window.
        // Width = 2 * _frontXWindow, Height = _frontRangeHeight.
        Vector3 origin = transform.position + Vector3.up * _frontRangeStartOffset;

        float leftX = origin.x - _frontXWindow;
        float rightX = origin.x + _frontXWindow;

        float topY = origin.y;
        float bottomY = origin.y - _frontRangeHeight;

        // Choose color:
        // - Green if fireaway condition is currently true (in play mode and player exists)
        // - Yellow otherwise
        bool frontCondition = false;
        if (Application.isPlaying && _pwrUPs != null)
        {
            bool infront = _pwrUPs.position.y < transform.position.y;
            bool aligned = Mathf.Abs(_pwrUPs.position.x - transform.position.x) <= _frontXWindow;
            frontCondition = infront && aligned;
        }

        Gizmos.color = frontCondition ? Color.green : Color.yellow;

        // Draw rectangle
        Vector3 bl = new Vector3(leftX, bottomY, 0f);
        Vector3 br = new Vector3(rightX, bottomY, 0f);
        Vector3 tl = new Vector3(leftX, topY, 0f);
        Vector3 tr = new Vector3(rightX, topY, 0f);

        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);

        // Draw a line to player so you can see relative position
        if (Application.isPlaying && _pwrUPs != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, _pwrUPs.position);
        }
    }
}
