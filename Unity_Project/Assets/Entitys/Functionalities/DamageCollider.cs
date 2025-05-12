using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public string ID;

    public int maxHitFrames;

    public int hitFrames;

    public int damage;

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == ID)
        {


            ///This is temporary.
            GameObject entity = other.gameObject;

            Entity currentEntity = entity.GetComponent<Entity>();

            currentEntity.OnTakeDamage(damage);

            
        }
    }

    public float damageTime = 0f;

    private float maxDamageTime = 1.5f;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == ID)
        {
          /*  GameObject entity = other.gameObject;

            Entity currentEntity = entity.GetComponent<Entity>();

            if (!currentEntity.hasDied)
                currentEntity.OnTakeDamage(damage);*/
            
            damageTime = 0f;
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == ID )
        {

            if(damageTime <= 0f)
            {
                GameObject entity = other.gameObject;

                Entity currentEntity = entity.GetComponent<Entity>();

                if (!currentEntity.hasDied)
                    currentEntity.OnTakeDamage(damage);
                damageTime = maxDamageTime;
            }
            
           

            damageTime -= Time.deltaTime;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        damageTime = 0f;
    }


}

