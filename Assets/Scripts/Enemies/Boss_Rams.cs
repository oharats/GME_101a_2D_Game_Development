using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Rams : MonoBehaviour
{
    //Script attached to Boss Ram Hit Boxes

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
