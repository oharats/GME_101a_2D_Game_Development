using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxL2 : MonoBehaviour
{
    [SerializeField]
    private int _hitCount = 3;
    [SerializeField]
    private GameObject _damageMidLeft;
    public bool _isDestroyed = false;
    public HealthSphereColor _sphereColorML1;
    public HealthSphereColor _sphereColorML2;
    public HealthSphereColor _sphereColorML3;
    public Boss _boss;


    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.Find("FinalBoss").GetComponent<Boss>();
        _sphereColorML1 = transform.Find("BossML1").GetComponent<HealthSphereColor>();
        _sphereColorML2 = transform.Find("BossML2").GetComponent<HealthSphereColor>();
        _sphereColorML3 = transform.Find("BossML3").GetComponent<HealthSphereColor>();
        _damageMidLeft.SetActive(false);
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
            _sphereColorML1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorML1.SetColor(Color.red);
            _sphereColorML2.SetColor(Color.red);
        }
        else if (_hitCount <= 0)
        {
            _sphereColorML3.SetColor(Color.red);
            _isDestroyed = true;
            _damageMidLeft.SetActive(true);
            _boss.HitBoxL2Dead();
        }

    }

}
