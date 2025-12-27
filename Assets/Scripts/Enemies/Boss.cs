using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _freq = 0.08f;  //Hz, not radians/sec (see below)
    [SerializeField]
    private float _amp = 6.0f;
    [SerializeField]
    private float _moveTimeSegA = 5.0f;
    [SerializeField]    
    private float _moveTimeSegB = 2.0f;
    [SerializeField]
    private float _moveTimeSegC = 1.25f;
    private float _oscTime = 0f;
    private Vector3 _centerPos;
    private bool _canOsciallate = false;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    // Update is called once per frame
    void Update()
    {
        if (_canOsciallate == true)
        {
            BasicMovement();
        }
        
    }

    IEnumerator Intro ()
    {
        yield return StartCoroutine(MoveTo(new Vector3(0, 11, 0), new Vector3(0, 2, 0), _moveTimeSegA));
        yield return StartCoroutine(MoveTo(new Vector3(0, 2, 0), new Vector3(0, 5, 0), _moveTimeSegB));
        yield return StartCoroutine(MoveTo(new Vector3(0, 5, 0), new Vector3(-1, 5, 0), _moveTimeSegC));

        _centerPos = transform.position;
        _oscTime = 0f;
        _canOsciallate = true;
        
    }

    IEnumerator MoveTo (Vector3 start, Vector3 end, float duration)
    {
        float _elapsed = 0;
        transform.position = start;

        while (_elapsed < duration)
        {
            _elapsed += Time.deltaTime;
            float t =  _elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null; 
        }

        transform.position = end;
    }

    void BasicMovement()
    {
        _oscTime += Time.deltaTime;    //This restarts Time so that the oscillation is centered in the scene
        
        //2f * Mathf.PI converts radians/sec to Hz
        float _xPos = (Mathf.Sin(_oscTime * 2f * Mathf.PI * _freq)) * _amp;             
        transform.position = new Vector3(_centerPos.x + _xPos, _centerPos.y, _centerPos.z);
            
    }
}
