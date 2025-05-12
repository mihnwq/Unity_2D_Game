using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SlimeAnimations : MonoBehaviour

{

    public Animator anim;

    private string currentAnimation;

    private string previousAnimation;

    private SpriteRenderer slimeSprite;

    private SlimeBehaivior bh;

    private int animationNumber;

    Trigonometry trigo;

    Rigidbody2D rb;

    private SlimeBehaivior thisSlime;



    private enum lastState
    {
        UP, DOWN, HORIZONTAL
    }

    private enum currentState
    {
        UP,DOWN,LEFT,RIGHT,IDLE
    }

    lastState state;

    currentState cstate;

    private currentState getState()
    {

        Vector2 direction = new Vector2(bh.target.position.x - transform.position.x, bh.target.position.y - transform.position.y).normalized;

      //  Vector2 direction = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y).normalized;

        float angle = trigo.getClosesstPerpendicular(direction);

        //  if (angle == 0) return currentState.IDLE;

        if (rb.linearVelocity == Vector2.zero) return currentState.IDLE;

        /*  if (angle >= 45 && angle <= 135)
              return currentState.UP;
          else if (angle >= 135 && angle <= 225)
              return currentState.LEFT;
          else if (angle >= 225 && angle <= 315)
              return currentState.DOWN;
          else return currentState.RIGHT;*/

      //  Debug.Log(angle);

        switch (angle)
        {
            case 90:
                return currentState.UP;
            case 180:
                return currentState.LEFT;
            case 270:
                return currentState.DOWN;
        }

        return currentState.RIGHT;

    /*    if (angle == 90)
            return currentState.UP;
        else if (angle == 180)
            return currentState.LEFT;
        else if (angle == 270)
            return currentState.DOWN;
        else return currentState.RIGHT;
    */


    }

    public void Start()
    {
        thisSlime = GetComponent<SlimeBehaivior>();
        trigo = new Trigonometry();
        currentAnimation = "Idle";
        previousAnimation = null;
        state = lastState.DOWN;
        cstate = currentState.DOWN;
        slimeSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bh = GetComponent<SlimeBehaivior>();
    }

    public void Update()
    {
        if(!thisSlime.hasDied)
        {
            if (!GameStates.isGamePaused)
            {
                handleAnimation();
                changeAnimation();
            }
        }else
        {
            anim.Play("Dying");

            Invoke(nameof(TerminateSlime), 0.5f);

        }

        


    }

    private void TerminateSlime()
    {
        anim.speed = 0f;
        Destroy(gameObject);
    }

    void changeAnimation()
    {

        if (currentAnimation != previousAnimation)
            anim.Play(currentAnimation);
    }



    void handleAnimation()
    {
        previousAnimation = currentAnimation;

        cstate = getState();

       if(!bh.isAttacking)
        {
            if (cstate == currentState.RIGHT || cstate == currentState.LEFT)
            {
                state = lastState.HORIZONTAL;

                if (cstate == currentState.RIGHT)
                    slimeSprite.flipX = false;
                else slimeSprite.flipX = true;

                currentAnimation = "MoveRight";
            }
            else if (cstate == currentState.UP)
            {
                state = lastState.UP;
                currentAnimation = "MoveUp";
            }
            else if (cstate == currentState.DOWN)
            {
                state = lastState.DOWN;
                currentAnimation = "MoveDown";
            }
            else
            {
                switch (state)
                {
                    case lastState.HORIZONTAL:
                        currentAnimation = "IdleRight";
                        break;
                    case lastState.UP:
                        currentAnimation = "IdleUp";
                        break;
                    case lastState.DOWN:
                        currentAnimation = "Idle";
                        break;
                }
            }


        }else
        {

            Debug.Log(state);

            if (state == lastState.HORIZONTAL)
            {
                slimeSprite.flipX = (cstate == currentState.RIGHT) ? false : true;

                currentAnimation = "AttackRight";
            }
            else if (state == lastState.UP) currentAnimation = "AttackUp";
            else currentAnimation = "AttackDown";
        }


    }
           
     


    }



