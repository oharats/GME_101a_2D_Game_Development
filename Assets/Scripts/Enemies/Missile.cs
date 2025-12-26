using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 10.0f;


    // Update is called once per frame
    void Update()
    {
       MoveDown();
    }

    //Enemy Missile
    private void MoveDown()
    {
        //Missile motion
        transform.Translate(Vector3.down * _missileSpeed * Time.deltaTime);

        //Destroy missile game object once it exits the screen
        if (transform.position.y <= -7.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player _player = other.GetComponent<Player>();

            if (_player != null)
            {
                _player.Damage();           //The missile is more powerful than the laser
                _player.Damage();           //so it hits for twice the damage
            }
        }
    }
}
