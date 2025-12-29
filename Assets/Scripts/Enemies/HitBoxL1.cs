using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxL1 : MonoBehaviour
{
    [SerializeField]
    private int _hitCount = 3;
    [SerializeField]
    private GameObject _damageLeft;
    public bool _isDown = false;
    public HealthSphereColor _sphereColorL1;
    public HealthSphereColor _sphereColorL2;
    public HealthSphereColor _sphereColorL3;
    

    // Start is called before the first frame update
    void Start()
    {
        _sphereColorL1 = transform.Find("BossL1").GetComponent<HealthSphereColor>();
        _sphereColorL2 = transform.Find("BossL2").GetComponent <HealthSphereColor>();
        _sphereColorL3 = transform.Find("BossL3").GetComponent<HealthSphereColor>();
        _damageLeft.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        if (_hitCount == 2)
        {
            _sphereColorL1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorL2.SetColor(Color.red);
        }
        else
        {
            _sphereColorL3.SetColor(Color.red);
            _damageLeft.SetActive(true);
            _isDown = true;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            $"[Trigger ENTER] Hit by: {other.name} | " +
            $"Tag: {other.tag} | " +
            $"Layer: {LayerMask.LayerToName(other.gameObject.layer)}"
        );
    }*/

}
