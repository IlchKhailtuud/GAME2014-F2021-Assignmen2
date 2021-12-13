using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private BoxCollider2D col;
    
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //add health to player upon contact with the player
            other.GetComponent<PlayerController>().AddHealth();
            Destroy(gameObject);
        }
    }
}
