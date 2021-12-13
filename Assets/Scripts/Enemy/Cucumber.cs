using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamageable
{
    public GameObject heartPre;
    
    //blow off bomb
    public void SetOff()
   {
       targetPoint.GetComponent<Bomb>()?.TurnOff();
   }
    
    //implement IDamageble inerface
    public void GetHit(float damage)
   {
       if (!isDead)
       {
           health -= damage;
           
           if (health < 1)
           {
               health = 0;
               isDead = true;
            
               //update UI
               GameManager.instance.AddScore();
               UIManager.instance.UpdateScore();
               
               SpawnHeart();
           }
           anim.SetTrigger("hit");
       }
   }

   //Spawn heart upon death
   public void SpawnHeart()
   {
       int ran = UnityEngine.Random.Range(0, 2);
       if (ran == 1)
           Instantiate(heartPre, transform.position, heartPre.transform.rotation);
   }
}
