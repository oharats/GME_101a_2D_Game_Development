using UnityEngine;

public class EnemyFireB : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyMissilePrefab;
    private float _fireRate = 3f;
    private float _canFire = -1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool _isDead = GetComponent<Enemy>().IsDead();

        if (Time.time > _canFire && _isDead == false)
        {
            EnemyFireMissile();
        }
    }

    private void EnemyFireMissile()
    {
        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;
        GameObject _enemyMissile = Instantiate(_enemyMissilePrefab, transform.position, Quaternion.identity);
    }
}