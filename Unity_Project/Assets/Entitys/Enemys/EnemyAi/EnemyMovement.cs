using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyMovement
{

    Rigidbody2D rb;

    Transform tr;

    public float speed;

    private float distanceTreshold = 0.01f;

    Vector2 smoothVelocity = Vector2.zero;

    public float smoothTime = 0.25f;

    public EnemyMovement(Rigidbody2D rb, Transform tr, float speed, float smoothTime)
    {
        this.rb = rb;

        this.tr = tr;

        this.speed = speed;

        this.smoothTime = smoothTime;
    }

    public EnemyMovement(EnemyMovement movement)
    {
        rb = movement.rb;

        tr = movement.tr;

        speed = movement.speed;

        smoothTime = movement.smoothTime;
    }

  

    public void moveTowards(Vector2 positionToFollow)
    {

        Vector2 startVector = new Vector2(tr.position.x, tr.position.y);

        Vector2 direction = (positionToFollow - startVector).normalized;

        Debug.DrawRay(tr.position, direction * 20, Color.gray);


        bool shoudlStop = (Vector2.Distance(startVector, positionToFollow) <= distanceTreshold) ? false : true;

        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity,direction * speed * Convert.ToInt32(shoudlStop),ref smoothVelocity, smoothTime);
    }



    public void stop()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public bool nearObject(float distanceTreshold, Vector3 center)
    {

        return Vector3.Distance(tr.position, center) <= distanceTreshold;
    }

}



