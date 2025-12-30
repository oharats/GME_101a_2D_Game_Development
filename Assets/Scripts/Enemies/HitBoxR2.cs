using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxR2 : MonoBehaviour
{
    [SerializeField]
    private int _hitCount = 3;
    [SerializeField]
    private GameObject _damageMidRight;
    public bool _isDestroyed = false;
    public HealthSphereColor _sphereColorMR1;
    public HealthSphereColor _sphereColorMR2;
    public HealthSphereColor _sphereColorMR3;
    public Boss _boss;

    // Start is called before the first frame update
    void Start()
    {
        _boss = GameObject.Find("FinalBoss").GetComponent<Boss>();
        _sphereColorMR1 = GameObject.Find("BossMR1").GetComponent<HealthSphereColor>();
        _sphereColorMR2 = GameObject.Find("BossMR2").GetComponent<HealthSphereColor>();
        _sphereColorMR3 = GameObject.Find("BossMR3").GetComponent<HealthSphereColor>();
        _damageMidRight.SetActive(false);
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
            _sphereColorMR1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorMR1.SetColor(Color.red);
            _sphereColorMR2.SetColor(Color.red);
        }
        else if (_hitCount <= 0)
        {
            _sphereColorMR3.SetColor(Color.red);
            _damageMidRight.SetActive(true);
            _isDestroyed = true;
            _boss.HitBoxR2Dead();
        }

    }

}
