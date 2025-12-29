using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Laser_AL : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FireLaser();
    }

    private void FireLaser()
    {
        float _angleDeg = 88f;
        float _rad = _angleDeg * Mathf.Deg2Rad;
        Vector3 _direction = new Vector3(Mathf.Cos(_rad), Mathf.Sin(_rad), 0);

        //Laser Motion
        transform.Translate(_direction * _laserSpeed * Time.deltaTime);

        //Destroy Laser Once it exits screen
        if (transform.position.y < -7.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();
            }
        }
    }
}
