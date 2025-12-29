using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Lasers : MonoBehaviour
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
        //Laser Motion
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

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
