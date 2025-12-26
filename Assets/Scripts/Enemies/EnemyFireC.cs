using UnityEngine;

public class EnemyFireC : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLazerPrefab;
    private float _fireRate = 3f;
    private float _canFire = -1;
    private Transform _player;
    private bool _isPlayerBehind;
    private bool _isPlayerAligned;
    [SerializeField]
    private float _initialFireDelay = 1.0f;
    [SerializeField] private float _rearXWindow = 3.0f;
    [SerializeField] private float _rearRangeHeight = 15f;   // how far upward to visualize
    [SerializeField] private float _rearRangeStartOffset = 0.0f; // start above enemy if desired
    [SerializeField] private bool _showRearFireGizmo = true;



    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) _player = p.transform;
        }

        _canFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        bool _isDead = GetComponent<Enemy>().IsDead();

        if (_isDead) return;
        if (_player == null) return;

        _isPlayerBehind = _player != null && transform.position.y < _player.transform.position.y;
        _isPlayerAligned = _player != null && Mathf.Abs(_player.position.x - transform.position.x) <= _rearXWindow;


        if (Time.time > _canFire && _isDead == false)
        {
            EnemyFireLaser();
        }

    }

    private void EnemyFireLaser()
    {
        //Debug.Log($"RearFire? behind={_isPlayerBehind} aligned={_isPlayerAligned} " + $"dx={Mathf.Abs(_player.position.x - transform.position.x):F3} " + $"dy={(_player.position.y - transform.position.y):F3} " + $"window={_rearXWindow}");

        if (_isPlayerBehind == true && _isPlayerAligned == true)
        {
            _fireRate = 1.0f;
            _canFire = Time.time + _fireRate;
            GameObject _enemyLaserBehind = Instantiate(_enemyLazerPrefab, transform.position, Quaternion.identity);
            Laser[] _lasersBehind = _enemyLaserBehind.GetComponentsInChildren<Laser>();

            for (int i = 0; i < _lasersBehind.Length; i++)
            {
                _lasersBehind[i].AssignEnemyLaserBehind();
            }
            Debug.Log("REAR BRANCH");
        }
        else
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
            Debug.Log("FORWARD BRANCH");
        }
    }

    void OnDrawGizmos()
    {
        if (!_showRearFireGizmo) return;

        // Draw a vertical "strip" above the enemy representing the rear-fire alignment window.
        // Width = 2 * _rearXWindow, Height = _rearRangeHeight.
        Vector3 origin = transform.position + Vector3.up * _rearRangeStartOffset;

        float leftX = origin.x - _rearXWindow;
        float rightX = origin.x + _rearXWindow;

        float bottomY = origin.y;
        float topY = origin.y + _rearRangeHeight;

        // Choose color:
        // - Green if rear-fire condition is currently true (in play mode and player exists)
        // - Yellow otherwise
        bool rearCondition = false;
        if (Application.isPlaying && _player != null)
        {
            bool behind = _player.position.y > transform.position.y;
            bool aligned = Mathf.Abs(_player.position.x - transform.position.x) <= _rearXWindow;
            rearCondition = behind && aligned;
        }

        Gizmos.color = rearCondition ? Color.green : Color.yellow;

        // Draw rectangle
        Vector3 bl = new Vector3(leftX, bottomY, 0f);
        Vector3 br = new Vector3(rightX, bottomY, 0f);
        Vector3 tl = new Vector3(leftX, topY, 0f);
        Vector3 tr = new Vector3(rightX, topY, 0f);

        Gizmos.DrawLine(bl, br);
        Gizmos.DrawLine(br, tr);
        Gizmos.DrawLine(tr, tl);
        Gizmos.DrawLine(tl, bl);

        // Optional: draw a line to player so you can see relative position
        if (Application.isPlaying && _player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }

}
