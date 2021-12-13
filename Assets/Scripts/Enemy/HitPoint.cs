using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    //call GetHit function when hitpoint makes contact with the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHit(1);
        }
    }
}
