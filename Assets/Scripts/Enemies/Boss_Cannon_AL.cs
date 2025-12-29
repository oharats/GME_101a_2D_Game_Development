using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Cannon_AL : MonoBehaviour
{
    [SerializeField]
    private GameObject _cannonPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private float _offsetX = -0.26f;
    private float _offsetY = -0.76f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _canFire)
        {
            FireCannons();
        }
    }

    private void FireCannons()
    {
        _fireRate = Random.Range(1f, 3f);
        _canFire = Time.time + _fireRate;
        Vector3 _position = new Vector3(transform.position.x + _offsetX, transform.position.y + _offsetY, 0);
        GameObject _cannonFire = Instantiate(_cannonPrefab, _position, _cannonPrefab.transform.rotation);
    }
}
