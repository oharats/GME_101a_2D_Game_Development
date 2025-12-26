using UnityEngine;

public class EnemyMovementD : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Transform _laser;
    private bool _isLaserFront = false;
    private bool _isLaserAligned = false;
    private bool _threatBox = false;
   
    [Header("Dodge")]
    [SerializeField] private int _dodgeDist = 5;
    [SerializeField] private float _frontXWindow = 1.0f;
    [SerializeField] private float _frontRangeHeight = 10f;   // how far upward to visualize
    [SerializeField] private float _frontRangeStartOffset = 0.0f; // start above enemy if desired
    [SerializeField] private bool _showFrontFireGizmo = true;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //Looks for laser fired each frame
        if (_laser == null)
        {
            GameObject laz = GameObject.FindGameObjectWithTag("Laser");
            if (laz != null) _laser = laz.transform;
        }
        //Establishes the threat box where a lazer is considered dangerous
        _isLaserFront = _laser != null && transform.position.y > _laser.transform.position.y;
        _isLaserAligned = _laser != null && Mathf.Abs(_laser.position.x - transform.position.x) <= _frontXWindow;
        _threatBox = _isLaserAligned && _isLaserFront;

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

        //Dodges the laser if found in the threat box
        if (_threatBox)
        {
            if (transform.position.x > _laser.transform.position.x)
            {
                transform.Translate(new Vector3(_dodgeDist,0,0) * _speed * Time.deltaTime);
            }
            else if (transform.position.x < _laser.transform.position.x)
            {
                transform.Translate(new Vector3(-_dodgeDist, 0, 0) * _speed * Time.deltaTime);
            }
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

    void OnDrawGizmos()
    {
        if (!_showFrontFireGizmo) return;

        // Draw a vertical "strip" above the enemy representing the rear-fire alignment window.
        // Width = 2 * _rearXWindow, Height = _rearRangeHeight.
        Vector3 origin = transform.position + Vector3.up * _frontRangeStartOffset;

        float leftX = origin.x - _frontXWindow;
        float rightX = origin.x + _frontXWindow;

        float topY = origin.y;
        float bottomY = origin.y - _frontRangeHeight;

        // Choose color:
        // - Green if rear-fire condition is currently true (in play mode and player exists)
        // - Yellow otherwise
        bool frontCondition = false;
        if (Application.isPlaying && _laser != null)
        {
            bool infront = _laser.position.y < transform.position.y;
            bool aligned = Mathf.Abs(_laser.position.x - transform.position.x) <= _frontXWindow;
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

        // Optional: draw a line to player so you can see relative position
        if (Application.isPlaying && _laser != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, _laser.position);
        }
    }
}
