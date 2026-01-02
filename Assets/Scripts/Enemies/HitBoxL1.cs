using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxL1 : MonoBehaviour
{
    [SerializeField]
    private int _hitCount = 3;
    [SerializeField]
    private GameObject _damageLeft;
    [SerializeField]
    private AudioSource _audioSource;
    public bool _isDestroyed = false;
    public HealthSphereColor _sphereColorL1;
    public HealthSphereColor _sphereColorL2;
    public HealthSphereColor _sphereColorL3;
    public Boss _boss;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _boss = GameObject.Find("FinalBoss(Clone)").GetComponent<Boss>();

        if (_boss == null)
        {
            Debug.LogError("Final Boss is NULL!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is NULL");
        }

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
            _sphereColorL1.SetColor(Color.red);
        }
        else if (_hitCount == 1)
        {
            _sphereColorL1.SetColor(Color.red);
            _sphereColorL2.SetColor(Color.red);
        }
        else if (_hitCount <= 0)
        {
            _sphereColorL3.SetColor(Color.red);
            _damageLeft.SetActive(true);
            _audioSource.Play();
            _isDestroyed = true;
            _boss.HitBoxL1Dead();
        }
    }
}
