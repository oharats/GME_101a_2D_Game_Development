using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Cannons_L : MonoBehaviour
{
    [SerializeField]
    private GameObject _cannonPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private float _offset = -1.1f;

    [SerializeField]
    private int _hitCount = 2;
    [SerializeField]
    private GameObject _damageLeft;
    private bool _isDown = false;

    // Start is called before the first frame update
    void Start()
    {
        _damageLeft.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool _isDead = GameObject.Find("FinalBoss(Clone)").GetComponent<Boss>().IsDead();

        if (!_isDead)
        {
            if (Time.time > _canFire && !_isDown)
            {
                FireCannons();
            }
        }
    }

    //Firing Protocol
    private void FireCannons()
    {
        _fireRate = Random.Range(1f, 3f);
        _canFire = Time.time + _fireRate;
        Vector3 _position = new Vector3(transform.position.x, transform.position.y + _offset, 0);
        GameObject _cannonFire = Instantiate(_cannonPrefab, _position, _cannonPrefab.transform.rotation);
    }

    //Health Protocol
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDown == true)
        {
            return;
        }

        if (other.tag == "Laser")
        {
            //Destroy Laser
            Destroy(other.gameObject);

            _hitCount--;
        }
        else if (other.tag == "Nuke")
        {
            //Destroy Nuke 
            Destroy(other.gameObject, 4.0f);

            //Nukes hit for 2 dmg
            _hitCount--;
            _hitCount--;
        }

        if (_hitCount <= 0)
        {
            _damageLeft.SetActive(true);
            _isDown = true;
        }
    }
}
