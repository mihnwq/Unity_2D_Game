using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DashAbility : MonoBehaviour
{

    //Handeling the player's dashing ability.


    //Dash duration effects how much the dash travels.
    [SerializeField]
    float dashDuration;

    [SerializeField]
    float dashCooldown;

    [SerializeField]
    float maxDashCooldown;

    [SerializeField]
    KeyCode dashingKey;

    //Dash force manipulates how fast the dash travels.
    [SerializeField]
    float dashForce;
    float relativeObjectMass = 4f;

    public bool dashing = false;

    [SerializeField]
    Collider2D entityCollider;
    [SerializeField]
    Rigidbody2D rb;

    public int maxNumberOfDashes;
    public int currentNumberOfDashes;

    Vector2 dir;

    private void Start()
    {
        currentNumberOfDashes = maxNumberOfDashes;
        dir = new Vector2(0, 0);
    }

    public float chargeSpeed;

    //I will go back to this.
    private void ChargeDashes()
    {
        if (currentNumberOfDashes != maxNumberOfDashes)
        {

            if (!dashing)
                StartCoroutine(ChargeDash(chargeSpeed));
            else StopAllCoroutines();
        }
        
    }

    private IEnumerator ChargeDash(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        currentNumberOfDashes++;

        StopAllCoroutines();
    }

    //Player will be invurnalable for the first dash.
    private void HandleEyeframes()
    {
        if (dashing && HaveUsedEyeframesDash())
            entityCollider.isTrigger = true;
        else entityCollider.isTrigger = false;
        
    }

    public void Update()
    {
        if(!PlayerStates.hadDied)
        {

            if (dashCooldown > 0)
                dashCooldown -= Time.deltaTime;

            if (currentNumberOfDashes != 0 && Input.GetKeyDown(dashingKey) && !dashing)
            {
                Dash();
            }


            ChargeDashes();

            HandleEyeframes();

            HandleDirections();
        }

    }

   
    /// I'm using these for dashing even when the player stays still, in the direction he is looking.
    public float move_x = 0, move_y = 0;

    private void HandleDirections()
    {
        if(PlayerStates.isMoving())
        {
            move_x = PlayerStates.getHorizontal();
            move_y = PlayerStates.getVertical();
        }
    }

    private void Dash()
    {
        if (dashCooldown > 0)
        {
            return;
        }


        float duration = currentNumberOfDashes * dashDuration;

        currentNumberOfDashes--;

        dashing = true;

        dir = new Vector2(move_x,move_y).normalized;

        rb.linearVelocity += (dir * dashForce) / relativeObjectMass;

        Invoke(nameof(ResetDash), duration);
    }

    private void ResetDash()
    {
        dashing = false;
        dashCooldown = maxDashCooldown;
    }

    private bool HaveUsedEyeframesDash()
    {
        return currentNumberOfDashes == (maxNumberOfDashes - 1);
    }
}

