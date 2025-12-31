using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Oscillation")]
    [SerializeField]
    private float _freq = 0.08f;  //Hz, not radians/sec (see below)
    [SerializeField]
    private float _amp = 6.0f;
    [SerializeField]
    private float _oscPhaseTime = 4f;

    [Header("Into Move Durations")]
    [SerializeField]
    private float _moveTimeSegA = 5.0f;
    [SerializeField]    
    private float _moveTimeSegB = 2.0f;
    [SerializeField]
    private float _moveTimeSegC = 1.25f;

    [Header("Phase Transitions")]
    [SerializeField]
    private float _centerMoveTime = 0.75f;
    [SerializeField]
    private float _ramDownTime = 0.6f;
    [SerializeField]
    private float _returnHomeTime = 0.9f;

    [Header("Ram Positions")]
    [SerializeField]
    private Vector3 _homePos = new Vector3(0, 5, 0);
    [SerializeField]
    private Vector3 _ramBottomPos = new Vector3(0, 1, 0); 

    private float _oscTime = 0f;
    [SerializeField]
    private bool _isDead = false;

    [Header("Hit Boxes")]
    public bool _hitBoxL1Dead = false;
    public bool _hitBoxL2Dead = false;
    public bool _hitBoxR1Dead = false;
    public bool _hitBoxR2Dead = false;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        StartCoroutine(BossRoutine());
    }

    private void Update()
    {
        
    }

    IEnumerator BossRoutine ()
    {
        //Boss Intro movements
        yield return StartCoroutine(MoveTo(new Vector3(0, 11, 0), new Vector3(0, 2, 0), _moveTimeSegA));
        yield return StartCoroutine(MoveTo(new Vector3(0, 2, 0), new Vector3(0, 5, 0), _moveTimeSegB));
        yield return StartCoroutine(MoveTo(new Vector3(0, 5, 0), new Vector3(-1, 5, 0), _moveTimeSegC));

        //Establish Home position
        _homePos = transform.position;

        while (!_isDead)
        {
            //Phase 1 - Oscialltion Movement (Ranged Attacks)
            yield return Oscillation(_oscPhaseTime);

            //Phase 2 - Ramming Attack
            yield return MoveTo(transform.position, new Vector3(0, _homePos.y, 0), _centerMoveTime);
            yield return MoveTo(transform.position, _ramBottomPos, _ramDownTime);
            yield return MoveTo(transform.position, _homePos, _returnHomeTime);
        }
    }

    IEnumerator Oscillation (float time)
    {
        _oscTime = 0;
        float _elapsed = 0;

        Vector3 _basePos = new Vector3(_homePos.x, transform.position.y, transform.position.z);

        while (_elapsed < time && !_isDead)
        {
            _elapsed += Time.deltaTime;
            _oscTime += Time.deltaTime;

            float _xOffset = Mathf.Sin(_oscTime * 2f * Mathf.PI * _freq) * _amp;
            transform.position = new Vector3(_basePos.x + _xOffset, _basePos.y, _basePos.z);

            yield return null;
        }
    }

    IEnumerator MoveTo (Vector3 start, Vector3 end, float duration)
    {
        float _elapsed = 0;
        transform.position = start;

        while (_elapsed < duration && !_isDead)
        {
            _elapsed += Time.deltaTime;
            float t =  Mathf.Clamp01(_elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null; 
        }

        transform.position = end;
    }

    private void Death()
    {
        if (_hitBoxL1Dead && _hitBoxL2Dead && _hitBoxR1Dead && _hitBoxR2Dead)
        {
            _isDead = true;
            StopCoroutine(BossRoutine());
            StartCoroutine(BossDeath());
            _uiManager.Victory();
        }
    }

    public bool IsDead()        //Used to call to the Cannons for firing permission
    {
        return this._isDead;
    }

    IEnumerator MoveToDeath(Vector3 start, Vector3 end, float duration)
    {
        float _elapsed = 0;
        transform.position = start;

        while (_elapsed < duration)
        {
            _elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }

    IEnumerator BossDeath()
    {
        while (_isDead)
        {
            yield return StartCoroutine(MoveToDeath(transform.position, new Vector3(0, 5, 0), 8.0f));
            yield return StartCoroutine(MoveToDeath(new Vector3(0, 5, 0), new Vector3(0, 15, 0), 5.0f));
        }
    }

    public void HitBoxL1Dead()
    {
        _hitBoxL1Dead = true;
        Death();
    }

    public void HitBoxL2Dead()
    {
        _hitBoxL2Dead = true;
        Death();
    }

    public void HitBoxR1Dead()
    {
        _hitBoxR1Dead = true;
        Death();
    }

    public void HitBoxR2Dead()
    {
        _hitBoxR2Dead = true;
        Death();
    }
}
