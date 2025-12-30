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
    public Boss _boss;


    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.Find("FinalBoss").GetComponent<Boss>();
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
        else if (other.tag == "Nuke")
        {
            //Destroy Nuke 
            Destroy(other.gameObject, 4.0f);

            //Nukes hit for 2 dmg
            _hitCount--;
            _hitCount--;
        }

        if (_hitCount == 2)
        {
            _sphereColorR1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorR1.SetColor(Color.red);
            _sphereColorR2.SetColor(Color.red);
        }
        else if (_hitCount <= 0)
        {
            _sphereColorR3.SetColor(Color.red);
            _damageRight.SetActive(true);
            _isDestroyed = true;
            _boss.HitBoxR1Dead();
        }
    }

}
