using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Knockback : MonoBehaviour
{

    [SerializeField]
    string ID;

    [SerializeField]
    float force;

    [SerializeField]
    Transform center;

    [SerializeField]
    Rigidbody2D attacker;

    Trigonometry trigo = new Trigonometry();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ID)
        {
            GameObject entity = collision.gameObject;

            Vector2 entityPosition = new Vector2(entity.transform.position.x, entity.transform.position.y);

            Vector2 objectPosition = new Vector2(center.position.x, center.position.y);

              Vector2 direction = (entityPosition - objectPosition).normalized;

            Rigidbody2D rb = entity.GetComponent<Rigidbody2D>();

             direction = trigo.rotateToClosestPerpendicular(direction);

            PlayerStates.setDir(direction);

            rb.AddForce(direction *  calculateForce(rb,force));
        }
    }

    float calculateForce(Rigidbody2D rb, float force)
    {
        return force * (rb.mass / 2);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}

/*   float angle = trigo.getAngle(direction.x, direction.y);

               float perpendicularAngle = 90 * Mathf.Round(angle / 90);

               int d = (perpendicularAngle > angle) ? 1 : -1;

               direction = trigo.rotateVector(direction, Mathf.Abs(angle - perpendicularAngle) * d);*/
