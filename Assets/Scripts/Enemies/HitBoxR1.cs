using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxR1 : MonoBehaviour
{
    [SerializeField]
    private int _hitCount = 3;
    [SerializeField]
    private GameObject _damageRight;
    public bool _isDestroyed = false;
    public HealthSphereColor _sphereColorR1;
    public HealthSphereColor _sphereColorR2;
    public HealthSphereColor _sphereColorR3;


    // Start is called before the first frame update
    void Start()
    {
        _sphereColorR1 = transform.Find("BossR1").GetComponent<HealthSphereColor>();
        _sphereColorR2 = transform.Find("BossR2").GetComponent<HealthSphereColor>();
        _sphereColorR3 = transform.Find("BossR3").GetComponent<HealthSphereColor>();
        _damageRight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDestroyed == true)
        {
            return;
        }

        if (other.tag == "Laser")
        {
            //Destroy Laser
            Destroy(other.gameObject);

            _hitCount--;
        }

        if (_hitCount == 2)
        {
            _sphereColorR1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorR2.SetColor(Color.red);
        }
        else
        {
            _sphereColorR3.SetColor(Color.red);
            _damageRight.SetActive(true);
            _isDestroyed = true;
        }
    }

}
