using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderer : MonoBehaviour
{
     //This is temporary.

     public List<RenderedObjects> objects = new List<RenderedObjects>();

     public float distanceTreshold;

     public void OnTriggerEnter2D(Collider2D collision)
     {
        
     }

     public void OnTriggerExit2D(Collider2D collision)
     {

        RenderedObjects currentObject = collision.gameObject.GetComponent<RenderedObjects>();

        currentObject.objName = collision.gameObject.name;
        currentObject.lastPosition = collision.transform.position;

        objects.Add(currentObject);

        collision.gameObject.SetActive(false);
     }

    public void FixedUpdate()
    {
        Vector2 position = transform.position;


       foreach (RenderedObjects obj in objects)
        {
            if ((position - obj.lastPosition).sqrMagnitude < distanceTreshold)
            {
                obj.gameObject.SetActive(true);
              //  objects.Remove(obj);
            }

        }
    }

}
